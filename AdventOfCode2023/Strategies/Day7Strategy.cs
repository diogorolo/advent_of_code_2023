using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Strategies;

public static class Day7
{
}

public class Day7Pt01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 7;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)> hands = new(input.Length);
        foreach (var line in input)
        {
            var parts = line.Split();
            var s = parts[0].Replace('A', 'Z').Replace('K', 'Y').Replace('Q', 'W').Replace('J', 'V').Replace('T', 'U');
            var hand = s.ToCharArray().GroupBy(x => x);
            var bid = int.Parse(parts[1]);
            hands.Add((hand, bid, s));
        }

        var fiveKind = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)>();
        var fourKind = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)>();
        var pair = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)>();
        var tpair = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)>();
        var trio = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)>();
        var fhouse = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)>();
        var hcard = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand)>();
        foreach (var hand in hands)
        {
            if (hand.hand.Count() == 1)
            {
                fiveKind.Add(hand);
            }

            if (hand.hand.Count() == 2)
            {
                // full house or four kind + high card
                if (hand.hand.Max(x => x.Count()) == 4)
                {
                    fourKind.Add(hand);
                }
                else
                {
                    fhouse.Add(hand);
                }
            }

            if (hand.hand.Count() == 3)
            {
                //2 pair or trio + 2 high cards
                if (hand.hand.Max(x => x.Count()) == 3)
                {
                    trio.Add(hand);
                }
                else
                {
                    tpair.Add(hand);
                }
            }

            if (hand.hand.Count() == 4)
            {
                pair.Add(hand);
            }

            if (hand.hand.Count() == 5)
            {
                hcard.Add(hand);
            }
        }

        List<int> winnings = new List<int>();

        winnings.AddRange(hcard.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(pair.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(tpair.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(trio.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(fhouse.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(fourKind.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(fiveKind.OrderBy(x => x.hhand).Select(x => x.bid));

        var result = winnings.Select((t, i) => (t * (i + 1))).Sum();
        return result.ToString();
    }
}

public class Day7Pt23Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 7;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };


    public string Compute(string[] input, bool debug = false)
    {
        var wildcard = '!';
        List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)> hands = new(input.Length);
        foreach (var line in input)
        {
            var parts = line.Split();
            var s = parts[0].Replace('A', 'Z').Replace('K', 'Y').Replace('Q', 'W').Replace('J', wildcard)
                .Replace('T', 'U');
            var hand = s.ToCharArray().GroupBy(x => x);
            var bid = int.Parse(parts[1]);
            hands.Add((hand, bid, s, parts[0], hand.FirstOrDefault(x => x.Key == wildcard)?.Count() ?? 0));
        }

        var fiveKind = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)>();
        var fourKind = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)>();
        var pair = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)>();
        var tpair = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)>();
        var trio = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)>();
        var fhouse = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)>();
        var hcard = new List<(IEnumerable<IGrouping<char, char>> hand, int bid, string hhand, string hhhand, int jokerCount)>();
        foreach (var hand in hands)
        {
            if (hand.hand.Count() == 1)
            {
                fiveKind.Add(hand);
            }

            if (hand.hand.Count() == 2)
            {
                // full house or four kind + high card
                if (hand.hand.Max(x => x.Count()) == 4)
                {
                    // 4kind
                    if (hand.jokerCount is 1 or 4)
                    {
                        fiveKind.Add(hand);
                    }
                    else
                    {
                        fourKind.Add(hand);
                    }
                }
                else
                {
                    
                    //fhouse
                    if (hand.jokerCount is 2 or 3)
                    {
                        fiveKind.Add(hand);
                    }
                    else
                    {
                        fhouse.Add(hand);
                    }
                }
            }

            if (hand.hand.Count() == 3)
            {
                //2 pair + high card or trio + 2 high cards
                if (hand.hand.Max(x => x.Count()) == 3)
                {
                    // trio
                    if (hand.jokerCount is 1 or 3)
                    {
                        fourKind.Add(hand);
                    }
                    else if (hand.jokerCount is 2)
                    {
                        fiveKind.Add(hand);
                    }
                    else
                    {
                        trio.Add(hand);
                    }
                }
                else
                {
                    // 2 pairs + high card
                    if (hand.jokerCount == 1)
                    {
                        fhouse.Add(hand);
                    }
                    else if (hand.jokerCount == 2)
                    {
                        fourKind.Add(hand);
                    }
                    else
                    {
                        tpair.Add(hand);
                    }
                }
            }

            if (hand.hand.Count() == 4)
            {
                if (hand.jokerCount == 1)
                {
                    trio.Add(hand);
                }
                else if (hand.jokerCount == 2)
                {
                    trio.Add(hand);
                }
                else if (hand.jokerCount == 3)
                {
                    fourKind.Add(hand);
                }
                else if (hand.jokerCount == 4)
                {
                    fiveKind.Add(hand);
                }
                else
                {
                    pair.Add(hand);
                }
            }

            if (hand.hand.Count() == 5)
            {
                if (hand.jokerCount == 1)
                {
                    pair.Add(hand);
                }
                else if (hand.jokerCount == 2)
                {
                    trio.Add(hand);
                }
                else if (hand.jokerCount == 3)
                {
                    fourKind.Add(hand);
                }
                else if (hand.jokerCount == 4)
                {
                    fiveKind.Add(hand);
                }
                else
                {
                    hcard.Add(hand);
                }
            }
        }

        List<int> winnings = new List<int>();

        winnings.AddRange(hcard.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(pair.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(tpair.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(trio.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(fhouse.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(fourKind.OrderBy(x => x.hhand).Select(x => x.bid));
        winnings.AddRange(fiveKind.OrderBy(x => x.hhand).Select(x => x.bid));

        if (debug)
        {
            Console.WriteLine("High card");
            foreach (var card in hcard.OrderBy(x => x.hhand))
            {
                Console.WriteLine($"{card.hhhand}");
            }

            Console.WriteLine("Pair");
            foreach (var card in pair.OrderBy(x => x.hhand))
            {
                Console.WriteLine($"{card.hhhand}");
            }

            Console.WriteLine("2Pair");

            foreach (var card in tpair.OrderBy(x => x.hhand))
            {
                Console.WriteLine($"{card.hhhand}");
            }

            Console.WriteLine("Trio");

            foreach (var card in trio.OrderBy(x => x.hhand))
            {
                Console.WriteLine($"{card.hhhand}");
            }

            Console.WriteLine("Full house");

            foreach (var card in fhouse.OrderBy(x => x.hhand))
            {
                Console.WriteLine($"{card.hhhand}");
            }

            Console.WriteLine("4kind");

            foreach (var card in fourKind.OrderBy(x => x.hhand))
            {
                Console.WriteLine($"{card.hhhand}");
            }

            Console.WriteLine("5kind");
            foreach (var card in fiveKind.OrderBy(x => x.hhand))
            {
                Console.WriteLine($"{card.hhhand}");
            }
        }

        var result = winnings.Select((t, i) => (t * (i + 1))).Sum();
        return result.ToString();
    }
}