CUDAGraph-1
===============

This project is designed to explore my theory of utilizing threads in a GPU to evaluate a graph of data nearly instantaneously. The scope of this test will be incredibly limited. I won't even use CUDA to support my hypothesis, I plan to simply use pointer values and C# threads - just as a proof of concept. For more information on my theory, here is an excerpt from my blog: "Traditional graph summation is recursive in nature and typically involves computing a value and then pushing it forward to the children nodes. But imagine a graph where each node is stored in its own thread. Each node in the graph would have a very simple task - pull the value of its parent, apply whatever transformations, and then update it's own value. That's it. Sound's simple, right?"

*Hypothesis*: by implementing a pull architecture and utilizing the crazy threading ability of a GPU, graphs could be evaluated nearly instantly. Since each node is constantly evaluating itself and using a pointer to the parent node[s], layers would never have to be notified that something changed. Everything would propagate naturally throughout the graph and the output would simply happen. Additionally, as the graph topology becomes more complicated, evaluation times should remain roughly unchanged due to the parallel nature of evaluation.

Blog can be found here: http://neuralresearch.blogspot.com/2014/02/parallel-graph-traversal.html

**CPUThreadedGraph**
This project contains a simplistic implementation of my theory. Due to the nature of CPU threading, however, it is absolutely *not* scalable.  By that I mean, it breaks at any marginally complicated topology. I threw in a System.Threading.Thread.Sleep(1) method in there which made the system work beautifully, however, it severely  crippled my benchmarking abilities and skewed the results. That being said, I did find out that my hypothesis seems to be correct. The evaluation time didn't fluctuate very much, even as the graph become exponentially more complicated.

**SimpleGraph**
This project is a simple, recursive implementation of the other project. I will be using it to benchmark against the CUDA version later on and to gather preliminary observations.