using System;
using System.Runtime.CompilerServices;

namespace Mandelbrot.Core
{
	public class CpuV5Builder : IBuilder
	{
		public String FileName => "cpu5.bmp";

		public void Calculate(UInt16[,] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom)
		{
			Double pLeftTopReal = pLeftTop.Real;
			Double pLeftTopIm = pLeftTop.Im;
			Double pRightBottomReal = pRightBottom.Real;
			Double pRightBottomIm = pRightBottom.Im;
			Double dReal = (pRightBottomReal - pLeftTopReal) / W;
			Double dIm = (pRightBottomIm - pLeftTopIm) / H;

			Double imagine = pLeftTopIm;
			for (int i = 0; i < H; i++)
			{
				Double real = pLeftTopReal;
				for (int j = 0; j < W; j++)
				{
					result[i,j] = Calc(real, imagine);
					real += dReal;
				}
				imagine += dIm;
			}
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