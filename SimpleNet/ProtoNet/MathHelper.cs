// Author: SharpCoder
// Date: 2013-10-09
// Notes: This is just a helper function
// which will eventually have other activation
// functions beyond sigmoid.

using System;
namespace ProtoNetwork
{
	class MathHelper
	{
		public static float Sigmoid(float input)
		{
			return (float)(1f/(1f + Math.Exp(-input)));
		}

		public static float SigmoidIntegral(float input)
		{
			return (float)Math.Log(Math.Exp(input) + 1);
		}
	}
}
