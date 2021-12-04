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

var calledNumbers = data[0].Split(',').Select(x => int.Parse(x)).ToArray();

long GetPart1() => FindWinner(winnerIsFirst: true);

long GetPart2() => FindWinner(winnerIsFirst: false);

long FindWinner(bool winnerIsFirst)
{
	var boards = new List<Board>();
	var currentIdx = 1;

	while (currentIdx < data.Length)
	{
		boards.Add(new Board(data, currentIdx));
		currentIdx += 5;
	}

	foreach (var calledNumber in calledNumbers)
		for (var idx = boards.Count - 1; idx >= 0; --idx)
		{
			boards[idx].Mark(calledNumber);
			if (boards[idx].IsWinner())
			{
				if (winnerIsFirst || boards.Count == 1)
					return boards[idx].SumUnmarked() * calledNumber;

				boards.RemoveAt(idx);
			}
		}

	throw new InvalidOperationException();
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");

class Board
{
	readonly int[,] numbers = new int[5, 5];

	public Board(string[] data, int firstLineIdx)
	{
		for (var idx = 0; idx < 5; ++idx)
		{
			var lineNumbers = data[firstLineIdx + idx].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
			for (var idx2 = 0; idx2 < 5; ++idx2)
				numbers[idx, idx2] = int.Parse(lineNumbers[idx2]);
		}
	}

	public void Mark(int number)
	{
		for (var idx = 0; idx < 5; ++idx)
			for (var idx2 = 0; idx2 < 5; ++idx2)
				if (numbers[idx, idx2] == number)
					numbers[idx, idx2] = -1;
	}

	public bool IsWinner()
	{
		for (var idx = 0; idx < 5; idx++)
		{
			if (numbers[idx, 0] + numbers[idx, 1] + numbers[idx, 2] + numbers[idx, 3] + numbers[idx, 4] == -5)
				return true;
			if (numbers[0, idx] + numbers[1, idx] + numbers[2, idx] + numbers[3, idx] + numbers[4, idx] == -5)
				return true;
		}

		return false;
	}

	public long SumUnmarked()
	{
		var result = 0L;

		for (var idx = 0; idx < 5; ++idx)
			for (var idx2 = 0; idx2 < 5; ++idx2)
				if (numbers[idx, idx2] != -1)
					result += numbers[idx, idx2];

		return result;
	}
}
