using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DoggyDog
{
	class Program
	{
		private static int[] pointsOfInterest;
		private static int[] meetingPoints;
		private static int amountOfVertices;
		private static int amountOfEdges;
		private static int m;
		private static int[][] graph;
		private static bool[] visited;
		private static int[] matching;

		static void Main(string[] args)
		{
			#region reading input
			var lines = File.ReadAllLines("in.txt");

			var firstLine = lines[0].Split(' ').Select(int.Parse).ToArray();
			amountOfVertices = firstLine[0];
			amountOfEdges = amountOfVertices - 1;
			m = firstLine[1];
			meetingPoints = lines[1].Split(' ').Select(int.Parse).ToArray();
			pointsOfInterest = lines[2].Split(' ').Select(int.Parse).ToArray();
			#endregion
			#region Creating graph
			visited = new bool[amountOfEdges];
			var edges = new bool[amountOfEdges][];
			matching = Enumerable.Range(0, m).Select(x => -1).ToArray();

			for (int i = 0; i < amountOfEdges; i++)
			{
				edges[i] = new bool[m];
				for (int j = 0; j < m; j++)
				{
					edges[i][j] = CanGoToPointOfInterest(meetingPoints[2 * i],
														meetingPoints[2 * i + 1],
														meetingPoints[2 * i + 2],
														meetingPoints[2 * i + 3],
														pointsOfInterest[2 * j],
														pointsOfInterest[2 * j + 1]);
				}
			}

			graph = new int[amountOfEdges][];
			for (int i = 0; i < amountOfEdges; i++)
			{
				graph[i] = edges[i].Select((z, index) => Tuple.Create(z, index))
					.Where(z => z.Item1)
					.Select(z => z.Item2)
					.ToArray();
			}

			#endregion
			for (int v = 0; v < amountOfEdges; ++v)
			{
				visited = new bool[amountOfEdges];
				TryKuhn(v);
			}

			for (int i = 0; i < m; ++i)
				if (matching[i] != -1)
					Console.WriteLine("{0} {1}", matching[i], i);

			File.WriteAllLines("out.txt", new[] { (meetingPoints.Length / 2 + matching.Where(x => x != -1).Count()).ToString() });
		}

		public static bool TryKuhn(int v)
		{
			if (visited[v]) return false;
			visited[v] = true;
			for (int i = 0; i < graph[v].Length; ++i)
			{
				int to = graph[v][i];
				if (matching[to] == -1 || TryKuhn(matching[to]))
				{
					matching[to] = v;
					return true;
				}
			}
			return false;
		}

		public static double GetDistance(double x1, double y1, double x2, double y2)
		{
			var distance = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
			return distance;
		}

		public static bool CanGoToPointOfInterest(double bobStartX, double bobStartY, double bobEndX, double bobEndY, double interestX, double interestY)
		{
			return GetDistance(bobStartX, bobStartY, bobEndX, bobEndY) * 2 >
				   GetDistance(bobStartX, bobStartY, interestX, interestY) + GetDistance(interestX, interestY, bobEndX, bobEndY);
		}

	}
}
