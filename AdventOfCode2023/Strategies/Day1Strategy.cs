using System.Buffers;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day1;

public class Day1Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 1;
    public IEnumerable<int> SupportedParts { get; init; } = new []{2,3};

    private static string _digits = "0123456789";
    private SearchValues<char> _searchValues = SearchValues.Create(_digits);

    private IEnumerable<string> numbers = new[]
    {
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
    };
    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        foreach (var line in input)
        {

            var firstOccurence = GetFirstOccurenceValue(line);
            var lastOccurence = GetLastOccurenceValue(line);
            var lineResult = int.Parse(
                $"{firstOccurence}{lastOccurence}");
            result += lineResult;
            if(debug)
                Console.WriteLine($"Found {lineResult} from {line}");
        }

        return result.ToString();
    }
    
    private string GetLastOccurenceValue(string line)
    {
        int occurence = line.AsSpan().LastIndexOfAny(_searchValues);
        var curValue = occurence == -1 ? ' ' : line[occurence];
        for(int i = 0 ; i < numbers.Count(); i++)
        {
            var curValIndex = line.LastIndexOf(numbers.ElementAt(i), StringComparison.Ordinal);
            if(curValIndex == -1)
                continue;
            if (curValIndex > occurence|| occurence == -1)
            {
                occurence = curValIndex;
                curValue = (i+1).ToString()[0];
            }
        }

        return curValue.ToString();
    }
    
    private string GetFirstOccurenceValue(string line)
    {
        int occurence = line.AsSpan().IndexOfAny(_searchValues);
        var curValue = occurence == -1 ? ' ' : line[occurence];
        for(int i = 0 ; i < numbers.Count(); i++)
        {
            var curValIndex = line.IndexOf(numbers.ElementAt(i), StringComparison.Ordinal);
            if(curValIndex == -1)
                continue;
            if (curValIndex < occurence || occurence == -1)
            {
                occurence = curValIndex;
                curValue = (i+1).ToString()[0];
            }
        }

        return curValue.ToString();
    }
}


public class Day1Part01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 1;
    public IEnumerable<int> SupportedParts { get; init; } = new []{0,1};

    private static string _digits = "0123456789";
    private SearchValues<char> _searchValues = SearchValues.Create(_digits);
    
    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        foreach (var line in input)
        {

            var firstOccurence = GetFirstOccurenceValue(line);
            var lastOccurence = GetLastOccurenceValue(line);
            var lineResult = int.Parse(
                $"{firstOccurence}{lastOccurence}");
            result += lineResult;
            if(debug)
                Console.WriteLine($"Found {lineResult} from {line}");
        }

        return result.ToString();
    }
    
    private string GetLastOccurenceValue(string line)
    {
        int occurence = line.AsSpan().LastIndexOfAny(_searchValues);
        var curValue = occurence == -1 ? ' ' : line[occurence];

        return curValue.ToString();
    }
    
    private string GetFirstOccurenceValue(string line)
    {
        int occurence = line.AsSpan().IndexOfAny(_searchValues);
        var curValue = occurence == -1 ? ' ' : line[occurence];
        return curValue.ToString();
    }
}