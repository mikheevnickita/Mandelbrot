using System;

namespace Mandelbrot.Core
{
	public struct Complex
	{
		public Double Real;
		public Double Im;

		public Complex(Double real, Double imagine)
		{
			Real = real;
			Im = imagine;
		}
	}
}