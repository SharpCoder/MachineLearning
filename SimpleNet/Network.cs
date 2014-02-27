using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNet
{
    public class Network
    {
        int[] topology;
        float[] weights;
		float[] outputs;
		int start = 0;

        public Network(int[] network_topology)
        {
            if (network_topology == null || network_topology.Length == 0) return;
            this.topology = network_topology;

            // Calculate the number of connections.
			// The input for this graph is equal to each node on the first layer
			// times itself.
			int connection_limit = (int)Math.Pow(topology[0], 2);

			// Then we iterate over the network topology.
			for (int i = 0; i < network_topology.Length; i++)
			{
				if (i < network_topology.Length - 1)
					connection_limit += (network_topology[i] * network_topology[i + 1]);

				start += network_topology[i];
			}

            // Initialize the connection weights.
            weights = new float[connection_limit];
			outputs = new float[start];
            Random rand = new Random(424242);
            for (int i = 0; i < weights.Length; i++)
                weights[i] = (float)(rand.NextDouble() - rand.NextDouble());
        }

        public float getOutput(float[] input)
        {
			outputs = new float[start];//weights.Length + (topology[topology.Length - 2] * topology[topology.Length - 1])];
            for (int i = 0; i < input.Length; i++)
                outputs[i] = input[i];

			int weight_stack = 0, process_stack = 0;
			float output = 0f, val = 0f;

            for (int y = 1; y < topology.Length; y++)
			{
				for (int x = 0; x < topology[y]; x++)
				{
					// For each node in the graph. Calculate the value coming into it.
					val = 0;
					for (int z = 0; z < topology[y - 1]; z++)
					{
						int arrIndex = process_stack + z;
						val += outputs[arrIndex] * weights[weight_stack++];
					}

					val = sig(val);
					outputs[process_stack + x + topology[y - 1]] = val;
					if (y == topology.Length - 1) output += val;
				}

				process_stack += topology[y - 1];				
            }

			return output;
        }

        private float sig(float input)
        {
            return (float)(1.0f / (1.0f + Math.Exp(-input)));
        }
    }
}
