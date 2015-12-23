using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;

namespace YPD
{
	class Program
	{
		static void Main()
		{
			var lines = File.ReadAllLines("in.txt");

			#region reading input
			var n = int.Parse(lines[0]);
			var points = new Point[n];
			for (int i = 1; i <= n; i++)
			{
				var input = lines[i];
				var coords = input.Split(' ').Select(int.Parse).ToArray();
				points[i - 1] = new Point(coords[0], coords[1]);
			}
			#endregion

			var spanningTree = new List<int>[n];
			for (int i = 0; i < n; i++)
			{
				spanningTree[i] = new List<int>();
			}

			var distance = new double[n];
			var parents = new int[n];

			for (int i = 1; i < n; i++)
			{
				distance[i] = ManhattanLength(points[0], points[i]);
				parents[i] = 0;
			}

			var visited = new List<int> {0};


			while (visited.Count < n)
			{
				var nearest = Enumerable.Range(0, n)
					.Except(visited)
					.OrderBy(i => distance[i])
					.First();

				spanningTree[nearest].Add(parents[nearest]);
				spanningTree[parents[nearest]].Add(nearest);

				visited.Add(nearest);

				for (int i = 0; i < n; i++)
				{
					var newDistance = ManhattanLength(points[nearest], points[i]);
					if (distance[i] > newDistance)
					{
						distance[i] = newDistance;
						parents[i] = nearest;
					}
				}
			}

			var output = spanningTree.Select(
				list => list.OrderBy(x => x)
							.Select(x => (x + 1).ToString())
						.Aggregate((x, a) => x + ' ' + a) + " 0")
						.ToList();
			
			double sum = 0;

			for(int i=0;i<n;i++)
				for (int j = 0; j < spanningTree[i].Count; j++)
				{
					sum += ManhattanLength(points[i], points[spanningTree[i][j]]);
					Console.WriteLine(ManhattanLength(points[i], points[j]));
				}
			output.Add((sum/2).ToString(CultureInfo.InvariantCulture));
			Console.WriteLine(sum);
			File.WriteAllLines("out.txt", output);
		}

		static double ManhattanLength(Point from, Point to)
		{
			return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
		}

	}
}
