using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHook
{
	public class Graph
	{
		Random rand;
		double startTicks;
		ValPtr[] outputs;
		ValPtr[] inputs;
		List<Node> flattened = new List<Node>();

		public Graph(int[] topology)
		{
			if (topology == null || topology.Length == 0) return;
			this.rand = new Random(1337);
			this.inputs = new ValPtr[topology[0]];

			// Instantiate the inputs.
			for (int i = 0; i < this.inputs.Length; i++)
				this.inputs[i] = new ValPtr(0f);

			// Then deploy the network.
			deploy(inputs, topology, 0);
		}

		public void Input(float[] values)
		{
			startTicks = DateTime.UtcNow.Ticks;
			if (values == null || values.Length != inputs.Length) return;
			for (int i = 0; i < values.Length; i++)
				this.inputs[i].Set(values[i]);
		}

		private float getOutput()
		{
			float result = 0f;
			if (this.outputs != null)
				for (int i = 0; i < outputs.Length; i++)
					result += outputs[i].value;
			return result;
		}

		private void deploy(ValPtr[] inputs, int[] topology, int index)
		{
			if (index + 1 > topology.Length)
			{
				// Confusing but... the input to this function is the output of the graph.
				outputs = inputs;

				// And while we're at it, let's hook up these output nodes
				// to fire an output event when they are loaded.
				for (int i = 0; i < inputs.Length; i++)
				{
					inputs[i].onChange = (x) =>
					{
						double end = DateTime.UtcNow.Ticks - startTicks;
						Console.ForegroundColor = ConsoleColor.Green;
						Console.WriteLine("\n" + ((ValPtr)x).value + " @ " + end + " ticks");
						Console.ResetColor();
					};
				}

				// Return since we've hit the end of the graph.
				return;
			}

			Node[] layer = new Node[topology[index]];
			ValPtr[] nextInputs = new ValPtr[layer.Length];

			// Iterate over the layer and create each node.
			for (int i = 0; i < layer.Length; i++)
			{
				layer[i] = createNode(inputs);
				nextInputs[i] = layer[i].output;
			}
			
			// Recursively deploy.
			deploy(nextInputs, topology, ++index);
		}

		private Node createNode(ValPtr[] inputs)
		{
			// Create the node.
			Node node = new Node(inputs, rand);
			flattened.Add(node);

			// Initialize a new thread to contain it.
			Task.Factory.StartNew(() =>
			{
				while (true)
				{
					node.Eval();

					// NOTE: This needs to be removed, but for the sanity of my test environment
					// I have included it. I don't think it would be required if you delegate
					// to the GPU?
					System.Threading.Thread.Sleep(10);
				}
			});			

			// Return the node.
			// NOTE: It should already be pumping off evaluations.
			return node;
		}
	}
}
