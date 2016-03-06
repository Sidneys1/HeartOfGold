using HeartOfGold.MonoGame.Helpers.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.Components {
	struct Frame {
		public Frame(double f, bool i) {
			FrameTime = f;
			IsLoad = i;
		}

		public double FrameTime;
		public bool IsLoad; 
	}

	class FpsComponent : DrawableGameComponent {
		readonly MovingAverage<Frame> _fpsAvg;
		readonly MovingAverage<double> _tpsAvg;
		SpriteFont _font;
		readonly SpriteBatch _spriteBatch;
		Texture2D _solidcol;
		bool _isTick;
		readonly uint _fpslen;

		public FpsComponent(Game game, uint fpsLength, uint tpsLength = 0) : base(game) {
			_fpsAvg = new MovingAverage<Frame>(fpsLength, arg => arg.FrameTime);
			_tpsAvg = new MovingAverage<double>(tpsLength == 0 ? fpsLength : tpsLength, d => d);
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			_fpslen = fpsLength;
		}
		protected override void LoadContent() {
			_font = Game.Content.Load<SpriteFont>("Fonts/FPS Font");
			_solidcol = new Texture2D(GraphicsDevice, 1, 1);
			_solidcol.SetData(new[] { Color.White });
		}

		public override void Update(GameTime gameTime) {
			_tpsAvg.AddSample(gameTime.ElapsedGameTime.TotalSeconds);
			_isTick = true;
		}

		public void DrawStuff() {
			double opac = 255.0;
			for (uint i = 0; i < _fpsAvg.InnerArray.Length; i++) {
				var val = _fpsAvg.InnerArray.Get(i);
				var t = val.FrameTime / (1 / 30.0);
				int x = (int) ((_fpslen*4) - (i * 4));
				int h = (int) (50 * t);
				int y = 50 - h;

				_spriteBatch.Draw(_solidcol, new Rectangle(x, y, 3, h), new Color(val.IsLoad ? Color.Green : Color.Red, (int)opac));
				opac -= 255.0 / _fpslen;
			}
		}

		public override void Draw(GameTime gameTime) {
			_fpsAvg.AddSample(new Frame(gameTime.ElapsedGameTime.TotalSeconds, _isTick));
			
			_spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			DrawStuff();
			_spriteBatch.DrawString(_font, $"{1 / _fpsAvg.Average:F2}fps\n{1 / _tpsAvg.Average:F2}tps", Vector2.Zero, gameTime.IsRunningSlowly ? Color.Red : Color.White);
			_spriteBatch.End();

			_isTick = false;
		}
	}
}
