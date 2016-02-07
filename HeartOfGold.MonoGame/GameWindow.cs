using System;
using HeartOfGold.MonoGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame {
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class GameWindow : Game {
		const int FPS = 120;

		readonly GraphicsDeviceManager _graphics;
		private VertexBuffer _vb;
		private IndexBuffer _ib;
		Matrix _worldMatrix;
		//Matrix _viewMatrix;
		Matrix _projectionMatrix;
		BasicEffect _basicEffect;

		public GameWindow() {
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			Window.AllowUserResizing = true;
			Window.Title = "Heart of Gold";
			IsMouseVisible = true;
			TargetElapsedTime = new TimeSpan((long)((TimeSpan.TicksPerSecond)*(1.0/FPS)));
			_graphics.SynchronizeWithVerticalRetrace = false;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize() {
			// This line gets me an FPS/TPS printout!
			Components.Add(new FpsComponent(this, FPS));
			
			float tilt = MathHelper.ToRadians(0);
			_worldMatrix = Matrix.CreateRotationX(tilt) * Matrix.CreateRotationY(tilt);
			//_viewMatrix = new Matrix(tilt);// Matrix.CreateRotationX(MathHelper.ToRadians(30)));

			_projectionMatrix = Matrix.CreateOrthographic(2,2,float.MinValue,float.MaxValue);

			_basicEffect = new BasicEffect(_graphics.GraphicsDevice) {
				World = _worldMatrix,
				//View = _viewMatrix,
				Projection = _projectionMatrix,
				//AmbientLightColor = new Vector3(0.1f, 0.1f, 0.1f),
				DiffuseColor = new Vector3(1, 1, 1),
				//SpecularColor = new Vector3(0.25f, 0.25f, 0.25f),
				//SpecularPower = 0.5f,
				Alpha = 1,
				//LightingEnabled = true
			};//_basicEffect.EnableDefaultLighting();


			//if(_basicEffect.LightingEnabled) {
			//	_basicEffect.DirectionalLight0.Enabled = true; // enable each light individually
			//	if (_basicEffect.DirectionalLight0.Enabled) {
			//		// x direction
			//		_basicEffect.DirectionalLight0.DiffuseColor = new Vector3(1, 0, 0); // range is 0 to 1
			//		_basicEffect.DirectionalLight0.Direction = Vector3.Normalize(new Vector3(-1, 0, 0));
			//		// points from the light to the origin of the scene
			//		_basicEffect.DirectionalLight0.SpecularColor = Vector3.One;
			//	}

			//	_basicEffect.DirectionalLight1.Enabled = true;
			//	if (_basicEffect.DirectionalLight1.Enabled) {
			//		// y direction
			//		_basicEffect.DirectionalLight1.DiffuseColor = new Vector3(0, 0.75f, 0);
			//		_basicEffect.DirectionalLight1.Direction = Vector3.Normalize(new Vector3(0, -1, 0));
			//		_basicEffect.DirectionalLight1.SpecularColor = Vector3.One;
			//	}

			//	_basicEffect.DirectionalLight2.Enabled = true;
			//	if (_basicEffect.DirectionalLight2.Enabled) {
			//		// z direction
			//		_basicEffect.DirectionalLight2.DiffuseColor = new Vector3(0, 0, 0.5f);
			//		_basicEffect.DirectionalLight2.Direction = Vector3.Normalize(new Vector3(0, 0, -1));
			//		_basicEffect.DirectionalLight2.SpecularColor = Vector3.One;
			//	}
			//}

			var colors = new VertexPositionNormalTexture[4];
			colors[0] = new VertexPositionNormalTexture(new Vector3(-1, 1, 0), new Vector3(1,0,0), Vector2.Zero);
			colors[1] = new VertexPositionNormalTexture(new Vector3(-1, -1, 0), new Vector3(1, 0, 0), Vector2.Zero);
			colors[2] = new VertexPositionNormalTexture(new Vector3(1, 1, 0), new Vector3(1, 0, 0), Vector2.Zero);
			colors[3] = new VertexPositionNormalTexture(new Vector3(1, -1, -1), new Vector3(1, 0, 0), Vector2.Zero);
			_vb = new VertexBuffer(GraphicsDevice, VertexPositionNormalTexture.VertexDeclaration, 4, BufferUsage.None);
			_vb.SetData(colors);

			_ib = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, 6, BufferUsage.None);
			
			_ib.SetData(new short[] {0,1,2,2,1,3});

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {

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
			_basicEffect.World = Matrix.CreateRotationX(MathHelper.ToRadians(90*sin)) * Matrix.CreateRotationY(MathHelper.ToRadians(0));

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);
			RasterizerState rasterizerState1 = new RasterizerState { CullMode = CullMode.None };
			_graphics.GraphicsDevice.RasterizerState = rasterizerState1;
			_graphics.GraphicsDevice.SetVertexBuffer(_vb);
			_graphics.GraphicsDevice.Indices = _ib;
			
			foreach (EffectPass pass in _basicEffect.CurrentTechnique.Passes) {
				pass.Apply();
				_graphics.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 4, 0, 2);
				//_graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
			}
			GraphicsDevice.Flush();
			base.Draw(gameTime);
		}
	}
}
