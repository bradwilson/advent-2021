using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select parseInstruction(line)).ToArray();

long GetPart1()
{
	var horizontal = 0L;
	var depth = 0L;

	foreach (var instruction in data)
	{
		if (instruction.Direction == Direction.down)
			depth += instruction.Distance;
		else if (instruction.Direction == Direction.up)
			depth -= instruction.Distance;
		else
			horizontal += instruction.Distance;
	}

	return horizontal * depth;
}

long GetPart2()
{
	var horizontal = 0L;
	var depth = 0L;
	var aim = 0L;

	foreach (var instruction in data)
	{
		if (instruction.Direction == Direction.down)
			aim += instruction.Distance;
		else if (instruction.Direction == Direction.up)
			aim -= instruction.Distance;
		else
		{
			horizontal += instruction.Distance;
			depth += instruction.Distance * aim;
		}
	}

	return horizontal * depth;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");

Instruction parseInstruction(string input)
{
	var pieces = input.Split(" ");
	return new(Enum.Parse<Direction>(pieces[0]), int.Parse(pieces[1]));
}

record Instruction(Direction Direction, int Distance);

enum Direction { forward, down, up };
