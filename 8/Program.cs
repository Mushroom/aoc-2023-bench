using System.Collections.Immutable;
using System.Text.RegularExpressions;

var inputFromFile = File.ReadAllLines("input.txt");

var directions = inputFromFile[0];

var nodes = inputFromFile.Skip(2).Select(x => Regex.Matches(x, "\\w+")).Select(x => new Node(x[0].Value, x[1].Value, x[2].Value)).ToImmutableDictionary(x => x.name);

void Part1()
{
    Console.WriteLine(GetPathLength("AAA", true));
}

void Part2()
{
    // Get the length of each ghost's path
    var lengths = nodes.Values.Where(x => x.name.EndsWith('A'))
                    .Select(x => GetPathLength(x.name, false));

    // Get the LCM of the paths, which gives the answer
    // LCM algo lifted wholesale from https://stackoverflow.com/a/29717490
    Console.WriteLine(lengths.Aggregate((x, y) => Math.Abs(x * y) / GCD(x, y)));
}

long GetPathLength(string startNode, bool isPart1)
{
    Node currentNode = nodes[startNode];
    long directionIndex = 0;

    while(true)
    {
        currentNode = nodes[(directions[(int)(directionIndex % directions.Length)] == 'L') ? currentNode.L : currentNode.R];

        directionIndex++;

        // Return our index if we hit the condition for the part we're on
        // Ie, "ZZZ" for P1, ending in "Z" for P2
        if((isPart1 && currentNode.name == "ZZZ") || (!isPart1 && currentNode.name.EndsWith('Z'))) return directionIndex;
    }
}

long GCD(long x, long y)
{
    return y == 0 ? x : GCD(y, x % y);
}

Part1();
Part2();

public readonly record struct Node(string name, string L, string R);