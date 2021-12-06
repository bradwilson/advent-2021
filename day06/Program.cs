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

var fish = new long[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

foreach (var age in data[0].Split(',').Select(x => int.Parse(x)))
	fish[age]++;

long GetPart1()
{
	for (var idx = 0; idx < 80; ++idx)
		AgeFish();

	return fish.Sum();
}

long GetPart2()
{
	for (var idx = 80; idx < 256; ++idx)
		AgeFish();

	return fish.Sum();
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");

void AgeFish()
{
	var cycle = fish[0];

	for (var age = 1; age < 9; ++age)
		fish[age - 1] = fish[age];

	fish[8] = cycle;
	fish[6] += cycle;
}
