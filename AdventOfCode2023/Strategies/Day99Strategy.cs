using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Strategies;

public static class Day99
{
}

public class Day99Pt01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 99;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
       
        return result.ToString();
    }
}

public class Day99Pt23Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 99;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };


    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        
        return result.ToString();
    }
    
}