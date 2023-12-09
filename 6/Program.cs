using System.Runtime.CompilerServices;

var inputFromFile = File.ReadAllLines("input.txt");

var times = Array.ConvertAll(inputFromFile[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
var distances = Array.ConvertAll(inputFromFile[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);

void Part1()
{
    long result = 1;
    for(int i = 0; i < times.Length; i++)
    {
        result *= WaysToWin(times[i], distances[i]);
    }

    Console.WriteLine(result);
}

void Part2()
{
    Console.WriteLine(WaysToWin(long.Parse(string.Join("", times)), long.Parse(string.Join("", distances))));
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
long WaysToWin(long time, long distance)
{
    // Quadratic formula
    var sqDiscrim = Math.Sqrt((time * time) - (4 * distance));
    var low = (time - sqDiscrim) / 2;
    var high = (time + sqDiscrim) / 2;
    return (long)(Math.Ceiling(high) - Math.Floor(low) - 1);
}

Part1();
Part2();
