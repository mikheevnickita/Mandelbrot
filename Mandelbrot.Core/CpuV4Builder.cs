using System;
using System.Runtime.CompilerServices;

namespace Mandelbrot.Core
{
	public class CpuV4Builder : IBuilder
	{
		public String FileName => "cpu4.bmp";

		public void Calculate(UInt16[][] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom)
		{
			for (int i = 0; i < H; i++)
				for (int j = 0; j < W; j++)
					result[i][j] = Calc(
						pLeftTop.Real + (pRightBottom.Real - pLeftTop.Real) / W * j,
						pLeftTop.Im + (pRightBottom.Im - pLeftTop.Im) / H * i
						);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private UInt16 Calc(Double complexReal, Double complexIm)
		{
			var pointReal = 0D;
			var pointIm = 0D;

			for (UInt16 i = 0; i < 255; i++)
			{
				var r = pointReal * pointReal + pointIm * pointIm;
				if (r > 4)
					return i;

				var tempReal = pointReal * pointReal - pointIm * pointIm + complexReal;
				pointIm = 2 * pointReal * pointIm + complexIm;

				pointReal = tempReal;
			}

			return 255;
		}
	}
}