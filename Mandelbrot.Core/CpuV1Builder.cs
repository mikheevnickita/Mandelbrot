using System;
using System.Runtime.CompilerServices;

namespace Mandelbrot.Core
{
	public class CpuV1Builder : IBuilder
	{
		public String FileName => "cpu1.bmp";

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
			var point = new Complex(0, 0);

			for (var i = 0; i < 255; i++)
			{
				var r = point.Real * point.Real + point.Im * point.Im;
				if (r > 4)
					return (UInt16)i;

				point = new Complex(
					point.Real * point.Real - point.Im * point.Im + complex.Real,
					2 * point.Real * point.Im + complex.Im
					);
			}

			return 255;
		}
	}
}