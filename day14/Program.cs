using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select line).ToArray();

var polymer = data[0];
var instructions = data.Skip(1).ToDictionary(x => x[..2], x => x[6]);

long GetPart1()
{
	var result = countPolymer(polymer, 10);

	var max = result.Max(kvp => kvp.Value);
	var min = result.Min(kvp => kvp.Value);

	return max - min;
}

long GetPart2()
{
	var result = countPolymer(polymer, 40);

	var max = result.Max(kvp => kvp.Value);
	var min = result.Min(kvp => kvp.Value);

	return max - min;
}

Dictionary<char, long> countPolymer(string polymer, int iterations)
{
	var pairCounts = new Dictionary<string, long>();

	for (var idx = 0; idx < polymer.Length - 1; idx++)
	{
		var pair = polymer.Substring(idx, 2);
		pairCounts.TryGetValue(pair, out var count1);
		pairCounts[pair] = count1 + 1;
	}

	for (var idx = 0; idx < iterations; ++idx)
	{
		var newCounts = new Dictionary<string, long>();

		foreach (var kvp in pairCounts)
		{
			var instruction = instructions[kvp.Key];

			var pair1 = new string(new[] { kvp.Key[0], instruction });
			newCounts.TryGetValue(pair1, out var count1);
			newCounts[pair1] = count1 + kvp.Value;

			var pair2 = new string(new[] { instruction, kvp.Key[1] });
			newCounts.TryGetValue(pair2, out var count2);
			newCounts[pair2] = count2 + kvp.Value;
		}

		pairCounts = newCounts;
	}

	var result = new Dictionary<char, long>();

	foreach (var kvp in pairCounts)
	{
		result.TryGetValue(kvp.Key[1], out var count2);
		result[kvp.Key[1]] = count2 + kvp.Value;
	}

	result.TryGetValue(polymer[0], out var count);
	result[polymer[0]] = count + 1;

	return result;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
