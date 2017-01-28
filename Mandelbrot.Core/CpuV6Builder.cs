using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Mandelbrot.Core
{
	public class CpuV6Builder : IBuilder
	{
		public String FileName => "cpu6.bmp";

		public void Calculate(UInt16[,] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom)
		{
			Double pLeftTopReal = pLeftTop.Real;
			Double pLeftTopIm = pLeftTop.Im;
			Double pRightBottomReal = pRightBottom.Real;
			Double pRightBottomIm = pRightBottom.Im;
			Double dReal = (pRightBottomReal - pLeftTopReal) / W;
			Double dIm = (pRightBottomIm - pLeftTopIm) / H;

			Parallel.For(0, H, i => InnerLoopForI(result, W, i, pLeftTopReal, pLeftTopIm, dReal, dIm));
		}

		private void InnerLoopForI(UInt16[,] result, Int32 W, Int32 i, Double pLeftTopReal, Double pLeftTopIm, Double dReal, Double dIm )
		{
			Double real = pLeftTopReal;
			Double imagine = pLeftTopIm + i * dIm;
			for (int j = 0; j < W; j++)
			{
				result[i,j] = Calc(real, imagine);
				real += dReal;
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