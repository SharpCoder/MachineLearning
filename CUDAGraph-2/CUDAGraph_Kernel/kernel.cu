#define _SIZE_T_DEFINED
#ifndef __CUDACC__
#define __CUDACC__
#endif
#ifndef __cplusplus
#define __cplusplus
#endif

#include "cuda_runtime.h"
#include <stdio.h>
#include <cmath>

extern "C" {

	// Takes 2 variables,
	// int* topology should point to an integer array.
	// float* out will be mapped to a huge output array.
	// int N is the length of the topology array.
	__global__ void kernel(int* topology, float* membank, float* weights, int TOPOLOGY_WIDTH, int NODE_WIDTH)
	{
		// Get the thread index.
		int threadIndex = threadIdx.x + (blockDim.x * blockIdx.x);
		
		// Calculate the layer.
		int layer = ( threadIndex / NODE_WIDTH );
	
		// Calculate the offset.
		int node = ( threadIndex % NODE_WIDTH );

		// Validate the datas.
		if ( layer > 0 && layer < TOPOLOGY_WIDTH + 1 ) {
			if ( node < topology[layer]) {
				
				// This is a valid case.
				// So first we need to start the loop.
				int terminate = 1000;
				float nodeOut = 0;
				bool stop = false;
				while ( !stop && terminate-- > 0) {

					// Set stop to true so that only a failure will make us iterate again.
					stop = true;

					// Now we iterate over each node above us.
					int max = layer - 1;
					for ( int i = 0; i < topology[max]; i++ ) {
						int arrayIndex = (max) * NODE_WIDTH + i;
						// Check the respsective sources.
						if ( membank[arrayIndex] == 0 ) {
							// If something hasn't been pushed to it yet, let's abort.
							stop = false;
							break;
						} else {
							// Otherwise, there is a value here! So let's add it to our collective.
							nodeOut += membank[arrayIndex] * weights[arrayIndex];
						}
					}

					if ( !stop ) continue;
					
					// Compute sigmoid.
					//nodeOut = nodeOut;//1.0 / ( 1.0 + exp(-nodeOut));

					// If we don't want to stop, it means we've added all of the nodes
					// we needed to. So let's push our value.
					membank[(layer) * NODE_WIDTH + node] = nodeOut;
				}

			}
		}

		__syncthreads();
		__shared__ float total;
		total = 0;
		for ( int i = 0; i < topology[TOPOLOGY_WIDTH - 1]; i++ ) {
			int arrayIndex = ( ( TOPOLOGY_WIDTH - 1 ) * NODE_WIDTH ) + i;
			total = membank[arrayIndex] * weights[arrayIndex];
		}

		
		__syncthreads();
		membank[0] = total;

	}

	int main()
	{
		return 0;
	}
}