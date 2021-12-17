using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select parseLine(line)).First();

var hitCount = 0L;
var highestY = int.MinValue;

for (var startVelocityX = 0; startVelocityX <= data.maxX; ++startVelocityX)
	for (var startVelocityY = data.minY; startVelocityY <= -data.minY; ++startVelocityY)
	{
		var curX = 0;
		var curY = 0;
		var veloX = startVelocityX;
		var veloY = startVelocityY;
		var maxY = int.MinValue;

		while (curX < data.maxX && curY > data.minY)
		{
			if (curY > maxY)
				maxY = curY;

			curX += veloX;
			curY += veloY;

			if (curX >= data.minX && curX <= data.maxX && curY >= data.minY && curY <= data.maxY)
			{
				++hitCount;

				if (maxY > highestY)
					highestY = maxY;

				break;
			}

			if (veloX > 0)
				veloX--;
			else if (veloX < 0)
				veloX++;

			veloY--;
		}
	}

long GetPart1() => highestY;

long GetPart2() => hitCount;

Range parseLine(string line)
{
	var pieces = line[13..].Split(", ");
	var xRange = pieces[0].Split("=")[1].Split("..").Select(x => int.Parse(x)).OrderBy(x => x).ToList();
	var yRange = pieces[1].Split("=")[1].Split("..").Select(x => int.Parse(x)).OrderBy(x => x).ToList();

	return new Range(xRange[0], xRange[1], yRange[0], yRange[1]);
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");

record Range(int minX, int maxX, int minY, int maxY);
