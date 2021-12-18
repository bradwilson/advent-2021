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
	var current = data[0];

	for (var idx = 1; idx < data.Length; ++idx)
		current = evaluate($"[{current},{data[idx]}]");

	return getMagnitude(current);
}

long GetPart2()
{
	var largest = 0L;

	for (var idx = 0; idx < data.Length; ++idx)
		for (var idx2 = 0; idx2 < data.Length; ++idx2)
			if (idx != idx2)
			{
				var source = $"[{data[idx]},{data[idx2]}]";
				var result = evaluate(source);
				var magnitude = getMagnitude(result);

				if (magnitude > largest)
					largest = magnitude;
			}

	return largest;
}

string evaluate(string value)
{
	var depth = 0;
	var idx = 0;
	var lastOpen = -1;
	var digitLocations = new Stack<int>();

	while (idx < value.Length)
	{
		switch (value[idx])
		{
			case '[':
				depth++;
				lastOpen = idx++;
				break;

			case ']':
				if (depth == 5)
				{
					var pieces = value[(lastOpen + 1)..idx].Split(",");
					var toExplodeLeft = int.Parse(pieces[0]);
					var toExplodeRight = int.Parse(pieces[1]);

					var prevDigitEndIdx = -1;
					while (digitLocations.Count > 0)
					{
						var digitIdx = digitLocations.Pop();
						if (digitIdx < lastOpen)
						{
							prevDigitEndIdx = digitIdx;
							break;
						}
					}

					var prevDigitStartIdx = prevDigitEndIdx;
					while (digitLocations.Count > 0)
					{
						var digitIdx = digitLocations.Pop();
						if (digitIdx == prevDigitStartIdx - 1)
							prevDigitStartIdx = digitIdx;
						else
							break;
					}

					var nextDigitStartIdx = idx;
					for (; nextDigitStartIdx < value.Length; nextDigitStartIdx++)
						if (value[nextDigitStartIdx] >= '0' && value[nextDigitStartIdx] <= '9')
							break;

					var nextDigitEndIdx = nextDigitStartIdx;
					for (; nextDigitEndIdx < value.Length - 1; nextDigitEndIdx++)
						if (value[nextDigitEndIdx + 1] < '0' || value[nextDigitEndIdx + 1] > '9')
							break;

					var prevValue = prevDigitEndIdx == -1 ? 0 : toExplodeLeft + int.Parse(value[prevDigitStartIdx..(prevDigitEndIdx + 1)]);
					var nextValue = nextDigitStartIdx == value.Length ? 0 : toExplodeRight + int.Parse(value[nextDigitStartIdx..(nextDigitEndIdx + 1)]);

					var newValue = "";

					if (prevDigitEndIdx == -1)
						newValue += value[0..lastOpen];
					else
						newValue += $"{value[0..prevDigitStartIdx]}{prevValue}{value[(prevDigitEndIdx + 1)..lastOpen]}";

					newValue += "0";

					if (nextDigitStartIdx == value.Length)
						newValue += value[(idx + 1)..];
					else
						newValue += $"{value[(idx + 1)..nextDigitStartIdx]}{nextValue}{value[(nextDigitEndIdx + 1)..]}";

					value = newValue;
					idx = 0;
					depth = 0;
					digitLocations.Clear();
				}
				else
				{
					idx++;
					depth--;
				}
				break;

			case ',':
				idx++;
				break;

			default:
				digitLocations.Push(idx++);
				break;
		}
	}

	// Now we look for splits by checking for paired up digits
	var firstDigitIdx = int.MaxValue;

	foreach (var digitIdx in digitLocations.Reverse())
	{
		if (digitIdx - 1 == firstDigitIdx)
		{
			var overSizeValue = int.Parse(value[firstDigitIdx..(digitIdx + 1)]);
			var newValue = $"{value[0..firstDigitIdx]}{split(overSizeValue)}{value[(digitIdx + 1)..]}";
			return evaluate(newValue);
		}
		firstDigitIdx = digitIdx;
	}

	return value;
}

string split(int value) => $"[{value / 2},{value / 2 + value % 2}]";

long getMagnitude(string value)
{
	var stack = new Stack<long>();

	foreach (var c in value)
		switch (c)
		{
			case '[':
			case ',':
				break;

			case ']':
				var right = stack.Pop();
				var left = stack.Pop();
				var result = (left * 3) + (right * 2);
				stack.Push(result);
				break;

			default:
				stack.Push(c - '0');
				break;
		}

	if (stack.Count != 1)
		throw new InvalidOperationException($"Stack count was {stack.Count}");

	return stack.Pop();
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
