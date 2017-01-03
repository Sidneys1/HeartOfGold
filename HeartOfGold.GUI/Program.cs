using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using VirusFactory.OpenTK.GameHelpers.VBOHelper;
using GL = OpenTK.Graphics.OpenGL.GL;
// ReSharper disable CompareOfFloatsByEqualityOperator

// ReSharper disable AccessToDisposedClosure

namespace HeartOfGold.GUI {
	class Program {
		[STAThread]
		static void Main() {
			Console.WriteLine("Entering");

			using (var game = new GameWindow(1920, 1080, new GraphicsMode(ColorFormat.Empty, 8, 0, 6), "Heart of Gold", GameWindowFlags.Default)) {
				
				game.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
				var size = 500;
				var zoom = 1.0;
				game.Load += (sender, e) => { game.VSync = VSyncMode.On;
					GL.PointSize(10);
					
				};
				game.MouseWheel += delegate (object sender, MouseWheelEventArgs args) {
					if (args.Delta > 0)
						zoom *= 1.1;
					//zoom += args.Delta/10.0;
					else
						zoom /= 1.1;
					//zoom += args.Delta/10.0;

					if (zoom <= 0.1) zoom = 0.1;

					//var aspect = game.Width / (double)game.Height;

					//GL.Viewport(0, 0, game.Width, game.Height);
					//GL.Ortho(-xsize, xsize, ysize, -ysize, 0, 4.0);
				};
				game.Resize += (sender, e) =>
					GL.Viewport(0, 0, game.Width, game.Height);

				#region Heights

				var matrix = new double[size, size];
				//var r = new Random();

				#endregion


				#region Color

				Console.WriteLine("Initializing color...");
				var cmatrix = new Color[size, size];
				using (var b = (Bitmap)Image.FromFile(@"E:\Downloads\Untitled5.png")) {
					for (var i = 0; i < size; i++) {
						Console.Write($"\rOn column {i,4} of {size}");
						for (var j = 0; j < size; j++) {
							matrix[i, j] = ((double)b.GetPixel(i, j).A + 1);// / 8.0;
							cmatrix[i, j] = Color.FromArgb(255, b.GetPixel(i, j));
						}
					}
				}
				Console.WriteLine();
				#endregion


				bool wDown = false,
					 aDown = false,
					 sDown = false,
					 dDown = false;
				game.KeyDown += (sender, args) => {
				    switch (args.Key) {
				        case Key.W:
				            wDown = true;
				            break;
				        case Key.A:
				            aDown = true;
				            break;
				        case Key.S:
				            sDown = true;
				            break;
				        case Key.D:
				            dDown = true;
				            break;
				    }
				};
				var translate = Vector2.Zero;
				game.KeyUp += (sender, args) => {
				    switch (args.Key) {
				        case Key.Escape:
				            game.Exit();
				            break;
				        case Key.R:
				            zoom = 1;
				            break;
				        case Key.W:
				            if (wDown)
				                translate += new Vector2(1f, 1f);
				            wDown = false;
				            break;
				        case Key.A:
				            if (aDown)
				                translate += new Vector2(1f, -1f);
				            aDown = false;
				            break;
				        case Key.S:
				            if (sDown)
				                translate += new Vector2(-1f, -1f);
				            sDown = false;
				            break;
				        case Key.D:
				            if (dDown)
				                translate += new Vector2(-1f, 1f);
				            dDown = false;
				            break;
				    }
				};

				double time=0, sin = 0;

				game.UpdateFrame +=
				    (sender, e) => {
				        time += e.Time;
				        sin = (Math.Sin(time/4) + 1)/2;
				        game.Title = $"Heart of Gold - {game.RenderFrequency:000.00}fps - {game.UpdateFrequency:000.00}tps";
				    };

				var items = new List<BufferElement>(size * size * 12);
				Console.WriteLine("Prepping buffer elements...");
				//First half
				var side = matrix.GetLength(0);
				var half = side / 2;
				for (var i = 0; i < side; i++) {
					Console.Write($"\rCreating row {i,4} of {side}");
					int x = i, y = 0;
					while (x >= 0) {
						var hasleft = y != side - 1;
						var hasright = x != side - 1;
						var left = hasleft ? (double?)matrix[x, y + 1] : -2;
						var right = hasright ? (double?)matrix[x + 1, y] : -2;
						items.AddRange(AddOne(matrix[x, y], x - half, y - half, cmatrix[x--, y++], left, right));
					}
				}
				// Pt 2
				for (var i = 1; i <= side - 1; i++) {
					Console.Write($"\rCreating column {i,4} of {side}");
					int x = side - 1, y = i;
					while (y < side) {
						var hasleft = y != side - 1; var hasright = x != side - 1;
						var left = hasleft ? (double?)matrix[x, y + 1] : -2;
						var right = hasright ? (double?)matrix[x + 1, y] : -2;
						items.AddRange(AddOne(matrix[x, y], x - half, y - half, cmatrix[x--, y++], left, right));
					}
				}
				Console.WriteLine("\rCreating vertex buffer object...           ");
				Action a = delegate {
					GL.EnableClientState(ArrayCap.VertexArray);
					GL.EnableClientState(ArrayCap.ColorArray);
					GL.VertexPointer(3, VertexPointerType.Float, BufferElement.SizeInBytes, new IntPtr(0));
					GL.ColorPointer(3, ColorPointerType.Float, BufferElement.SizeInBytes, new IntPtr(Vector3.SizeInBytes));
				};
				var terrain = new VertexBuffer<BufferElement>(items.ToArray(), a, BufferUsageHint.StaticDraw);
				Console.WriteLine("Done!");
				game.RenderFrame += delegate {
					// render graphics
					GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
					GL.MatrixMode(MatrixMode.Projection);
					GL.LoadIdentity();

					GL.Ortho(size, -size, size, -size, -double.MaxValue, double.MaxValue);

					GL.Scale(1*zoom, (game.Width/(double) game.Height)*zoom, 1*zoom);
					GL.Rotate(90*sin, 1, 0, 0);
					GL.Rotate(45, 0, 0, 1);
					GL.Translate(translate.X, translate.Y, -matrix[(int) -translate.X + half, (int) -translate.Y + half]);

					terrain.Render(PrimitiveType.Quads);

					//GL.Begin(PrimitiveType.LineLoop);

					//DrawOne(matrix[(int) -translate.X + half, (int) -translate.Y + half], (int) -translate.X, (int) -translate.Y,
					//	Color.Black);
					//GL.End();

					game.SwapBuffers();
				};
				game.WindowState = WindowState.Maximized;
				game.Run(60.0, 60);
			}
		}

		static void DrawOne(double height, int x, int y, Color col, double? leftHeight = null, double? rightHeight = null) {
			if (leftHeight.HasValue && leftHeight.Value < height)
				DrawLeft(height, x, y, col, leftHeight.Value);

			if (rightHeight.HasValue && rightHeight.Value < height)
				DrawRight(height, x, y, col, rightHeight.Value);

			DrawTop(height, x, y, col);
		}

		static IEnumerable<BufferElement> AddOne(double height, int x, int y, Color col, double? leftHeight = null, double? rightHeight = null) {
			if (leftHeight.HasValue && leftHeight.Value < height)
				foreach (var e in AddLeft((float)height, x, y, col, (float)leftHeight.Value))
					yield return e;

			if (rightHeight.HasValue && rightHeight.Value < height)
				foreach (var e in AddRight((float)height, x, y, col, (float)rightHeight.Value))
					yield return e;

			foreach (var e in AddTop((float)height, x, y, col))
				yield return e;
		}

		static void DrawTop(double height, int x, int y, Color col) {
			//var h = -height * 0.25;
			//double dx = 0, dy = 0;
			//dx += (x / 2.0) + (y / -2.0);
			//dy += (y / 4.0) + (x / 4.0);
			GL.Color3(col);
			//GL.Vertex3(dx, -0.25 + h + dy, 0);
			//GL.Vertex3(0.5 + dx, dy + h, 0);
			//GL.Vertex3(dx, 0.25 + h + dy, 1);
			//GL.Vertex3(-0.5 + dx, dy + h, 1);

			GL.Vertex3(x + 0.5, y + 0.5, height);
			GL.Vertex3(x + 0.5, y - 0.5, height);
			GL.Vertex3(x - 0.5, y - 0.5, height);
			GL.Vertex3(x - 0.5, y + 0.5, height);
		}

		static IEnumerable<BufferElement> AddTop(float height, int x, int y, Color col) {
			var c = new Vector3(col.R / 255f, col.G / 255f, col.B / 255f);
			yield return new BufferElement(new Vector3(x + 0.5f, y + 0.5f, height), c);
			yield return new BufferElement(new Vector3(x + 0.5f, y - 0.5f, height), c);
			yield return new BufferElement(new Vector3(x - 0.5f, y - 0.5f, height), c);
			yield return new BufferElement(new Vector3(x - 0.5f, y + 0.5f, height), c);
		}

		static void DrawLeft(double height, int x, int y, Color col, double sideheight) {
			var c2 = Color.FromArgb(col.A,
				(int)(col.R * 0.8), (int)(col.G * 0.8), (int)(col.B * 0.8));
			var c3 = Color.FromArgb(col.A,
				(int)(col.R * 0.9), (int)(col.G * 0.9), (int)(col.B * 0.9));

			GL.Color3(c2);
			GL.Vertex3(x + 0.5, y + 0.5, height);
			GL.Vertex3(x - 0.5, y + 0.5, height);

			GL.Color3(c3);
			GL.Vertex3(x - 0.5, y + 0.5, sideheight);
			GL.Vertex3(x + 0.5, y + 0.5, sideheight);
		}

		static IEnumerable<BufferElement> AddLeft(float height, int x, int y, Color col, float sideheight) {
			var c = new Vector3((col.R / 255f) * 0.8f, (col.G / 255f) * 0.8f, (col.B / 255f) * 0.8f);
			yield return new BufferElement(new Vector3(x + 0.5f, y + 0.5f, height), c);
			yield return new BufferElement(new Vector3(x - 0.5f, y + 0.5f, height), c);
			c = new Vector3((col.R / 255f) * 0.8f, (col.G / 255f) * 0.8f, (col.B / 255f) * 0.8f);
			yield return new BufferElement(new Vector3(x - 0.5f, y + 0.5f, sideheight), c);
			yield return new BufferElement(new Vector3(x + 0.5f, y + 0.5f, sideheight), c);
		}

		static void DrawRight(double height, int x, int y, Color col, double sideheight) {
			var c2 = Color.FromArgb(col.A,
				(int)(col.R * 0.8), (int)(col.G * 0.8), (int)(col.B * 0.8));
			var c3 = Color.FromArgb(col.A,
				(int)(col.R * 0.9), (int)(col.G * 0.9), (int)(col.B * 0.9));

			GL.Color3(c2);
			GL.Vertex3(x + 0.5, y + 0.5, height);
			GL.Vertex3(x + 0.5, y - 0.5, height);

			GL.Color3(c3);
			GL.Vertex3(x + 0.5, y - 0.5, sideheight);
			GL.Vertex3(x + 0.5, y + 0.5, sideheight);
		}

		static IEnumerable<BufferElement> AddRight(float height, int x, int y, Color col, float sideheight) {
			var c = new Vector3((col.R / 255f) * 0.8f, (col.G / 255f) * 0.8f, (col.B / 255f) * 0.8f);
			yield return new BufferElement(new Vector3(x + 0.5f, y + 0.5f, height), c);
			yield return new BufferElement(new Vector3(x + 0.5f, y - 0.5f, height), c);
			c = new Vector3((col.R / 255f) * 0.8f, (col.G / 255f) * 0.8f, (col.B / 255f) * 0.8f);
			yield return new BufferElement(new Vector3(x + 0.5f, y - 0.5f, sideheight), c);
			yield return new BufferElement(new Vector3(x + 0.5f, y + 0.5f, sideheight), c);
		}
	}
}
