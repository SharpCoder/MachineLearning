using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGraph
{
	public class Node
	{
		internal float weight;
		internal float[] inputs;
		int index = 0;
		public Node[] children;

		public Node(int inputCount, Random rand)
		{
			if (rand == null) return;
			this.inputs = new float[inputCount];
			this.weight = (float)(rand.NextDouble() - rand.NextDouble());
			this.children = null;
		}

		public void Push(float val)
		{
			if (index + 1 > inputs.Length) index = 0;
			this.inputs[index++] = val;
		}

		public float Eval()
		{
			if (index != inputs.Length) return 0f;

			float outVal = 0f;
			for (int i = 0; i < inputs.Length; i++)
			{
				outVal += inputs[i] * weight;
			}

			if (children != null)
			{
				float output = 0f;
				for (int i = 0; i < children.Length; i++)
				{
					children[i].Push(outVal);
					output += children[i].Eval();
				}
				return output;
			}

			return outVal;
		}
	}
}
