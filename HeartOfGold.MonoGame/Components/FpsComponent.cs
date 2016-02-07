using HeartOfGold.MonoGame.Helpers.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.Components {
	class FpsComponent : DrawableGameComponent {
		readonly MovingAverage _fpsAvg, _tpsAvg;
		SpriteFont _font;
		readonly SpriteBatch _spriteBatch;

		public FpsComponent(Game game, uint fpsLength, uint tpsLength = 0) : base(game) {
			_fpsAvg = new MovingAverage(fpsLength);
			_tpsAvg = new MovingAverage(tpsLength == 0 ? fpsLength : tpsLength);
			_spriteBatch = new SpriteBatch(game.GraphicsDevice);
		}
		protected override void LoadContent() {
			_font = Game.Content.Load<SpriteFont>("Fonts/FPS Font");
		}

		public override void Update(GameTime gameTime) {
			_tpsAvg.AddSample(gameTime.ElapsedGameTime.TotalSeconds);
		}

		public override void Draw(GameTime gameTime) {
			_fpsAvg.AddSample(gameTime.ElapsedGameTime.TotalSeconds);

			_spriteBatch.Begin();
			_spriteBatch.DrawString(_font, $"{1 / _fpsAvg.Average:F2}fps\n{1 / _tpsAvg.Average:F2}tps", Vector2.Zero, gameTime.IsRunningSlowly ? Color.Red : Color.Black);
			_spriteBatch.End();
		}
	}
}
