using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Mandelbrot.Core
{
	public class CpuV7Builder : IBuilder
	{
		public String FileName => "cpu7.bmp";

		public void Calculate(UInt16[,] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom)
		{
			Double pLeftTopReal = pLeftTop.Real;
			Double pLeftTopIm = pLeftTop.Im;
			Double pRightBottomReal = pRightBottom.Real;
			Double pRightBottomIm = pRightBottom.Im;
			Double dReal = (pRightBottomReal - pLeftTopReal) / W;
			Double dIm = (pRightBottomIm - pLeftTopIm) / H;

			Parallel.For(0, H, i => InnerLoopForI(result, W, H, i, pLeftTopReal, pLeftTopIm, dReal, dIm));
		}

		private void InnerLoopForI(UInt16[,] result, Int32 W, Int32 H, Int32 i, Double pLeftTopReal, Double pLeftTopIm, Double dReal, Double dIm)
		{
			var vThreshold = new Vector<Single>(4.0f);
			var realArr = new Single[vSize];

			var vCIm = new Vector<Single>((Single)(pLeftTopIm + i * dIm));
			for (int j = 0; j < W; j += vSize)
			{
				for (var q = 0; q < vSize; q++)
					realArr[q] = (Single)(pLeftTopReal + (j + q) * dReal);

				var vCReal = new Vector<Single>(realArr);

				var vReal = Vector<Single>.Zero;
				var vIm = Vector<Single>.Zero;

				var vIters = Vector<int>.Zero;
				var vIncrement = Vector<int>.One;

				var it = 0;
				do
				{
					it++;
					vIters += vIncrement;

					var vRealSq = vReal * vReal;
					var vImSq = vIm * vIm;

					var vTempReal = vRealSq - vImSq + vCReal;
					vIm = 2 * vReal * vIm + vCIm;
					vReal = vTempReal;

					var vLessThanThreshold = Vector.LessThanOrEqual(vRealSq + vImSq, vThreshold);
					vIncrement &= vLessThanThreshold;
				}
				while (it <= 255 && vIncrement != Vector<int>.Zero);

				for (var q = 0; q < vSize; q++)
					result[i, j + q] = (UInt16)vIters[q];
			}

		}

		private static readonly Int32 vSize = Vector<Single>.Count;
	}
}