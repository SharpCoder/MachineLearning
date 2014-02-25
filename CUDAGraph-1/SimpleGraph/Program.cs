using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraph
{
	class Program
	{
		static void Main(string[] args)
		{
			Graph graph = new Graph(new int[] { 1, 2, 1 });

			Console.WriteLine("Performing basic graph traversal");

			for (int i = 0; i < 11; i++)
			{
				graph.Evaluate(new float[] { i });
			}

			// NOTE: The following code is for benchmarking purposes.
			// I skip the first iteration to account for initialization costs.
			double avg = 0d;
			for ( int i = 1; i < graph.benchmark.Count; i++ )
				avg += graph.benchmark[i];
			avg /= (graph.benchmark.Count - 1);

			Console.WriteLine("Average ticks: " + avg);
			Console.WriteLine("Program Terminated. Press any key to continue.");
			Console.ReadKey();
		}
	}
}
