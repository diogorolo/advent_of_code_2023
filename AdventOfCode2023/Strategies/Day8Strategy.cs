using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Strategies;

public static class Day8
{
    public static long gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    public static long lcm(long a, long b)
    {
        return (a / gcf(a, b)) * b;
    }

    public static long lcm(long[] nums)
    {
        long lcmVal = 1;

        for (int i = 0; i < nums.Length; i++)
        {
            lcmVal = lcm(lcmVal, nums[i]);
        }

        return lcmVal;
    }
    public static List<Node8> GetNodes(string[] input)
    {
        var nodes = new List<Node8>();
        for (int i = 2; i < input.Length; i++)
        {
            var line = input[i].Replace(" ", "").Split("=");

            var node = nodes.SingleOrDefault(x => x.Name == line[0]) ?? new Node8
            {
                Name = line[0],
                EndsWithZ = line[0].EndsWith("Z")
            };

            nodes.Add(node);
        }

        for (int i = 2; i < input.Length; i++)
        {
            var line = input[i].Replace(" ", "").Split("=");

            var node = nodes.Single(x => x.Name == line[0]);

            var connectedNodes =
                line[1].Replace("(", "").Replace(")", "").Split(",").Where(x => x.Length > 0).ToArray();
            var leftNode = nodes.Single(x => x.Name == connectedNodes[0]);
            var rightNode = nodes.Single(x => x.Name == connectedNodes[1]);
            node.LeftNode8 = leftNode;
            node.RightNode8 = rightNode;
        }

        return nodes;
    }
}

public record Node8()
{
    public required string Name { get; init; }
    public bool EndsWithZ { get; init; }
    public Node8 LeftNode8 { get; set; }
    public Node8 RightNode8 { get; set; }
    public int LoopDistance { get; set; }
}

public class Day8Pt01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 8;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var instructions = input[0].ToCharArray();
        var nodes = Day8.GetNodes(input);

        var steps = 0;
        var curNode = nodes.Single(x => x.Name == "AAA");
        while (curNode.Name != "ZZZ")
        {
            var curInstruction = instructions[steps % instructions.Length];
            if (debug)
            {
                Console.WriteLine($"Step {steps}, Instruction: {curInstruction} Node {curNode.Name}");
            }

            curNode = curInstruction == 'L' ? curNode.LeftNode8 : curNode.RightNode8;
            steps++;
        }

        return steps.ToString();
    }
}

public class Day8Pt23Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 8;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };


    public string Compute(string[] input, bool debug = false)
    {
        var instructions = input[0].ToCharArray();
        var nodes = Day8.GetNodes(input);

        var startNodes = nodes.Where(x => x.Name.EndsWith("A")).ToArray();
        var distancesToZ = new List<long>();

        foreach (var node in startNodes)
        {
            var steps = 0;
            var curNode = node;
            do
            {
                if (curNode.EndsWithZ)
                {
                    distancesToZ.Add(steps);
                    break;
                }
                var curInstruction = instructions[steps % instructions.Length];
                curNode = curInstruction == 'L' ? curNode.LeftNode8 : curNode.RightNode8;
                
                steps++;
            } while (curNode != node);

            //node.LoopDistance = steps;
        }

        var result = Day8.lcm(distancesToZ.ToArray());
        return result.ToString();
    }
}