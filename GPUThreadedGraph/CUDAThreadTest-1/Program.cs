using ManagedCuda;
using ManagedCuda.BasicTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUDAGraph_2
{
	class Program
	{
		const int THREADS_PER_BLOCK = 1024;
		const int VECTOR_SIZE = 5120;

		static CudaKernel kernel;
		static void Main(string[] args)
		{
			// NOTE: You need to change this location to match your own machine.
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("NOTE: You must change the kernel location before running this project so it matches your own environment.");
			Console.ResetColor();
			System.Threading.Thread.Sleep(500);

			string path = @"X:\MachineLearning\CUDAGraph-2\CUDAGraph_Kernel\Debug\kernel.cu.ptx";
			CudaContext ctx = new CudaContext();
			CUmodule module = ctx.LoadModule(path);
			kernel = new CudaKernel("kernel", module, ctx);

			// This tells the kernel to allocate a lot of threads for the Gpu.
			kernel.BlockDimensions = THREADS_PER_BLOCK;
			kernel.GridDimensions = VECTOR_SIZE / THREADS_PER_BLOCK + 1; ;

			// Now let's load the kernel!
			// Create the topology.
			int[] topology = new int[] { 1, 200, 200, 100, 1 };

			int height = topology.Length;
			int width = 0;

			for (int i = 0; i < topology.Length; i++)
				if (width < topology[i]) width = topology[i];

			// Launch!
			float[] res = new float[height * width];
			for (int i = 0; i < 10; i++)
			{
				float[] matrix = new float[height * width];
				float[] weights = new float[height * width];
				Random rand = new Random(424242);
				for (int y = 0; y < height; y++)
				{
					for (int x = 0; x < width; x++)
					{
						matrix[y * width + x] = (y == 0 && x < topology[y]) ? 1.0f : 0;
						weights[y * width + x] = (x < topology[y]) ? (float)(rand.NextDouble() - rand.NextDouble()) : 0;
					}
				}

				// Load the kernel with some variables.
				CudaDeviceVariable<int> cuda_topology = topology;
				CudaDeviceVariable<float> cuda_membank = matrix;
				CudaDeviceVariable<float> cuda_weights = weights;

				Stopwatch sw = new Stopwatch();
				sw.Start();
				kernel.Run(cuda_topology.DevicePointer, cuda_membank.DevicePointer, cuda_weights.DevicePointer, height, width);
				cuda_membank.CopyToHost(res);
				sw.Stop();

				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("{0} ticks to compute -> {1}", sw.ElapsedTicks, res[0]);
				Console.ResetColor();

			}

			Console.ReadKey();
		}
		
	}
}
