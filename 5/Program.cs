using System.Runtime.CompilerServices;

var inputFromFile = File.ReadAllLines("input.txt");

var seeds = Array.ConvertAll(inputFromFile.First().Split(": ")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse);

// Parse out the long arrays
var seedToSoilMapRangesTuple = inputFromFile.Skip(3).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(y => Array.ConvertAll(y.Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse)).Select(x => (start: x[1], end: x[1] + x[2] - 1, offset: x[0] - x[1])).ToArray();
var soilToFertilizerMapRangesTuple = inputFromFile.Skip(3 + seedToSoilMapRangesTuple.Length + 2).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(y => Array.ConvertAll(y.Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse)).Select(x => (start: x[1], end: x[1] + x[2] - 1, offset: x[0] - x[1])).ToArray();
var fertilizerToWaterMapRangesTuple = inputFromFile.Skip(3 + seedToSoilMapRangesTuple.Length + soilToFertilizerMapRangesTuple.Length + 4).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(y => Array.ConvertAll(y.Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse)).Select(x => (start: x[1], end: x[1] + x[2] - 1, offset: x[0] - x[1])).ToArray();
var watertoLightMapRangesTuple = inputFromFile.Skip(3 + seedToSoilMapRangesTuple.Length + soilToFertilizerMapRangesTuple.Length + fertilizerToWaterMapRangesTuple.Length + 6).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(y => Array.ConvertAll(y.Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse)).Select(x => (start: x[1], end: x[1] + x[2] - 1, offset: x[0] - x[1])).ToArray();
var lightToTemperatureMapRangesTuple = inputFromFile.Skip(3 + seedToSoilMapRangesTuple.Length + soilToFertilizerMapRangesTuple.Length + fertilizerToWaterMapRangesTuple.Length + watertoLightMapRangesTuple.Length + 8).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(y => Array.ConvertAll(y.Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse)).Select(x => (start: x[1], end: x[1] + x[2] - 1, offset: x[0] - x[1])).ToArray();
var temperatureToHumidityMapRangesTuple = inputFromFile.Skip(3 + seedToSoilMapRangesTuple.Length + soilToFertilizerMapRangesTuple.Length + fertilizerToWaterMapRangesTuple.Length + watertoLightMapRangesTuple.Length + lightToTemperatureMapRangesTuple.Length + 10).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(y => Array.ConvertAll(y.Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse)).Select(x => (start: x[1], end: x[1] + x[2] - 1, offset: x[0] - x[1])).ToArray();
var humidityToLocationMapRangesTuple = inputFromFile.Skip(3 + seedToSoilMapRangesTuple.Length + soilToFertilizerMapRangesTuple.Length + fertilizerToWaterMapRangesTuple.Length + watertoLightMapRangesTuple.Length + lightToTemperatureMapRangesTuple.Length + temperatureToHumidityMapRangesTuple.Length + 12).TakeWhile(x => !string.IsNullOrEmpty(x)).Select(y => Array.ConvertAll(y.Split(' ', StringSplitOptions.RemoveEmptyEntries), long.Parse)).Select(x => (start: x[1], end: x[1] + x[2] - 1, offset: x[0] - x[1])).ToArray();

void Part1()
{
    long minValue = long.MaxValue;

    foreach(long seed in seeds)
    {
        long soilMapAdjustedValue = MapAdjustedValue(seed, seedToSoilMapRangesTuple);
        long fertilizerMapAdjustedValue = MapAdjustedValue(soilMapAdjustedValue, soilToFertilizerMapRangesTuple);
        long waterMapAdjustedValue = MapAdjustedValue(fertilizerMapAdjustedValue, fertilizerToWaterMapRangesTuple);
        long lightMapAdjustedValue = MapAdjustedValue(waterMapAdjustedValue, watertoLightMapRangesTuple);
        long temperatureMapAdjustedValue = MapAdjustedValue(lightMapAdjustedValue, lightToTemperatureMapRangesTuple);
        long humidityMapAdjustedValue = MapAdjustedValue(temperatureMapAdjustedValue, temperatureToHumidityMapRangesTuple);
        long locationMapAdjustedValue = MapAdjustedValue(humidityMapAdjustedValue, humidityToLocationMapRangesTuple);

        minValue = Math.Min(minValue, locationMapAdjustedValue);
    }

    Console.WriteLine(minValue);
}

[MethodImpl(MethodImplOptions.AggressiveInlining)]
long MapAdjustedValue(long inputValue, (long start, long end, long offset)[] mapRanges)
{
    foreach(var mapRange in mapRanges)
    {
        if(mapRange.start <= inputValue && inputValue <= mapRange.end)
        {
            return inputValue + mapRange.offset;
        }
    }

    return inputValue;
}

void Part2()
{
    var ranges = new List<(long start, long end)>();

    // Parse out the ranges that we would like in chunks of 2
    // These are our initial maps
    for(int i = 0; i < seeds.Length; i+=2)
    {
        ranges.Add(new (seeds[i], seeds[i] + seeds[i+1] - 1));
    }

    // Loop through all of the range maps we have
    foreach(var rangeMap in new []{ seedToSoilMapRangesTuple, soilToFertilizerMapRangesTuple, fertilizerToWaterMapRangesTuple, watertoLightMapRangesTuple, lightToTemperatureMapRangesTuple, temperatureToHumidityMapRangesTuple, humidityToLocationMapRangesTuple })
    {
        var newRanges = new List<(long start, long end)>();

        // Loop through the current mapped ranges
        foreach(var range in ranges)
        {
            // Assign to var in order to avoid CS1654
            var rangeVar = range;

            // Go through the mappings in the current rangemap in ascending order
            foreach(var mapping in rangeMap.OrderBy(x => x.start))
            {
                // If the current range var is from a lesser value than the current mapping
                // Then narrow the mapping space
                if(rangeVar.start < mapping.start)
                {
                    newRanges.Add((rangeVar.start, Math.Min(rangeVar.end, mapping.start - 1)));
                    rangeVar.start = mapping.start;
                    if(rangeVar.start > rangeVar.end) break;
                }

                // If the current range var overlaps the mapping,
                // Then create a sub-range based on the offset and the smallest value
                if(rangeVar.start <= mapping.end)
                {
                    newRanges.Add((rangeVar.start + mapping.offset, Math.Min(rangeVar.end, mapping.end) + mapping.offset));
                    rangeVar.start = mapping.end + 1;
                    if(rangeVar.start > rangeVar.end) break;
                }
            }

            // If the range has any mapping space at all (ie, not discarded)
            if (rangeVar.start <= rangeVar.end)
            {
                newRanges.Add(rangeVar);
            }
        }

        // Update our mapping ranges
        ranges = newRanges;
    }

    Console.WriteLine(ranges.OrderBy(x => x.start).First().start);
}

Part1();
Part2();
