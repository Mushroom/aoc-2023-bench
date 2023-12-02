using System.Text.RegularExpressions;

var inputFromFile = File.ReadAllText("input.txt");
var input = inputFromFile.TrimEnd('\n');

var inputSplit = input.Split('\n').Select(x => x.Split(':')[1].Trim()).ToArray(); // Get each line without "Game X: "

void Part1and2()
{
    int totalPart1Sum = 0;
    int totalPart2Sum = 0;

    // Loop through each set of game results
    for(int i = 1; i <= inputSplit.Length; i++)
    {
        bool redFail = false;
        bool greenFail = false;
        bool blueFail = false;

        int maxRedValue = 0;
        int maxGreenValue = 0;
        int maxBlueValue = 0;

        string gameString = inputSplit[i-1];

        // Extract the values via regex capture groups
        var matches = Regex.Matches(gameString, "([0-9]+) (\\w+)");
        foreach(Match match in matches)
        {
            // We take the max possible value of each colour for P2
            // For P1 if it ever exceeds the threshold we set a bool
            switch(match.Groups[2].Value)
            {
                case "red":
                    // Part 2
                    int redValue = int.Parse(match.Groups[1].Value);
                    maxRedValue = Math.Max(maxRedValue, redValue);

                    // Part 1
                    if(redFail) break;
                    redFail = redValue > 12;
                    break;
                case "green":
                    // Part 2
                    int greenValue = int.Parse(match.Groups[1].Value);
                    maxGreenValue = Math.Max(maxGreenValue, greenValue);
                    
                    // Part 1
                    if(greenFail) break;
                    greenFail = int.Parse(match.Groups[1].Value) > 13;
                    break;
                case "blue":
                    // Part 2
                    int blueValue = int.Parse(match.Groups[1].Value);
                    maxBlueValue = Math.Max(maxBlueValue, blueValue);

                    // Part 1
                    if(blueFail) break;
                    blueFail = int.Parse(match.Groups[1].Value) > 14;
                    break;
            }
        }

        // If no thresholds were exceeded for P1, add to the sum
        if(!redFail && !greenFail && !blueFail)
        {
            totalPart1Sum += i;
        }

        // Add the product of the max values for P2
        totalPart2Sum += maxRedValue * maxGreenValue * maxBlueValue;
    }

    Console.WriteLine(totalPart1Sum);
    Console.WriteLine(totalPart2Sum);
}

Part1and2();
