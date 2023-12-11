using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Strategies;

public static class Day5
{
    public static List<List<(long destRangeStart, long sourceRangeStart, long length)>> GetMaps(string[] input )
    {
        var maps = new List<List<(long destRangeStart, long sourceRangeStart, long length )>>();
        maps =
        [
            ..new List<(long destRangeStart, long sourceRangeStart, long length)>[7]
        ];
        for (var index = 1; index < input.Length; index++)
        {
            index = FillMap("seed-to-soil map",0, input, maps, index);
            index = FillMap("soil-to-fertilizer",1, input, maps, index);
            index = FillMap("fertilizer-to-water",2, input, maps, index);
            index = FillMap("water-to-light",3, input, maps, index);
            index = FillMap("light-to-temperature",4, input, maps, index);
            index = FillMap("temperature-to-humidity",5, input, maps, index);
            index = FillMap("humidity-to-location",6, input, maps, index);
        }

        return maps;
    }


    public static int FillMap(string mapType,int mapIdx, string[] input, List<List<(long destRangeStart, long sourceRangeStart, long length)>> maps, int index)
    {
        if (input[index].StartsWith(mapType))
        {
            maps[mapIdx] = new List<(long destRangeStart, long sourceRangeStart, long length)>();
            var dataLine = input[++index];
            while(dataLine.Length > 0){
                FillData(dataLine, maps, mapIdx);
                if (index + 1 >= input.Length)
                {
                    break;
                }
                dataLine = input[++index];
            }
        }

        return index;
    }

    private static void FillData(string dataLine, List<List<(long destRangeStart, long sourceRangeStart, long length)>> maps, int index)
    {
        var a = dataLine.Split().Where(x => x.Length > 0).Select(long.Parse).ToArray();
        maps[index].Add((a[0],a[1],a[2]));
    }
}

public class Day5Pt01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 5;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var result = long.Parse("0");
        var seeds = input[0].Split(":")[1].Split().Where(x => x.Length > 0).Select(long.Parse).ToArray();
        var maps = Day5.GetMaps(input);
        var location = new List<long>();
        foreach (var seed in seeds)
        {
            var curIteration = seed;
            foreach (var map in maps)
            {
                foreach (var mapping in map)
                {
                    if (mapping.sourceRangeStart <= curIteration && curIteration <= mapping.sourceRangeStart + mapping.length)
                    {
                        curIteration = mapping.destRangeStart + (curIteration - mapping.sourceRangeStart);
                        break;
                    }
                }
            }
            location.Add(curIteration);
        }

        result = location.Min();
        return result.ToString();
    }

}

public class Day5Pt23Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 5;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };
    
    public string Compute(string[] input, bool debug = false)
    {
        var seedRanges = input[0].Split(":")[1].Split().Where(x => x.Length > 0).Select(long.Parse).ToArray();
        var maps = Day5.GetMaps(input);
        var location = long.MaxValue;
        Parallel.For(0, seedRanges.Length, i =>
        {
            if (i % 2 != 0)
            {
                return;
            }

            Parallel.For(seedRanges[i], seedRanges[i] + seedRanges[i + 1], j =>
            {
                var curIteration = j;
                foreach (var map in maps)
                {
                    foreach (var mapping in map)
                    {
                        if (mapping.sourceRangeStart <= curIteration &&
                            curIteration <= mapping.sourceRangeStart + mapping.length)
                        {
                            curIteration = mapping.destRangeStart + (curIteration - mapping.sourceRangeStart);
                            break;
                        }
                    }
                }

                if (curIteration < location)
                {
                    location = curIteration;
                }
            });

        });
        return location.ToString();
    }
    
}