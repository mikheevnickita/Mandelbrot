using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mandelbrot.Core;

namespace Mandelbrot.Console
{
	class Program
	{
		const Int32 W = 1920;
		const Int32 H = 1080;
		const Int32 C = 1;
		static List<String> log = new List<String>();

		static void Main(string[] args)
		{
			var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var array = CreateArray();

			foreach (var builder in new IBuilder[] { new CpuBuilder() })
			{
				using (var bm = new DirectBitmap(W, H))
				{
					var destPath = Path.Combine(desktop, builder.FileName);

					Measure(
						() =>
						{
							for (int i = 0; i < C; i++)
								builder.Calculate(array, W, H, new Complex(-2.277777, 1), new Complex(1.277777, -1));
						},
						sw => $"{builder.FileName,-8}: build x{C} times in {sw.ElapsedTicks/1000.0/C,7:## ##0.000} ms"
					);

					SaveBitmap(bm, array);
					bm.Bitmap.Save(destPath);
					//Process.Start(destPath);
				}

				foreach (var line in log)
					System.Console.WriteLine(line);

				System.Console.ReadKey();
			}
		}

		private static ushort[][] CreateArray()
		{
			var array = new UInt16[H][];
			for (int i = 0; i < H; i++)
				array[i] = new UInt16[W];
			return array;
		}

		private static void Measure(Action action, Func<Stopwatch, String> logmsg)
		{
			var sw = new Stopwatch();
			sw.Start();
			action?.Invoke();
			sw.Stop();

			log.Add(logmsg?.Invoke(sw));
		}

		private static void SaveBitmap(DirectBitmap bm, ushort[][] bytes)
		{
			for (int i = 0; i < H; i++)
				for (int j = 0; j < W; j++)
					bm.Bits[i*W + j] = GetColor(bytes[i][j]);
		}

		private static UInt32 GetColor(UInt32 sh)
		{
			if (sh > 0xFF)
				sh = 0xFF;

			return 0xFF000000 | (sh&0xFF) << 24 >> 8 | (sh & 0xFF) << 24 >> 16 | (sh & 0xFF) << 24 >> 24;
		}
	}
}
