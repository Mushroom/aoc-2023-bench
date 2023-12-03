using System.Text.RegularExpressions;

var inputFromFile = File.ReadAllText("input.txt");
var input = inputFromFile.TrimEnd('\n');

void Part1and2()
{
    var rowLength = input.IndexOf('\n') + 1;
    var symbolPositions = new List<int>();
    var starPositions = new Dictionary<int, List<int>>();
    int partNumberSum = 0;

    // Get the positions of all symbols (anything not a digit or a .)
    var matches = Regex.Matches(input, "[^\\d.\n]");
    foreach(Match match in matches)
    {
        // Part 1: We just care about where the positions are
        symbolPositions.Add(match.Index);

        // Part 2: Create a list associated with each star, which we will populate later
        if(match.Value == "*") starPositions.Add(match.Index, new List<int>());
    }

    // Now scan out the numbers
    matches = Regex.Matches(input, "\\d+");
    foreach(Match match in matches)
    {
        // Search for symbol adjacency
        int[] indexesToAdd = Enumerable.Range(0, match.Length + 2).ToArray();
        // Generate a grid of every index around the number
        int[] indexesToCheck = Enumerable.Range(match.Index - rowLength - 1, match.Length + 2) // Row above
                                .Concat(new []{ match.Index - 1, match.Index + match.Length }) // Either side
                                .Concat(Enumerable.Range(match.Index + rowLength - 1, match.Length + 2)) // Row below
                                .ToArray();
        
        // Part 1
        // Add the number to the list if it has an adjacent symbol
        if(indexesToCheck.Any(symbolPositions.Contains))
        {
            partNumberSum += int.Parse(match.Value);
        }

        // Part 2
        // Get all the star positions within range of this number and add it to their internal list
        foreach(var starPosition in starPositions.Where(x => indexesToCheck.Contains(x.Key)))
        {
            starPosition.Value.Add(int.Parse(match.Value));
        }
        
    }

    // For the stars that have 2 gears, sum their gear ratios
    var gearRatioSum = starPositions
                        .Where(x => x.Value.Count == 2)
                        .Sum(x => x.Value.Aggregate((y, z) => y * z));

    Console.WriteLine(partNumberSum);
    Console.WriteLine(gearRatioSum);
}

Part1and2();