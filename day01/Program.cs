using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select int.Parse(line)).ToArray();

long GetPart1()
{
	var lastValue = data[0];
	var increments = 0;

	for (var idx = 1; idx < data.Length; ++idx)
	{
		var value = data[idx];
		if (value > lastValue)
			++increments;
		lastValue = value;
	}

	return increments;
}

long computeWindow(int startIndex)
{
	return data[startIndex] + data[startIndex + 1] + data[startIndex + 2];
}

long GetPart2()
{
	var lastValue = computeWindow(0);
	var increments = 0;

	for (var idx = 3; idx < data.Length; ++idx)
	{
		var value = computeWindow(idx - 2);
		if (value > lastValue)
			++increments;
		lastValue = value;
	}

	return increments;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
