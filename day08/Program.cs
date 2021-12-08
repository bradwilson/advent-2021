using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select parseLine(line)).ToArray();

long GetPart1() =>
	data.Sum(line => line.value.Count(x => x.Count != 5 && x.Count != 6));

long GetPart2() =>
	data.Sum(getDecodedValue);

long getDecodedValue((HashSet<char>[] digits, HashSet<char>[] value) line)
{
	var digitMap = new HashSet<char>[10];

	// *** Single length matches ***

	digitMap[1] = line.digits.Single(d => d.Count == 2); // cf
	digitMap[4] = line.digits.Single(d => d.Count == 4); // bcdf
	digitMap[7] = line.digits.Single(d => d.Count == 3); // acf
	digitMap[8] = line.digits.Single(d => d.Count == 7); // abcdefg

	// *** Five digit length matches ***

	var fiveDigitValues = line.digits.Where(d => d.Count == 5).ToList();

	digitMap[2] = fiveDigitValues.Single(fdv => fdv.Intersect(digitMap[4]).Count() == 2); // acdef (shares cd with 4)
	fiveDigitValues.Remove(digitMap[2]);

	digitMap[3] = fiveDigitValues.Single(fdv => fdv.Intersect(digitMap[1]).Count() == 2); // acdfg (shares cf with 1)
	fiveDigitValues.Remove(digitMap[3]);

	digitMap[5] = fiveDigitValues.Single();

	// *** Six digit length matches ***

	var sixDigitValues = line.digits.Where(d => d.Count == 6).ToList();

	digitMap[6] = sixDigitValues.Single(fdv => fdv.Intersect(digitMap[1]).Count() == 1); // abdefg (shares f with 1)
	sixDigitValues.Remove(digitMap[6]);

	digitMap[9] = sixDigitValues.Single(fdv => fdv.Intersect(digitMap[4]).Count() == 4); // abcdfg (shares bcdf with 4)
	sixDigitValues.Remove(digitMap[9]);

	digitMap[0] = sixDigitValues.Single();

	// *** Match the digits ***

	var value = 0L;

	foreach (var digit in line.value)
		for (var digitIdx = 0; digitIdx < 10; ++digitIdx)
			if (digitMap[digitIdx].Count == digit.Count && digitMap[digitIdx].Intersect(digit).Count() == digit.Count)
			{
				value = value * 10 + digitIdx;
				break;
			}

	return value;
}

(HashSet<char>[] digits, HashSet<char>[] value) parseLine(string text)
{
	var pieces = text.Split('|');
	var digits = pieces[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToHashSet()).ToArray();
	var value = pieces[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToHashSet()).ToArray();
	return (digits, value);
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
