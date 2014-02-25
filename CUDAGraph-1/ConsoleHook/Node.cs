using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHook
{
	public class Node
	{
		public float weight;
		public ValPtr output;
		public ValPtr[] parents;

		public Node(ValPtr[] parentNodes, Random rand)
		{
			if (rand == null) return;
			this.output = new ValPtr(0f);
			this.weight = (float)(rand.NextDouble() - rand.NextDouble());
			this.parents = parentNodes;
		}

		public void Eval()
		{
			// Simple null checking.
			if (this.parents == null) return;

			// Create a variable for the output.
			float outVal = 0f;
			for (int i = 0; i < parents.Length; i++)
				outVal += parents[i].value * weight;

			this.output.Set(outVal);
		}
	}
}
