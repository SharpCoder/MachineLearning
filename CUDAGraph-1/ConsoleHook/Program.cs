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
			Graph graph = new Graph(new int[] { 1, 20, 100, 1 });

			while (true)
			{
				Console.Write("Enter a value: ");
				var strInput = Console.ReadLine();
				float input = 0f;
				if (float.TryParse(strInput, out input))
				{
					graph.Input(new float[] { input });
				}
			}

			Console.WriteLine("Program Terminated. \nPress any key to continue.");
			Console.ReadKey();
		}
	}
}
