using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraph
{
	public class Graph
	{
		Random rand;
		Node inputNode;
		public List<Node> flattened = new List<Node>();
		public List<double> benchmark = new List<double>();
		Stopwatch bm = new Stopwatch();

		public Graph(int[] topology)
		{
			if (topology == null || topology.Length == 0) return;

			this.rand = new Random(424242);
			inputNode = new Node(topology[0], this.rand);
			flattened.Add(inputNode);
			deploy(new Node[] { inputNode }, topology, 1);
		}

		public float Evaluate(float[] input)
		{
			bm.Reset();
			bm.Start();
			if (inputNode == null || input == null || input.Length == 0) return 0f;
			for (int i = 0; i < input.Length; i++)
				inputNode.Push(input[i]);

			float output = inputNode.Eval();
			Console.ForegroundColor = ConsoleColor.Green;
			double benchmarkValue = bm.ElapsedTicks;
			Console.Write("\n" + output + " @ " + benchmarkValue + " ticks");
			benchmark.Add(benchmarkValue);
			Console.ResetColor();
			return output;
		}

		private void deploy(Node[] parents, int[] topology, int index)
		{
			if (index + 1 > topology.Length)
			{
				// Output layer.
				return;
			}

			Node[] layer = new Node[topology[index]];
			for (int i = 0; i < layer.Length; i++)
			{
				layer[i] = new Node(parents.Length, this.rand);
				flattened.Add(layer[i]);
			}

			// Iterate over the parents.
			for (int i = 0; i < parents.Length; i++)
			{
				parents[i].children = layer;
			}

			// Recurse.
			deploy(layer, topology, index + 1);
		}
	}
}
