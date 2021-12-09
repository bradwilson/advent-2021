using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select line.ToCharArray().Select(x => int.Parse(new[] { x })).ToArray()).ToArray();

var maxX = data.Length;
var maxY = data[0].Length;

long GetPart1()
{
	var total = 0L;

	for (var x = 0; x < maxX; ++x)
		for (var y = 0; y < maxY; ++y)
		{
			var cur = data[x][y];
			var left = x > 0 ? data[x - 1][y] : 10;
			var right = x < maxX - 1 ? data[x + 1][y] : 10;
			var top = y > 0 ? data[x][y - 1] : 10;
			var bottom = y < maxY - 1 ? data[x][y + 1] : 10;

			if (cur < left && cur < right && cur < top && cur < bottom)
				total += cur + 1;
		}

	return total;
}

long GetPart2()
{
	var basinSizes = new List<long>();

	for (var x = 0; x < maxX; ++x)
		for (var y = 0; y < maxY; ++y)
		{
			var basinSize = calculateBasin(x, y);
			if (basinSize != 0)
				basinSizes.Add(basinSize);
		}

	return basinSizes.OrderByDescending(x => x).Take(3).Aggregate(1L, (x, y) => x * y);
}

long calculateBasin(int x, int y)
{
	if (x < 0 || x >= maxX || y < 0 || y >= maxY || data[x][y] > 8)
		return 0;

	data[x][y] = 10;
	return 1 + calculateBasin(x - 1, y) + calculateBasin(x + 1, y) + calculateBasin(x, y - 1) + calculateBasin(x, y + 1);
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
