using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select parseLine(line)).ToArray();

var maxX = data.Length;
var maxY = data[0].Length;

long? firstFullFlash = null;

long GetPart1()
{
	var result = 0L;

	for (var idx = 1; idx <= 100; ++idx)
		result += step(idx);

	return result;
}

long GetPart2()
{
	var idx = 101;

	while (firstFullFlash == null)
		step(idx++);

	return firstFullFlash.Value;
}

long step(int step)
{
	for (var x = 0; x < maxX; ++x)
		for (var y = 0; y < maxY; ++y)
			processOctopus(x, y);

	var result = 0L;

	for (var x = 0; x < maxX; ++x)
		for (var y = 0; y < maxY; ++y)
			if (data[x][y] == 10)
			{
				result++;
				data[x][y] = 0;
			}

	if (firstFullFlash == null && result == maxX * maxY)
		firstFullFlash = step;

	return result;
}

void processOctopus(int x, int y)
{
	if (x < 0 || x >= maxX || y < 0 || y >= maxY)
		return;

	if (data[x][y] < 9)
		data[x][y]++;
	else if (data[x][y] == 9)
	{
		data[x][y] = 10;
		processOctopus(x - 1, y - 1);
		processOctopus(x - 1, y);
		processOctopus(x - 1, y + 1);
		processOctopus(x, y - 1);
		processOctopus(x, y + 1);
		processOctopus(x + 1, y - 1);
		processOctopus(x + 1, y);
		processOctopus(x + 1, y + 1);
	}
}

int[] parseLine(string line)
{
	var result = new int[line.Length];
	var idx = 0;

	foreach (var c in line)
		result[idx++] = int.Parse(new[] { c });

	return result;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
