using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FordFulkersonEdmondsKarp
{
	class Program
	{
		public static bool Bfs(int[][] residualGraph, int s, int t, int[] parent)
		{
			int amount = parent.Length;

			bool[] visited = new bool[amount];
			for (int i = 0; i < amount; ++i)
				visited[i] = false;

			Queue<int> queue = new Queue<int>();
			queue.Enqueue(s);
			visited[s] = true;
			parent[s] = -1;

			while (queue.Count != 0)
			{
				if (visited[t])
					break;
				var u = queue.Dequeue();

				for (var v = 0; v < amount; v++)
				{
					if (visited[v] || residualGraph[u][v] <= 0) continue;
					queue.Enqueue(v);
					parent[v] = u;
					visited[v] = true;
				}
			}

			return (visited[t]);
		}

		public static int FordFulkerson(int[][] graph, int s, int t, out int[][] residualGraph)
		{

			residualGraph = new int[graph.Length][];
			for (var i = 0; i < graph.Length; i++)
			{
				residualGraph[i] = new int[graph[i].Length];
				for (var j = 0; j < graph[i].Length; j++)
				{
					residualGraph[i][j] = graph[i][j];
				}
			}
			var parent = new int[graph.Length];
			var maxFlow = 0;
			int v, u;

			while (Bfs(residualGraph, s, t, parent))
			{
				int pathFlow = int.MaxValue;
				for (v = t; v != s; v = parent[v])
				{
					u = parent[v];
					pathFlow = Math.Min(pathFlow, residualGraph[u][v]);
				}
				for (v = t; v != s; v = parent[v])
				{
					u = parent[v];
					residualGraph[u][v] -= pathFlow;
					residualGraph[v][u] += pathFlow;
				}
				maxFlow += pathFlow;
			}
			return maxFlow;
		}

		static void Main()
		{
			var reader = new StreamReader("in.txt");
			var n = int.Parse(reader.ReadLine());
			var capacityMatrix = new int[n][];
			for (int i = 0; i < n; i++)
			{
				var line = reader.ReadLine();
				capacityMatrix[i] = line
					.Split(' ')
					.Select(int.Parse)
					.ToArray();
			}
			var source = int.Parse(reader.ReadLine()) - 1;
			var sink = int.Parse(reader.ReadLine()) - 1;
			reader.Dispose();

			int[][] residualGraph;

			var maxFlow = FordFulkerson(capacityMatrix, source, sink, out residualGraph);
			var output = new List<string>();

			for (int i = 0; i < residualGraph.Length; i++)
			{
				var line = "";
				for (int j = 0; j < residualGraph[i].Length; j++)
				{
					var diff = (capacityMatrix[i][j] - residualGraph[i][j]);
					diff = diff > 0 ? diff : 0;
					line += diff + " ";
				}
				output.Add(line);
			}
			output.Add(maxFlow.ToString());
			File.WriteAllLines("out.txt", output);
		}
	}
}
