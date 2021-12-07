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

var crabs = data[0].Split(',').Select(x => long.Parse(x));
var low = crabs.Min();
var high = crabs.Max();

long GetPart1() =>
	findBestFuel((crab, current) => Math.Abs(crab - current));

long GetPart2() =>
	findBestFuel((crab, current) =>
	{
		var distance = Math.Abs(crab - current);
		return distance * (distance + 1) / 2;
	});

long findBestFuel(Func<long, long, long> fuelCost)
{
	var best = (pos: low - 1, fuel: long.MaxValue);

	for (var current = low; current <= high; ++current)
	{
		var target = 0L;

		foreach (var crab in crabs)
			target += fuelCost(crab, current);

		if (best.fuel > target)
			best = (current, target);
	}

	return best.fuel;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
