using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 select line.Trim()).ToList();

var blank = data.IndexOf("");

var points = new List<(int x, int y)>();
for (var idx = 0; idx < blank; ++idx)
{
	var pieces = data[idx].Split(',');
	points.Add((int.Parse(pieces[0]), int.Parse(pieces[1])));
}

var instructions = new List<(char direction, int line)>();
for (var idx = blank + 1; idx < data.Count; ++idx)
{
	var pieces = data[idx].Split(' ');
	var subPieces = pieces[2].Split('=');
	instructions.Add((subPieces[0][0], int.Parse(subPieces[1])));
}

var maxX = points.Max(d => d.x);
var maxY = points.Max(d => d.y);
var grid = new bool[maxX + 1, maxY + 1];

foreach (var (x, y) in points)
	grid[x, y] = true;

long GetPart1()
{
	fold(instructions[0]);

	var result = 0L;

	for (var y = 0; y <= maxY; ++y)
		for (var x = 0; x <= maxX; ++x)
			if (grid[x, y])
				++result;

	return result;
}

void GetPart2()
{
	for (var idx = 1; idx < instructions.Count; ++idx)
		fold(instructions[idx]);

	Console.WriteLine();

	for (var y = 0; y <= maxY; ++y)
	{
		for (var x = 0; x <= maxX; ++x)
			Console.Write(grid[x, y] ? '#' : ' ');

		Console.WriteLine();
	}

	Console.WriteLine();
}

void fold((char direction, int line) instruction)
{
	if (instruction.direction == 'x')
		foldAlongX(instruction.line);
	else
		foldAlongY(instruction.line);
}

void foldAlongX(int point)
{
	var offset = (point * 2) - maxX;

	for (var x = point + 1; x <= maxX; ++x)
		for (var y = 0; y <= maxY; ++y)
			if (grid[x, y])
				grid[maxX - x + offset, y] = true;

	maxX = point - 1;
}

void foldAlongY(int point)
{
	var offset = (point * 2) - maxY;

	for (var y = point + 1; y <= maxY; ++y)
		for (var x = 0; x <= maxX; ++x)
			if (grid[x, y])
				grid[x, maxY - y + offset] = true;

	maxY = point - 1;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2");
