using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

var data =
	makeMultiDictionary(
		from line in File.ReadAllLines("input.txt")
		where !string.IsNullOrWhiteSpace(line)
		select line.Split('-')
	);

long GetPart1() => findAllPaths(false).Count;

long GetPart2() => findAllPaths(true).Count;

Dictionary<string, List<string>> makeMultiDictionary(IEnumerable<string[]> lines)
{
	var result = new Dictionary<string, List<string>>();

	foreach (var line in lines)
	{
		if (!result.TryGetValue(line[0], out var list))
		{
			list = new List<string>();
			result[line[0]] = list;
		}

		list.Add(line[1]);

		if (!result.TryGetValue(line[1], out var list2))
		{
			list2 = new List<string>();
			result[line[1]] = list2;
		}

		list2.Add(line[0]);
	}

	return result;
}

bool isLower(string value) => value.All(c => char.IsLower(c));

List<List<string>> findAllPaths(bool canHaveDoubleSmallCaveVisit) =>
	findAllSubPaths(canHaveDoubleSmallCaveVisit, new List<string>(), "start").ToList();

var emptyList = new List<List<string>>();

IEnumerable<List<string>> findAllSubPaths(bool canHaveDoubleSmallCaveVisit, IReadOnlyCollection<string> path, string node)
{
	if (isLower(node) && path.Contains(node))
	{
		if (canHaveDoubleSmallCaveVisit && node != "start" && node != "end")
			canHaveDoubleSmallCaveVisit = false;
		else
			yield break;
	}

	var subPath = new List<string>(path) { node };

	if (node == "end")
	{
		yield return subPath;
		yield break;
	}

	foreach (var neighbor in data[node])
		foreach (var result in findAllSubPaths(canHaveDoubleSmallCaveVisit, subPath, neighbor))
			yield return result;
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");
