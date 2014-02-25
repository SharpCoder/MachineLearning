CUDAGraph-1
===============

This project is designed to explore my theory of utilizing threads in a GPU to evaluate a graph of data nearly instantaneously. An excerpt from my blog: "Traditional graph summation is recursive in nature and typically involves computing a value and then pushing it forward to the children nodes. But imagine a graph where each node is stored in its own thread. Each node in the graph would have a very simple task - pull the value of its parent, apply whatever transformations, and then update it's own value. That's it. Sound's simple, right?"

*Hypothesis*: by implementing a pull architecture and utilizing the crazy threading ability of a GPU, graphs could be evaluated nearly instantly. Since each node is constantly evaluating itself and using a pointer to the parent node[s], layers would never have to be notified that something changed. Everything would propagate naturally throughout the graph and the output would simply happen. Additionally, as the graph topology becomes more complicated, evaluation times should remain roughly unchanged due to the parallel nature of evaluation.

Blog can be found here: http://neuralresearch.blogspot.com/2014/02/parallel-graph-traversal.html