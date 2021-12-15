using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var stopwatch = Stopwatch.StartNew();

long GetPart1()
{
	var data =
		(from line in File.ReadAllLines("input.txt")
		 where !string.IsNullOrWhiteSpace(line)
		 select line.Select(x => new Node { RiskLevel = int.Parse(new string(new[] { x })) }).ToArray()).ToArray();

	var nodes = data.Select(x => x.ToArray()).ToArray();

	return getLowestRisk(nodes);
}

long GetPart2()
{
	var data =
		(from line in File.ReadAllLines("input.txt")
		 where !string.IsNullOrWhiteSpace(line)
		 select line.Select(x => new Node { RiskLevel = int.Parse(new string(new[] { x })) }).ToArray()).ToArray();

	var nodes = data.Select(x => x.ToList()).ToList();

	for (var idx = 1; idx < 5; ++idx)
		for (var x = 0; x < data.Length; x++)
			for (var y = 0; y < data[0].Length; y++)
			{
				var newRiskLevel = nodes[x][y].RiskLevel + idx;
				while (newRiskLevel > 9)
					newRiskLevel -= 9;
				nodes[x].Add(new Node { RiskLevel = newRiskLevel });
			}

	var maxX = nodes.Count;
	var maxY = nodes[0].Count;
	var currentX = 0;

	for (var idx = 1; idx < 5; ++idx)
	{
		for (var x = 0; x < maxX; ++x, ++currentX)
		{
			var newRow = new List<Node>();

			for (var y = 0; y < maxY; y++)
			{
				var newRiskLevel = nodes[currentX][y].RiskLevel + 1;
				while (newRiskLevel > 9)
					newRiskLevel -= 9;
				newRow.Add(new Node { RiskLevel = newRiskLevel });
			}

			nodes.Add(newRow);
		}
	}

	return getLowestRisk(nodes);
}

void printNodes(IReadOnlyList<IReadOnlyList<Node>> nodes)
{
	for (var x = 0; x < nodes.Count; ++x)
	{
		if (x % 10 == 0)
			Console.WriteLine();

		for (var y = 0; y < nodes[0].Count; ++y)
		{
			if (y % 10 == 0)
				Console.Write(" ");

			Console.Write(nodes[x][y].RiskLevel);
		}

		Console.WriteLine();
	}
}

long getLowestRisk(IReadOnlyList<IReadOnlyList<Node>> nodes)
{
	var maxX = nodes.Count;
	var maxY = nodes[0].Count;

	for (var x = 0; x < maxX; ++x)
		for (var y = 0; y < maxY; ++y)
		{
			var node = nodes[x][y];
			node.X = x;
			node.Y = y;

			if (y > 0)
				node.Connections.Add(nodes[x][y - 1]);
			if (y < maxY - 1)
				node.Connections.Add(nodes[x][y + 1]);
			if (x > 0)
				node.Connections.Add(nodes[x - 1][y]);
			if (x < maxX - 1)
				node.Connections.Add(nodes[x + 1][y]);
		}

	var start = nodes[0][0];
	var end = nodes[maxX - 1][maxY - 1];

	mapPaths(nodes, start, end);

	var shortestPath = new List<Node> { end };
	buildShortedPath(shortestPath, end);
	shortestPath.Reverse();

	return shortestPath.Aggregate(-start.RiskLevel, (x, y) => x + y.RiskLevel);
}

void buildShortedPath(List<Node> list, Node node)
{
	if (node.PathBack == null)
		return;

	list.Add(node.PathBack);
	buildShortedPath(list, node.PathBack);
}

void mapPaths(IReadOnlyList<IReadOnlyList<Node>> nodes, Node start, Node end)
{
	start.MinRiskToStart = 0;
	var queue = new List<Node> { start };

	do
	{
		queue = queue.OrderBy(x => x.MinRiskToStart).ToList();
		var node = queue.First();
		queue.Remove(node);

		foreach (var childNode in node.Connections.OrderBy(x => x.RiskLevel))
		{
			if (childNode.Visited)
				continue;
			if (childNode.MinRiskToStart == null || node.MinRiskToStart + childNode.RiskLevel < childNode.MinRiskToStart)
			{
				childNode.MinRiskToStart = node.MinRiskToStart + childNode.RiskLevel;
				childNode.PathBack = node;

				if (!queue.Contains(childNode))
					queue.Add(childNode);
			}
		}

		node.Visited = true;

		if (node == end)
			return;
	} while (queue.Any());
}

Console.WriteLine($"[{stopwatch.Elapsed}] Pre-compute");

stopwatch = Stopwatch.StartNew();
var part1Result = GetPart1();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 1: {part1Result}");

stopwatch = Stopwatch.StartNew();
var part2Result = GetPart2();
Console.WriteLine($"[{stopwatch.Elapsed}] Part 2: {part2Result}");

class Node
{
	public int X;
	public int Y;
	public long RiskLevel;
	public Node? PathBack;
	public long? MinRiskToStart;
	public bool Visited;
	public List<Node> Connections = new();
}
