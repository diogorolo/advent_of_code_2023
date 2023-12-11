using System.Buffers;
using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Day2;

public static class Day2{
    private static Regex greenRegex = new Regex(@"(\d+)(?: green)", RegexOptions.Compiled);
    private static Regex redRegex = new Regex(@"(\d+) red", RegexOptions.Compiled);
    private static Regex blueRegex = new Regex(@"(\d+) blue", RegexOptions.Compiled);
    
    public static (int green, int blue, int red) GetCubes(string input)
    {
        
        return (int.Parse(greenRegex.IsMatch(input) ? greenRegex.Match(input).Groups[1].Value : "0"),
            int.Parse(blueRegex.IsMatch(input) ? blueRegex.Match(input).Groups[1].Value : "0"),
            int.Parse(redRegex.IsMatch(input) ? redRegex.Match(input).Groups[1].Value : "0"));
    }
}

public class Day2Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 2;
    public IEnumerable<int> SupportedParts { get; init; } = new []{2,3};

    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        foreach (var game in input)
        {
            var gameData = game.Split(":");
            var gameDraws = gameData[1];
            (int green, int blue, int red) minCubes = (1, 1, 1);
            foreach (var cubes in gameDraws.Split(";"))
            {
                var drawCubes = Day2.GetCubes(cubes);
                if (drawCubes.green > minCubes.green)
                {
                    minCubes.green = drawCubes.green;
                }
                if ( drawCubes.red > minCubes.red)
                {
                    minCubes.red = drawCubes.red;
                }
                if (drawCubes.blue > minCubes.blue)
                {
                    minCubes.blue = drawCubes.blue;
                }
            }

            result += (minCubes.green * minCubes.blue * minCubes.red);

        }
        return result.ToString();
    }
    
}

public class Day2Part01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 2;
    public IEnumerable<int> SupportedParts { get; init; } = new []{0,1};
    
    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        var maxCubes = Day2.GetCubes(input[0]);
        var games = input[1..];
        foreach (var game in games)
        {
            var gameData = game.Split(":");
            var gameId = int.Parse(gameData[0].Split(" ")[1]);
            var gameDraws = gameData[1];
            var validGame = true;
            foreach (var cubes in gameDraws.Split(";"))
            {
                var drawCubes = Day2.GetCubes(cubes);
                if (drawCubes.green > maxCubes.green || drawCubes.red > maxCubes.red ||
                    drawCubes.blue > maxCubes.blue)
                {
                    validGame = false;
                    break;
                }
            }
            if(validGame)
                result += gameId;

        }
        return result.ToString();
    }
}