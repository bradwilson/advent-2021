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

var incompleteLines = new List<Stack<char>>();

long GetPart1()
{
	var scores = new Dictionary<char, (char, long)> { { ')', ('(', 3) }, { ']', ('[', 57) }, { '}', ('{', 1197) }, { '>', ('<', 25137) } };
	var score = 0L;

	foreach (var line in data)
	{
		var stack = new Stack<char>();
		var incomplete = true;

		foreach (var c in line)
		{
			scores.TryGetValue(c, out var charScore);

			if (charScore.Item2 == 0)
				stack.Push(c);
			else
			{
				if (stack.TryPop(out var matching) && matching == charScore.Item1)
					continue;

				score += charScore.Item2;
				incomplete = false;
				break;
			}
		}

		if (incomplete)
			incompleteLines.Add(stack);
	}

	return score;
}

long GetPart2()
{
	var scores = new Dictionary<char, long> { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };
	var lineScores = new List<long>();

	foreach (var line in incompleteLines)
	{
		var lineScore = 0L;

		while (line.TryPop(out var c))
			lineScore = lineScore * 5 + scores[c];

		lineScores.Add(lineScore);
	}

	return lineScores.OrderBy(x => x).Skip(lineScores.Count / 2).First();
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
