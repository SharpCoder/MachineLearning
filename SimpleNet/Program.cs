using ProtoNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNet
{
    class Program
    {
        static void Main(string[] args)
        {
			int[] topology = new int[] { 1, 2, 3, 5 };
			float[] input = new float[topology[0]];

			for (int i = 0; i < input.Length; i++)
			{
				input[i] = 1.0f;
			}

			Stopwatch sw = new Stopwatch();
			ProtoNet protonetwork = new ProtoNet(topology);
            Network network = new Network(topology);

			List<float> simple_perf = new List<float>();
			List<float> complex_perf = new List<float>();

			for (int i = 0; i < 11; i++)
			{
				sw.Reset();
				sw.Start();
				network.getOutput(input);
				sw.Stop();
				simple_perf.Add(sw.ElapsedTicks);
				sw.Reset();
				sw.Start();
				protonetwork.GetValue(input);
				sw.Stop();
				complex_perf.Add(sw.ElapsedTicks);
			}

			// Calculate the averages.
			float avg1 = 0f, avg2 = 0f;
			for (int i = 1; i < simple_perf.Count; i++)
			{
				avg1 += simple_perf[i];
				avg2 += complex_perf[i];
			}

			avg1 /= (simple_perf.Count - 1);
			avg2 /= (complex_perf.Count - 1);

			Console.WriteLine("Proto:  {0} @   {1}", protonetwork.GetValue(input), avg2);
			Console.WriteLine("Simple: {0} @   {1}", network.getOutput(input), avg1);
            Console.ReadKey();
        }
    }
}
