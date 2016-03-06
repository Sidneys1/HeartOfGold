using System;
using System.Collections.Generic;
using System.Threading;
using HeartOfGold.MonoGame.Components;
using HeartOfGold.MonoGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameWindow : Game {
		const int Fps = 60;
		const int Size = 100;

		readonly GraphicsDeviceManager _graphics;
		private VertexBuffer _vb;
		private IndexBuffer _ib;
		Matrix _worldMatrix;
		Matrix _viewMatrix;
		Matrix _projectionMatrix;
		BasicEffect _basicEffect;

		Texture2D _solidcol; SpriteFont _font; SpriteBatch _sb;

		public GameWindow() {
			_graphics = new GraphicsDeviceManager(this) {
				PreferredBackBufferWidth = 1280,
				PreferredBackBufferHeight = 720
			};
			Content.RootDirectory = "Content";
			Window.AllowUserResizing = false;
			Window.Title = "Heart of Gold";

			IsMouseVisible = true;
			TargetElapsedTime = new TimeSpan((long)((TimeSpan.TicksPerSecond) * (1.0 / Fps)));

			_graphics.SynchronizeWithVerticalRetrace = false;

		}

		double loadpercent = 0;
		public void LoadStuff(object o) {
			var n = new NoiseGen();
			float[][] map = new float[Size][];

			for (int i = 0; i < Size; i++) {
				map[i] = new float[Size];
				for (int j = 0; j < Size; j++) {
					map[i][j] = n.GetNoise(i, j, 0);
					loadpercent += (33.3 / (Size * Size));
				}
			}

			var colors = new VertexPositionColor[Size * Size];
			Random r = new Random();
			for (int i = 0; i < Size; i++) {
				for (int j = 0; j < Size; j++) {
					colors[(i * Size) + j] = new VertexPositionColor(new Vector3(i - (Size / 2), j - (Size / 2), map[i][j]), new Color(r.Next(255), r.Next(255), r.Next(255)));
					loadpercent += (33.3 / (Size * Size));
				}
			}

			var indexes = new short[(Size - 1) * (Size - 1) * 6];

			var rti = new Func<int, int, short>((x, y) => (short)((x * Size) + y));
			
			for (int i = 0; i < (Size - 1); i++) {
				int y = i * (Size - 1) * 6;
				for (int j = 0; j < (Size - 1); j++) {
					int x = y + (j * 6);

					var tl = rti(i, j);
					var tr = rti(i + 1, j);
					var bl = rti(i, j + 1);
					var br = rti(i + 1, j + 1);

//					float sum = 0;
//					sum += colors[tl].Position.Z;
//					sum += colors[tr].Position.Z;
//					sum += colors[bl].Position.Z;
//					sum += colors[br].Position.Z;
//					sum /= 4f;
//					var newCol = Color.Lerp(Color.Green, Color.White, sum);
//					colors[tl].Color = colors[tr].Color = colors[bl].Color = colors[br].Color = ;
					indexes[x] = tl;
					indexes[x + 1] = tr;
					indexes[x + 2] = bl;
					indexes[x + 3] = bl;
					indexes[x + 4] = tr;
					indexes[x + 5] = br;
					loadpercent += (33.3 / ((Size - 1) * (Size - 1)));
				}
			}
			
			_vb = new VertexBuffer(GraphicsDevice, VertexPositionColor.VertexDeclaration, colors.Length, BufferUsage.None);
			_vb.SetData(colors);
			_ib = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, (Size - 1) * (Size - 1) * 6, BufferUsage.None);
			_ib.SetData(indexes);

			loadpercent = 100;
		}
		
		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			// This line gets me an FPS/TPS printout!
			Components.Add(new FpsComponent(this, Fps));

			_worldMatrix = Matrix.CreateRotationX(0);
			//			_viewMatrix = Matrix.CreateRotationZ(MathHelper.ToRadians(45)) * Matrix.CreateRotationX(MathHelper.ToRadians(60));


			_projectionMatrix = Matrix.CreateOrthographic(Size, Size, float.MaxValue, float.MinValue);

			_basicEffect = new BasicEffect(_graphics.GraphicsDevice) {
				World = _worldMatrix,
				Projection = _projectionMatrix,
				//				View = _viewMatrix,
				Alpha = 1,
				VertexColorEnabled = true,
				FogEnabled = true,
				FogStart = -100,
				FogEnd = 100,
				FogColor = Vector3.Zero
			};

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			_sb = new SpriteBatch(this.GraphicsDevice);
			_font = Content.Load<SpriteFont>("Fonts/FPS Font");
			_solidcol = new Texture2D(GraphicsDevice, 1, 1);
			_solidcol.SetData(new[] { Color.White });

			ThreadPool.QueueUserWorkItem(LoadStuff);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent() {
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();
			var sin = (float)(Math.Sin(gameTime.TotalGameTime.TotalSeconds) + 1) / 2f;
			//			_basicEffect.View = Matrix.CreateRotationX(MathHelper.ToRadians(90*sin));
			_basicEffect.View = Matrix.CreateRotationZ(MathHelper.ToRadians(45)) * Matrix.CreateRotationX(sin);

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			if (loadpercent >= 100.0) {
				var rasterizerState1 = new RasterizerState { CullMode = CullMode.None };
				_graphics.GraphicsDevice.RasterizerState = rasterizerState1;
				_graphics.GraphicsDevice.SetVertexBuffer(_vb);
				_graphics.GraphicsDevice.Indices = _ib;

				foreach (var pass in _basicEffect.CurrentTechnique.Passes) {
					pass.Apply();
					_graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, _vb.VertexCount, 0, _ib.IndexCount / 3);
				}
			} else {
				_sb.Begin();
				var s = $"Generating Terrain: {loadpercent}%";
				var size = _font.MeasureString(s);
				var x = _graphics.PreferredBackBufferWidth / 2f;
				var y = _graphics.PreferredBackBufferHeight / 2f;
				_sb.DrawString(_font, s, new Vector2((int)(x - (size.X / 2)), (int)((y - 25) - (size.Y / 2))), Color.White);
				var rec = new Rectangle((int)(x - 200), (int)(y - 10), (int)(loadpercent * 4), 20);

				_sb.Draw(_solidcol, rec, Color.White);
				_sb.End();
			}



			base.Draw(gameTime);
		}
	}
}
