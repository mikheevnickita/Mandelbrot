using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Mandelbrot.Core;

namespace Mandelbrot.Console
{
	class Program
	{
		const Int32 W = 1920;
		const Int32 H = 1080;
		const Int32 C = 50;
		static readonly string Desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		static UInt16[,] Array;

		static void Main(string[] args)
		{
			GetImplemenations<IBuilder>()
			.ForEach(builder =>
			{
				using (var bm = new DirectBitmap(W, H))
				{
					var destPath = Path.Combine(Desktop, builder.FileName);

					Array = CreateArray();

					Measure(
						() =>
						{
							for (int i = 0; i < C; i++)
								builder.Calculate(Array, W, H, new Complex(-2.277777, 1), new Complex(1.277777, -1));
						},
						sw => $"{builder.FileName,-10}: build x{C} times in {(Double)(sw.ElapsedMilliseconds)/C,9:# ##0.000} ms"
					);

					SaveBitmap(bm, Array);
					bm.Bitmap.Save(destPath);
					//Process.Start(destPath);
				}
			});

			System.Console.ReadKey();
		}

		private static List<T> GetImplemenations<T>()
		{
			var iface = typeof (T);

			return AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(p => iface.IsAssignableFrom(p) && !p.IsInterface)
				.OrderByDescending(p => p.FullName)
				.Select(x => (T) Activator.CreateInstance(x))
				.ToList();
		}

		private static UInt16[,] CreateArray()
		{
			var array = new UInt16[H,W];
			return array;
		}

		private static void Measure(Action action, Func<Stopwatch, String> logmsg)
		{
			var sw = new Stopwatch();
			sw.Start();
			action?.Invoke();
			sw.Stop();

			System.Console.WriteLine(logmsg?.Invoke(sw));
		}

		private static void SaveBitmap(DirectBitmap bm, UInt16[,] bytes)
		{
			for (int i = 0; i < H; i++)
				for (int j = 0; j < W; j++)
					bm.Bits[i*W + j] = GetColor(bytes[i,j]);
		}

		private static UInt32 GetColor(UInt32 sh)
		{
			if (sh > 0xFF)
				sh = 0xFF;

			return 0xFF000000 | (sh&0xFF) << 24 >> 8 | (sh & 0xFF) << 24 >> 16 | (sh & 0xFF) << 24 >> 24;
		}
	}
}
