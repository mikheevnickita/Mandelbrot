using System;
using System.Runtime.CompilerServices;

namespace Mandelbrot.Core
{
	public class CpuV3Builder : IBuilder
	{
		public String FileName => "cpu3.bmp";

		public void Calculate(UInt16[][] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom)
		{
			for (int i = 0; i < H; i++)
				for (int j = 0; j < W; j++)
					result[i][j] = Calc(
						new Complex(
							pLeftTop.Real + (pRightBottom.Real - pLeftTop.Real) / W * j,
							pLeftTop.Im + (pRightBottom.Im - pLeftTop.Im) / H * i
							)
						);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private UInt16 Calc(Complex complex)
		{
			var pointReal = 0D;
			var pointIm = 0D;

			for (UInt16 i = 0; i < 255; i++)
			{
				var prs = pointReal*pointReal;
				var pis = pointIm*pointIm;
				var r = prs + pis;
				if (r > 4)
					return i;

				var tReal = prs - pis + complex.Real;
				var tIm = 2 * pointReal * pointIm + complex.Im;

				pointReal = tReal;
				pointIm = tIm;
			}

			return 255;
		}
	}
}