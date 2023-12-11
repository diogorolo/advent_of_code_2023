using System.Buffers;
using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Day2;

public static class Day3
{
    public static string Digits = "0123456789";
    public static string Symbols = "&+-#@$*/%=";
    public static string SymbolsNoStar = "&+-#@$/%=";
    public static string EndDigits = ".&+-#@$*/%=";

    public static bool HasSymbolNear(string[] input, int line, int startIdx, int endIdx)
    {
        var hasLefSymbol = false;
        startIdx = startIdx - 1;
        if (startIdx < 0)
        {
            startIdx = 0;
        }
        else
        {
            hasLefSymbol = Day3.Symbols.Contains(input[line][startIdx]);
        }

        endIdx = endIdx + 1;
        var hasRightSymbol = false;

        if (endIdx >= input.Length)
        {
            endIdx = input.Length - 1;
        }
        else
        {
            hasRightSymbol = Day3.Symbols.Contains(input[line][endIdx - 1]);
        }

        var topLine = line - 1;
        var hasTopSymbol = false;
        if (topLine >= 0)
        {
            hasTopSymbol = input[topLine][startIdx..endIdx].AsSpan().IndexOfAny(Day3.Symbols) > -1;
        }

        var bottomLine = line + 1;

        var hasBottomSymbol = false;
        if (bottomLine < input.Length)
        {
            hasBottomSymbol = input[bottomLine][startIdx..endIdx].AsSpan().IndexOfAny(Day3.Symbols) > -1;
        }

        return hasLefSymbol || hasRightSymbol || hasTopSymbol || hasBottomSymbol;
    }
}

public class Day3Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 3;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        var lineLength = input[0].Length;
        for (int i = 0; i < input.Length; i++)
        {
            var curIdx = 0;
            if (debug)
            {
                Console.WriteLine($"Analyzing line {i}-> {input[i]}:");
            }

            while (curIdx != -1 && curIdx != lineLength - 1)
            {
                var prevIdx = curIdx;
                var line = input[i][curIdx..].AsSpan();
                var match = line.IndexOfAny(Day3.Digits);
                var matchValue = 0;
                if (match == -1)
                {
                    curIdx = -1;
                    continue;
                }

                var endMatch = line[match..].IndexOfAny(Day3.EndDigits);
                if (endMatch == -1)
                {
                    matchValue = int.Parse(line[match..]);
                    curIdx = -1;
                    endMatch = lineLength;
                }
                else
                {
                    endMatch += match;
                    matchValue = int.Parse(line[match..endMatch]);
                    curIdx = endMatch + prevIdx;
                }

                if (debug)
                {
                    Console.WriteLine($"\t- Found {matchValue} from {match + prevIdx} to {endMatch + prevIdx}");
                }

                var hasSymbolNear = Day3.HasSymbolNear(input, i, match + prevIdx, endMatch + prevIdx);
                if (hasSymbolNear)
                {
                    result += matchValue;
                }
                else
                {
                    if (debug)
                    {
                        Console.WriteLine($"\t\t {matchValue} is NOT a match");
                    }
                }
            }
        }

        return result.ToString();
    }
}

public class Day3Part2Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 3;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };

    public string Compute(string[] input, bool debug = false)
    {
        Regex reg = new Regex($"[{Day3.SymbolsNoStar.Replace("-", "\\-")}]");
        var result = 0;
        string[] convertedLines = new string[input.Length];
        for (var i = 0; i < input.Length; i++)
        {
            convertedLines[i] = reg.Replace(input[i], ".");
        }

        var lineLength = input[0].Length;
        for (int i = 0; i < input.Length; i++)
        {
            var curLine = convertedLines[i];

            if (debug)
            {
                Console.WriteLine($"Analyzing line {i}-> {curLine}:");
            }

            for (int starIdx = curLine.IndexOf('*'); starIdx > -1; starIdx = curLine.IndexOf('*', starIdx + 1))
            {
                var indexes =
                    ((starIdx - 1) < 0 ? 0 : (starIdx - 1))..((starIdx + 1) >= lineLength
                        ? lineLength - 1
                        : (starIdx + 1));
                if (debug)
                {
                    Console.WriteLine($"\tLooking for digits from {indexes}");
                }

                var gears = new List<int>();
                for (int j = -1; j <= 1; j++)
                {
                    if (i + j >= 0 && i + j < input.Length)
                    {
                        var ranges = GetNumbersOnLine(convertedLines, i + j);
                        foreach (var range in ranges)
                        {
                            if (range.Item1.Start.Value <= starIdx+1 && range.Item1.End.Value >= starIdx)
                            {
                                if (debug)
                                {
                                    Console.WriteLine($"\t\tFound number in range in line {i + j}, position {range.Item1}, value {range.Item2}");
                                }
                                gears.Add(range.Item2);
                            }
                        }
                    }
                }

                if (gears.Count > 2)
                {
                    throw new UnreachableException();
                }

                if (gears.Count() == 2)
                {
                    result = result + gears[0] * gears[1];
                }
            }
        }

        return result.ToString();
    }

    private static List<(Range, int)> GetNumbersOnLine(string[] input, int i)
    {
        var data = new List<(Range, int)>();
        var curIdx = 0;
        var lineLength = input[0].Length;
        while (curIdx != -1 && curIdx != lineLength - 1)
        {
            var prevIdx = curIdx;
            var line = input[i][curIdx..].AsSpan();
            var match = line.IndexOfAny(Day3.Digits);
            var matchValue = 0;
            if (match == -1)
            {
                curIdx = -1;
                continue;
            }

            var endMatch = line[match..].IndexOfAny(Day3.EndDigits);
            if (endMatch == -1)
            {
                matchValue = int.Parse(line[match..]);
                curIdx = -1;
                endMatch = lineLength;
            }
            else
            {
                endMatch += match;
                matchValue = int.Parse(line[match..endMatch]);
                curIdx = endMatch + prevIdx;
            }

            data.Add(((match+prevIdx)..(endMatch+prevIdx), matchValue));
        }

        return data;
    }
}