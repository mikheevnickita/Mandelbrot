using System;

namespace Mandelbrot.Core
{
	public interface IBuilder
	{
		String FileName { get; }
		void Calculate(UInt16[,] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom);
	}
}