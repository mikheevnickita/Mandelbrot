using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Mandelbrot.Core
{
	public class CpuV6Builder1 : IBuilder
	{
		public String FileName => "cpu61.bmp";
		private const Int32 TaskCount = 128;

		public void Calculate(UInt16[,] result, Int32 W, Int32 H, Complex pLeftTop, Complex pRightBottom)
		{
			Double pLeftTopReal = pLeftTop.Real;
			Double pLeftTopIm = pLeftTop.Im;
			Double pRightBottomReal = pRightBottom.Real;
			Double pRightBottomIm = pRightBottom.Im;
			Double dReal = (pRightBottomReal - pLeftTopReal) / W;
			Double dIm = (pRightBottomIm - pLeftTopIm) / H;

			var tasks = Enumerable.Range(0, TaskCount).Select(i => new Task(() =>
			{
				InnerLoopForI(result, i, W, H, pLeftTopReal, pLeftTopIm, dReal, dIm);
			})).ToArray();

			foreach (var task in tasks)
				task.Start();

			Task.WaitAll(tasks);
		}

		private void InnerLoopForI(UInt16[,] result, Int32 k, Int32 W, Int32 H, Double pLeftTopReal, Double pLeftTopIm, Double dReal, Double dIm)
		{
			for (int i = k; i < H; i += TaskCount)
			{
				Double real = pLeftTopReal;
				Double imagine = pLeftTopIm + i * dIm;
				for (int j = 0; j < W; j++)
				{
					result[i,j] = Calc(real, imagine);
					real += dReal;
				}
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