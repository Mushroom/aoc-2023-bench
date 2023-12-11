using System.Collections.Immutable;

var inputFromFile = File.ReadAllLines("input.txt");

// Split out into immutable list of long arrays
var inputLists = inputFromFile.Select(x => x.Split(' ').Select(long.Parse)).ToImmutableList();

void Part1And2()
{
    long sumP1 = 0;
    long sumP2 = 0;
    foreach(var inputLine in inputLists)
    {
        // P2 is basically just P1 but backwards, so here we just mirror the arrays
        // And do the exact same thing as P1
        sumP1 += CalcNextNumber(inputLine.ToArray());
        sumP2 += CalcNextNumber(inputLine.Reverse().ToArray());
    }

    Console.WriteLine(sumP1);
    Console.WriteLine(sumP2);
}

long CalcNextNumber(long[] prevLine)
{
    // We have hit the bottom line so no more values to add
    if(prevLine.All(x => x == 0)) return 0;

    // Calculate the differences, and make an array of them
    var newLine = new long[prevLine.Length - 1];
    for (int i = 0; i < newLine.Length; i++)
    {
        newLine[i] = prevLine[i + 1] - prevLine[i];
    }

    // Calculate the differences of the next one down and recursively add it
    return prevLine[newLine.Length] + CalcNextNumber(newLine);
}

Part1And2();
