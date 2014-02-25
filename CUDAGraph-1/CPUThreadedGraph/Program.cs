using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHook
{
	class Program
	{
		static void Main(string[] args)
		{
			double avg = 0d;
			Graph graph = new Graph(new int[] { 1, 2, 1 });

			Console.WriteLine("Waiting 2 seconds for the threads to initialize");
			System.Threading.Thread.Sleep(2000);

			// Do the calculations.
			for (int i = 0; i < 11; i++)
			{
				graph.Input(new float[] { i });
				System.Threading.Thread.Sleep(500);
			}

			// Now do the calculations for the benchmark system.
			// (wait for all the threads to catch up, just in case).
			System.Threading.Thread.Sleep(1000);

			// Iterate over all the benchmarks.
			for (int i = 1; i < graph.benchmark.Count; i++)
				avg += graph.benchmark[i];

			// Divide the total by the count.
			avg /= (graph.benchmark.Count - 1);

			// Write the average.
			Console.WriteLine("Average " + avg + " ticks");
			Console.WriteLine("Program Terminated. \nPress any key to continue.");
			Console.ReadKey();
		}
	}
}
