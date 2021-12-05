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

long GetPart1() => CalculateBoard(diagonal: false);

long GetPart2() => CalculateBoard(diagonal: true);

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");

long CalculateBoard(bool diagonal)
{
	var board = new Dictionary<(int, int), int>();

	foreach (var row in data)
	{
		var pieces = row.Split(' ');
		var left = pieces[0].Split(',').Select(x => int.Parse(x)).ToArray();
		var right = pieces[2].Split(',').Select(x => int.Parse(x)).ToArray();

		var xInc = left[0] > right[0] ? -1 : (left[0] < right[0] ? 1 : 0);
		var yInc = left[1] > right[1] ? -1 : (left[1] < right[1] ? 1 : 0);

		if ((xInc != 0 && yInc != 0) && !diagonal)
			continue;

		var x = left[0];
		var y = left[1];

		var limit = Math.Max(Math.Abs(left[0] - right[0]), Math.Abs(left[1] - right[1]));

		for (var idx = 0; idx <= limit; ++idx)
		{
			var coord = (x, y);

			board.TryGetValue(coord, out var count);
			board[coord] = count + 1;

			x += xInc;
			y += yInc;
		}
	}

	return board.Values.Where(x => x > 1).Count();
}
