using System.Text.RegularExpressions;

var inputFromFile = File.ReadAllText("input.txt");
var input = inputFromFile.TrimEnd('\n');

var inputSplit = input.Split('\n');

string[] numbers = {
    "one",
    "two",
    "three",
    "four",
    "five",
    "six",
    "seven",
    "eight",
    "nine"
};

void Part1()
{
    // Select numerical characters, then combine first and last via strings
    var firstAndLastSum = inputSplit
                            .Select(x => x.Where(y => char.IsNumber(y)))
                            .Select(z => int.Parse($"{z.First()}{z.Last()}"))
                            .Sum();
    Console.WriteLine(firstAndLastSum);
}

void Part2()
{
    int total = 0;

    foreach(var input in inputSplit)
    {
        // SortedDictionary allows us to just take first and last at the end
        // Even when we have sparse indexes, which is a nice shortcut
        SortedDictionary<int, int> numberAtIndex = [];

        // Loop through the "numbers" array and check for occurences of each in turn
        for (int i = 1; i <= numbers.Length; i++)
        {
            string number = numbers[i-1];

            // Find the indexes of occurences of the string-type numbers,
            // then put the values in the Dictionary at that index
            MatchCollection matches = Regex.Matches(input, number);
            foreach(Match match in matches)
            {
                numberAtIndex[match.Index] = i;
            }

            // Now do the same, but the the integer format numbers
            matches = Regex.Matches(input, $"{i}");
            foreach(Match match in matches)
            {
                numberAtIndex[match.Index] = i;
            }
        }

        // Parse out the first and last by concatting them in a string
        total += int.Parse($"{numberAtIndex.First().Value}{numberAtIndex.Last().Value}");
    }

    Console.WriteLine(total);
}

Part1();
Part2();