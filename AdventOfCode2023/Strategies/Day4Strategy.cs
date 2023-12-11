using System.Buffers;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Day2;

public static class Day4
{
}

public class Day4Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 4;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        Regex reg = new Regex("\\s+", RegexOptions.Compiled);
        int i = 0;
        foreach (var line in input)
        {
            var cardPoints = 0;
            var parsedLine = reg.Replace(line, " ");
            var numbers = parsedLine.Split(":")[1].Split("|").Select(x => x.Trim()).ToArray();
            var winNumbers = numbers[0].Split(" ").Select(x => x.Trim()).Select(int.Parse).ToArray();
            var drawNumbers = numbers[1].Split(" ").Select(x => x.Trim()).Select(int.Parse);
            foreach (var number in drawNumbers)
            {
                if (winNumbers.Contains(number))
                {
                    if (cardPoints == 0)
                    {
                        cardPoints++;
                    }
                    else
                    {
                        cardPoints *= 2;
                    }
                }
            }

            if (debug)
            {
                Console.WriteLine($"Card {i} is worth {cardPoints}");
            }
            result += cardPoints;
        }
        return result.ToString();
    }
}

public class Day4Part2Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 4;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };

    private List<int> cards;

    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        Regex reg = new Regex("\\s+", RegexOptions.Compiled);
        cards = new List<int>(new int[input.Length]);
        cards = IncrementRange(cards, .., 1);
        int i = 0;
        foreach (var line in input)
        {
            var parsedLine = reg.Replace(line, " ");
            var numbers = parsedLine.Split(":")[1].Split("|").Select(x => x.Trim()).ToArray();
            var winNumbers = numbers[0].Split(" ").Select(x => x.Trim()).Select(int.Parse).ToArray();
            var drawNumbers = numbers[1].Split(" ").Select(x => x.Trim()).Select(int.Parse);
            var cardPoints = drawNumbers.Count(number => winNumbers.Contains(number));

            if (debug)
            {
                Console.WriteLine($"Card {i} is worth {cardPoints}");
            }

            cards = IncrementRange(cards, (i + 1)..(i + cardPoints + 1), cards[i]);
            i++;
        }

        for (var index = 0; index < cards.Count; index++)
        {
            var card = cards[index];
            if (debug)
            {
                Console.WriteLine($"Total Cards {index}: {card}");
            }
        }

        return cards.Sum().ToString();
    }

    private List<int> IncrementRange(List<int> cards, Range range, int times)
    {
        var(off,length) = range.GetOffsetAndLength(cards.Count);
        for (var index = off; index < off+length; index++)
        {
            cards[index]+=times;
        }

        return cards;
    }
}