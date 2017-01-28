using System;
using System.Runtime.CompilerServices;

namespace Mandelbrot.Core
{
	public class CpuV2Builder : IBuilder
	{
		public String FileName => "cpu2.bmp";

		public void Calculate(UInt16[,] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom)
		{
			for (int i = 0; i < H; i++)
				for (int j = 0; j < W; j++)
					result[i,j] = Calc(
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
				var r = pointReal * pointReal + pointIm * pointIm;
				if (r > 4)
					return i;

				var tempReal = pointReal*pointReal - pointIm*pointIm + complex.Real;
				var tempIm = 2*pointReal*pointIm + complex.Im;

				pointReal = tempReal;
				pointIm = tempIm;
			}

			return 255;
		}
	}
}