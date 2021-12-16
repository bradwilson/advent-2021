using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

var stopwatch = Stopwatch.StartNew();

var data =
	(from line in File.ReadAllLines("input.txt")
	 where !string.IsNullOrWhiteSpace(line)
	 select parseLine(line)).First();

var idx = 0;
var (version, result) = parsePacket(ref idx);

long GetPart1() => version;

long GetPart2() => result;

(long version, long result) parsePacket(ref int idx)
{
	var versionTotal = 0L;
	var result = 0L;

	var version = Convert.ToInt32(data[idx..(idx + 3)], 2);
	versionTotal += version;

	var type = Convert.ToInt32(data[(idx + 3)..(idx + 6)], 2);

	idx += 6;

	if (type == 4)
	{
		var valueText = new StringBuilder();

		while (true)
		{
			var firstBit = data[idx++];

			valueText.Append(data[idx..(idx + 4)]);
			idx += 4;

			if (firstBit == '0')
				break;
		}

		result = Convert.ToInt64(valueText.ToString(), 2);
	}
	else
	{
		var results = new List<long>();
		var lengthType = data[idx++];
		if (lengthType == '0')
		{
			var length = Convert.ToInt32(data[idx..(idx + 15)], 2);
			idx += 15;
			var targetIndex = idx + length;

			while (idx < targetIndex)
			{
				var (v, r) = parsePacket(ref idx);
				versionTotal += v;
				results.Add(r);
			}
		}
		else
		{
			var packetCount = Convert.ToInt32(data[idx..(idx + 11)], 2);
			idx += 11;

			for (; packetCount > 0; --packetCount)
			{
				var (v, r) = parsePacket(ref idx);
				versionTotal += v;
				results.Add(r);
			}
		}

		switch (type)
		{
			case 0:
				result = results.Sum();
				break;

			case 1:
				result = results.Aggregate(1L, (total, value) => total * value);
				break;

			case 2:
				result = results.Min();
				break;

			case 3:
				result = results.Max();
				break;

			case 5:
				result = results[0] > results[1] ? 1 : 0;
				break;

			case 6:
				result = results[0] < results[1] ? 1 : 0;
				break;

			case 7:
				result = results[0] == results[1] ? 1 : 0;
				break;
		}
	}

	return (versionTotal, result);
}

string parseLine(string line)
{
	var result = new StringBuilder();

	for (var idx = 0; idx < line.Length; ++idx)
		result.Append(Convert.ToString(Convert.ToInt32(new string(new[] { line[idx] }), 16), 2).PadLeft(4, '0'));

	return result.ToString();
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
