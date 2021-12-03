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

long GetPart1()
{
	var gamma = 0L;
	var epsilon = 0L;

	for (var idx = 0; idx < data[0].Length; ++idx)
	{
		var zeros = 0L;
		var ones = 0L;

		foreach (var line in data)
			if (line[idx] == '0')
				++zeros;
			else
				++ones;

		gamma *= 2;
		epsilon *= 2;

		if (zeros > ones)
			++epsilon;
		else
			++gamma;
	}

	return gamma * epsilon;
}

long GetPart2()
{
	var oxygen = getValue(leastCommon: false);
	var co2 = getValue(leastCommon: true);

	return oxygen * co2;
}

long getValue(bool leastCommon)
{
	var values = data.ToList();

	for (var idx = 0; idx < data[0].Length; ++idx)
	{
		var zeros = 0L;
		var ones = 0L;

		foreach (var line in values)
			if (line[idx] == '0')
				++zeros;
			else
				++ones;

		if ((leastCommon && ones >= zeros) || (!leastCommon && zeros > ones))
		{
			for (var idx2 = values.Count - 1; idx2 >= 0; --idx2)
				if (values[idx2][idx] == '1')
					values.RemoveAt(idx2);
		}
		else
		{
			for (var idx2 = values.Count - 1; idx2 >= 0; --idx2)
				if (values[idx2][idx] == '0')
					values.RemoveAt(idx2);
		}

		if (values.Count == 1)
			break;
	}

	var result = 0L;
	foreach (var ch in values[0])
	{
		result *= 2;
		if (ch == '1')
			result++;
	}

	return result;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
