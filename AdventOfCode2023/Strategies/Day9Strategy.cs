using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;

namespace AdventOfCode2023.Strategies;

public static class Day9
{
}

public class Day9Pt01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 9;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var result = 0l;
        foreach (var line in input)
        {
            var data = line.Split().Where(x => x.Length > 0).Select(double.Parse);
            var xs = Generate.LinearRange(0, 1, data.Count()-1);
  
            var a = Fit.Polynomial( xs.ToArray(),data.ToArray(),  data.Count()-1, DirectRegressionMethod.QR);
            var pol = new Polynomial(a);
            var curX = data.Count()*1.0d;
            var nextValue = pol.Evaluate(curX);
            result += (long)Math.Round(nextValue);
            
        }
        return result.ToString();
    }
}

public class Day9Pt23Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 9;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };


    public string Compute(string[] input, bool debug = false)
    {
        var result = 0l;
        foreach (var line in input)
        {
            var data = line.Split().Where(x => x.Length > 0).Select(double.Parse);
            var xs = Generate.LinearRange(0, 1, data.Count()-1);

            var a = Fit.Polynomial( xs.ToArray(),data.ToArray(),  data.Count()-2); // -2 because numerical errors?
            var pol = new Polynomial(a);
            var nextValue = pol.Evaluate(-1);
            
            result += (long)Math.Round(nextValue);
            
        }
        return result.ToString();
    }
    
}