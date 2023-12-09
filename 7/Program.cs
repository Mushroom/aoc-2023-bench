using System.Diagnostics;
using System.Runtime.CompilerServices;

Dictionary<char, CardTypeStrength> letterMappings = new Dictionary<char, CardTypeStrength>()
{
    {'0', CardTypeStrength.Joker},
    {'1', CardTypeStrength.One},
    {'2', CardTypeStrength.Two},
    {'3', CardTypeStrength.Three},
    {'4', CardTypeStrength.Four},
    {'5', CardTypeStrength.Five},
    {'6', CardTypeStrength.Six},
    {'7', CardTypeStrength.Seven},
    {'8', CardTypeStrength.Eight},
    {'9', CardTypeStrength.Nine},
    {'T', CardTypeStrength.Ten},
    {'J', CardTypeStrength.Jack},
    {'Q', CardTypeStrength.Queen},
    {'K', CardTypeStrength.King},
    {'A', CardTypeStrength.Ace}
};

var inputFromFile = File.ReadAllLines("input.txt");

void Part1And2()
{
    List<(string hand, HandType handType, int bid, byte[] strength)> handsP1 = new List<(string hand, HandType handType, int bid, byte[] strength)>(inputFromFile.Length);
    List<(string hand, HandType handType, int bid, byte[] strength)> handsP2 = new List<(string hand, HandType handType, int bid, byte[] strength)>(inputFromFile.Length);

    foreach(var line in inputFromFile)
    {
        var splitLine = line.Split(' ');
        var hand = splitLine[0];
        var bid = int.Parse(splitLine[1]);

        // Group by matching card type and count how much of each group
        var groupsP1 = hand.GroupBy(x => x).Select(y => y.Count()).OrderByDescending(z => z).ToArray();
        var groupsP2 = hand.Replace('J', '0').GroupBy(x => x).Select(y => y.First() == '0' ? 0 : y.Count()).OrderByDescending(z => z).ToArray();
        groupsP2[0] += hand.Count(x => x == 'J');
        
        // Determine the hand type by counts of each group
        HandType handTypeP1 = GroupsToHandType(groupsP1);
        HandType handTypeP2 = GroupsToHandType(groupsP2);

        // Map the hand strength using the predefined dict
        var handStrengthP1 = hand.Select(x => (byte)letterMappings[x]).ToArray();
        var handStrengthP2 = hand.Replace('J', '0').Select(x => (byte)letterMappings[x]).ToArray();

        handsP1.Add((hand, handTypeP1, bid, handStrengthP1));
        handsP2.Add((hand, handTypeP2, bid, handStrengthP2));
    }

    // Order the hands first by type, then strength
    var orderedHandsP1 = handsP1.OrderBy(x => x.handType).ThenBy(x => x.strength, new ByteArrayComparer()).Select(x => x.bid).ToArray();
    var orderedHandsP2 = handsP2.OrderBy(x => x.handType).ThenBy(x => x.strength, new ByteArrayComparer()).Select(x => x.bid).ToArray();

    // Calculate the winnings
    var totalWinningsP1 = 0;
    for(int i = 0; i < orderedHandsP1.Length; i++)
    {
        totalWinningsP1 += ((i + 1) * orderedHandsP1[i]);
    }

    var totalWinningsP2 = 0;
    for(int i = 0; i < orderedHandsP2.Length; i++)
    {
        totalWinningsP2 += ((i + 1) * orderedHandsP2[i]);
    }

    Console.WriteLine(totalWinningsP1);
    Console.WriteLine(totalWinningsP2);
}

Part1And2();

[MethodImpl(MethodImplOptions.AggressiveInlining)]
HandType GroupsToHandType(int[] groups)
{
    switch(groups[0])
    {
        case 5:
            return HandType.FiveOfAKind;
        case 4:
            return HandType.FourOfAKind;
        case 3:
            return groups[1] == 2 ? HandType.FullHouse : HandType.ThreeOfAKind;
        case 2:
            return groups[1] == 2 ? HandType.TwoPair : HandType.OnePair;
        case 1:
            return HandType.HighCard;
        default:
            Debugger.Break();
            throw new UnreachableException("Group has more than 5 occurences");
    }
}

class ByteArrayComparer : IComparer<byte[]>
{
    public int Compare(byte[]? x, byte[]? y)
    {
        for(int i = 0; i < x!.Length; i++)
        {
            if(x![i] > y![i])
            {
                return 1;
            }
            else if (y![i] > x![i])
            {
                return -1;
            }
        }

        return 0;
    }
}

enum HandType : byte
{
    HighCard = 1,
    OnePair = 2,
    TwoPair = 3,
    ThreeOfAKind = 4,
    FullHouse = 5,
    FourOfAKind = 6,
    FiveOfAKind = 7
}

enum CardTypeStrength : byte
{
    Joker = 0,
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13,
    Ace = 14
}