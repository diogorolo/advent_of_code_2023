using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Strategies;

public static class Day6
{
}

public class Day6Pt01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 6;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var time = input[0].Split(":")[1].Split().Where(x => x.Length > 0).Select(int.Parse).ToArray();
        var distance = input[1].Split(":")[1].Split().Where(x => x.Length > 0).Select(int.Parse).ToArray();

        List<int> ways2win = new List<int>(new int[time.Length]);
        for (int race = 0; race < time.Length; race++)
        {
            for (int i = 0; i < time[race]; i++)
            {
                if (((time[race] - i) * i) > distance[race])
                {
                    ways2win[race]++;
                }
            }
        }

        var result = ways2win.Aggregate(1, (current, i) => current * i);
        return result.ToString();
    }
}

public class Day6Pt23Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 6;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };


    public string Compute(string[] input, bool debug = false)
    {
        var time = input[0].Replace(" ", "").Split(":")[1].Split().Where(x => x.Length > 0).Select(long.Parse).ToArray();
        var distance = input[1].Replace(" ", "").Split(":")[1].Split().Where(x => x.Length > 0).Select(long.Parse).ToArray();

        List<long> ways2win = [..new long[time.Length]];
        for (int race = 0; race < time.Length; race++)
        {
            for (int i = 0; i < time[race]; i++)
            {
                if (((time[race] - i) * i) > distance[race])
                {
                    ways2win[race]++;
                }
            }
        }
        var result = ways2win.Aggregate((long)1, (current, i) => current * i);
        return result.ToString();
    }
    
}