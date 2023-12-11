using System.Text.RegularExpressions;
using AdventOfCode2023.Day1;

namespace AdventOfCode2023.Strategies;

public static class Day10
{
    public static (int x, int y) GetNorth(int x, int y) => (x, y - 1);
    public static (int x, int y) GetSouth(int x, int y) => (x, y + 1);
    public static (int x, int y) GetWest(int x, int y) => (x - 1, y);
    public static (int x, int y) GetEast(int x, int y) => (x + 1, y);

    public static int GetHammingDistance((int x, int y) c1, (int x, int y) c2) =>
        Math.Abs(c1.x - c2.x) + Math.Abs(c1.y - c2.y);

    public static int GetHammingDistance(Node10 c1, Node10 c2) =>
        GetHammingDistance((c1.X, c1.Y), (c2.X, c2.Y));

    // public static Node10? GetNode(IEnumerable<Node10> nodes, (int x, int y) coords) =>
    //     nodes.SingleOrDefault(x => x.X == coords.x && x.Y == coords.y);

    public static Node10? GetNode(Dictionary<int, Node10?> nodes, (int x, int y) coords)
    {
        return nodes.ContainsKey(GetKey(coords)) ? nodes[GetKey(coords)] : null;
    }

    public static int GetKey(Node10 node) => node.X * 10000 + node.Y;
    public static int GetKey((int X, int Y) coords) => coords.X * 10000 + coords.Y;

    public static void Init(string[] input, ref Dictionary<int, Node10> nodes)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                var node = new Node10()
                {
                    X = j,
                    Y = i,
                    Val = input[i][j],
                };
                if (input[i][j] == 'S')
                {
                    node.IsStarting = true;
                }

                nodes[Day10.GetKey(node)] = node;
            }
        }
    }

    public static void GetMap(string[] input, ref Dictionary<int, Node10> nodes)
    {
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                var node = Day10.GetNode(nodes, (j, i))!;
                if (input[i][j] == '|')
                {
                    var node1 = Day10.GetNode(nodes, Day10.GetNorth(j, i));
                    if (node1 is not null)
                        node.ConnectedNodes.Add(node1);
                    var node2 = Day10.GetNode(nodes, Day10.GetSouth(j, i));
                    if (node2 is not null)
                        node.ConnectedNodes.Add(node2);
                }
                else if (input[i][j] == '-')
                {
                    var node1 = Day10.GetNode(nodes, Day10.GetWest(j, i));
                    if (node1 is not null)
                        node.ConnectedNodes.Add(node1);
                    var node2 = Day10.GetNode(nodes, Day10.GetEast(j, i));
                    if (node2 is not null)
                        node.ConnectedNodes.Add(node2);
                }
                else if (input[i][j] == 'L')
                {
                    var node1 = Day10.GetNode(nodes, Day10.GetNorth(j, i));
                    if (node1 is not null)
                        node.ConnectedNodes.Add(node1);
                    var node2 = Day10.GetNode(nodes, Day10.GetEast(j, i));
                    if (node2 is not null)
                        node.ConnectedNodes.Add(node2);
                }
                else if (input[i][j] == 'J')
                {
                    var node1 = Day10.GetNode(nodes, Day10.GetNorth(j, i));
                    if (node1 is not null)
                        node.ConnectedNodes.Add(node1);
                    var node2 = Day10.GetNode(nodes, Day10.GetWest(j, i));
                    if (node2 is not null)
                        node.ConnectedNodes.Add(node2);
                }
                else if (input[i][j] == '7')
                {
                    var node1 = Day10.GetNode(nodes, Day10.GetSouth(j, i));
                    if (node1 is not null)
                        node.ConnectedNodes.Add(node1);
                    var node2 = Day10.GetNode(nodes, Day10.GetWest(j, i));
                    if (node2 is not null)
                        node.ConnectedNodes.Add(node2);
                }
                else if (input[i][j] == 'F')
                {
                    var node1 = Day10.GetNode(nodes, Day10.GetSouth(j, i));
                    if (node1 is not null)
                        node.ConnectedNodes.Add(node1);
                    var node2 = Day10.GetNode(nodes, Day10.GetEast(j, i));
                    if (node2 is not null)
                        node.ConnectedNodes.Add(node2);
                }
            }
        }

        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                var node = Day10.GetNode(nodes, (j, i))!;
                if (input[i][j] == 'S')
                {
                    var coords = Day10.GetNorth(j, i);
                    try
                    {
                        if (input[coords.y][coords.x] is '|' or '7' or 'F')
                        {
                            var node1 = Day10.GetNode(nodes, coords);
                            if (node1 is not null)
                                node.ConnectedNodes.Add(node1);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    coords = Day10.GetSouth(j, i);
                    try
                    {
                        if (input[coords.y][coords.x] is '|' or 'L' or 'J')
                        {
                            var node1 = Day10.GetNode(nodes, coords);
                            if (node1 is not null)
                                node.ConnectedNodes.Add(node1);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    coords = Day10.GetWest(j, i);
                    try
                    {
                        if (input[coords.y][coords.x] is '-' or 'L' or 'F')
                        {
                            var node1 = Day10.GetNode(nodes, coords);
                            if (node1 is not null)
                                node.ConnectedNodes.Add(node1);
                        }
                    }
                    catch (Exception)
                    {
                    }

                    try
                    {
                        coords = Day10.GetEast(j, i);
                        if (input[coords.y][coords.x] is '-' or 'J' or '7')
                        {
                            var node1 = Day10.GetNode(nodes, coords);
                            if (node1 is not null)
                                node.ConnectedNodes.Add(node1);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
    }
}

public class Day10Pt01Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 10;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 0, 1 };

    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        var nodes = new Dictionary<int, Node10>();
        Day10.Init(input, ref nodes);
        Day10.GetMap(input, ref nodes);

        var startingNode = nodes.Single(x => x.Value.IsStarting).Value;
        var curNode = startingNode.ConnectedNodes.First();
        int lastX = startingNode.X;
        int lastY = startingNode.Y;
        var steps = 1;
        do
        {
            steps++;
            var nextNode = curNode.ConnectedNodes.Single(x => !(x.X == lastX && x.Y == lastY));
            lastX = curNode.X;
            lastY = curNode.Y;
            curNode = nextNode;
        } while (!curNode.Equals(startingNode));

        result = steps / 2;
        return result.ToString();
    }
}

public class Node10
{
    public int X { get; set; }
    public int Y { get; set; }
    public char Val { get; set; }
    public bool IsStarting { get; set; } = false;

    public List<Node10> ConnectedNodes { get; set; } = new List<Node10>();

    public override string ToString()
    {
        return $"X={X},Y={Y},Nodes={ConnectedNodes.Count},{Val}";
    }

    public override int GetHashCode()
    {
        return X * 10000 + Y;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Node10 other)
            return false;
        return other.X == X && other.Y == Y;
        return base.Equals(other);
    }
}

public class Day10Pt23Strategy : IDayComputerStrategy
{
    public int SupportedDay { get; init; } = 10;
    public IEnumerable<int> SupportedParts { get; init; } = new[] { 2, 3 };

    public int wrap(int x, int x_max, int x_min) => (((x - x_min) % (x_max - x_min)) + (x_max - x_min)) % (x_max - x_min) + x_min;

    bool IsInside(List<Node10> loop, (int x, int y) coords)
    {
        var intersects = 0;
        var onEdge = false;
        List<int> analyzed = new List<int>();
        for (int i = coords.x; i >= 0; i--)
        {
            Node10 cNode = null;
            Node10 nNode = null;
            Node10 pNode = null;
            for (int k = 0; k < loop.Count; k++)
            {
                if (analyzed.Contains(k))
                {
                    continue;
                }
                cNode = loop.ElementAt(k);
                if (cNode.X == i && cNode.Y == coords.y)
                {
                    int blah = k;
                    do
                    {
                        nNode = loop.ElementAt(wrap(++blah, 0, loop.Count-1));
                        if(nNode.Y == cNode.Y)
                            analyzed.Add(blah);
                    } while (nNode.Y == cNode.Y);
                    blah = k;
                    do
                    {
                        pNode = loop.ElementAt(wrap(--blah, 0, loop.Count-1));
                        if(pNode.Y == cNode.Y)
                            analyzed.Add(blah);
                    } while (pNode.Y == cNode.Y);
                    break;
                }
            }
            if (nNode is not null)
            {
                if (cNode.Y != nNode.Y && pNode.Y != nNode.Y)
                {
                    intersects++;
                }
                // if (node.Val is '|')
                // {
                //     intersects++;
                //     continue;
                // }

               
            }
        }
        

        return intersects % 2 != 0;
    }


    public string Compute(string[] input, bool debug = false)
    {
        var result = 0;
        var nodes = new Dictionary<int, Node10>();

        Day10.Init(input, ref nodes);
        Day10.GetMap(input, ref nodes);

        var startingNode = nodes.Single(x => x.Value.IsStarting).Value;
        var curNode = startingNode.ConnectedNodes.First();
        int lastX = startingNode.X;
        int lastY = startingNode.Y;
        Dictionary<int, Node10> nodesInLoop = new Dictionary<int, Node10>();
        nodesInLoop[Day10.GetKey(startingNode)] = startingNode;
        nodesInLoop[Day10.GetKey(startingNode.ConnectedNodes.First())] = startingNode.ConnectedNodes.First();
        nodesInLoop[Day10.GetKey(startingNode.ConnectedNodes.Last())] = startingNode.ConnectedNodes.Last();
        List<Node10> loop = new List<Node10>();
        loop.Add(startingNode);
        do
        {
            var nextNode = curNode.ConnectedNodes.Single(x => !(x.X == lastX && x.Y == lastY));
            loop.Add(curNode);
            lastX = curNode.X;
            lastY = curNode.Y;
            curNode = nextNode;
            nodesInLoop[Day10.GetKey(curNode)] = curNode;
        } while (!curNode.Equals(startingNode));

        loop.Add(startingNode.ConnectedNodes.Last());

        nodesInLoop[Day10.GetKey(curNode)] = curNode;
        var inside = 0;
        Parallel.For(0, input.Length, i =>
        {
            Parallel.For(0, input[i].Length, j =>
            {
                if (nodesInLoop.ContainsKey(Day10.GetKey((j, i))))
                {
                    if (debug)
                    {
                        Console.Write(nodesInLoop[Day10.GetKey((j, i))].Val);
                    }

                    //Console.Write("O");
                    return;
                }

                var isInside = IsInside(loop, (j, i));
                if (isInside)
                {
                    if (debug)
                    {
                        Console.Write("I");
                    }

                    inside++;
                }
                else
                {
                    if (debug)
                    {
                        Console.Write("O");
                    }
                }
                //Console.Write(".");
            });

            if (debug)
            {
                Console.WriteLine();
            }
        });

        return inside.ToString();
    }
}