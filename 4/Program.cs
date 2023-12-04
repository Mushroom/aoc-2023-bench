var inputFromFile = File.ReadAllText("input.txt");
var input = inputFromFile.TrimEnd('\n');
var inputSplit = input.Split('\n');

void Part1And2() {
  int part1Sum = 0;
  int[] cardCounts = new int[inputSplit.Length];
  Array.Fill(cardCounts, 1); // We start with 1 of each card

  for (int i = 0; i < inputSplit.Length; i++) {
    string cardLine = inputSplit[i];

    // Parse out the "cards" to two int arrays
    var splitCard =
        cardLine.Split(": ") [1]
            .Split(" | ")
            .ToArray()
            .Select(x => Array.ConvertAll(
                        x.Split(' ', StringSplitOptions.RemoveEmptyEntries),
                        int.Parse))
            .ToArray();

    // Part 1
    var matchingNumbers = splitCard[0].Intersect(splitCard[1]).ToArray();

    // Part 2
    // For each card that we have a copy of, add another match
    for (int j = 0; j < cardCounts[i]; j++) {
      // Add it to subsequent consecutive numbers
      for (int k = 0; k < matchingNumbers.Length; k++) {
        cardCounts[i + 1 + k]++;
      }
    }

    // Increase the sum by 2^(x-1) where x is the number of matches
    part1Sum += matchingNumbers.Length > 0
                    ? (int)Math.Pow(2, matchingNumbers.Length - 1)
                    : 0;
  }

  Console.WriteLine(part1Sum);
  Console.WriteLine(cardCounts.Sum());
}

Part1And2();