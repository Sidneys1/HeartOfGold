using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HeartOfGold.MonoGame.UiElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.Components {
    public class UiComponent : DrawableGameComponent {
        public readonly List<UiElement> UiElements = new List<UiElement>();
        public readonly SpriteBatch SpriteBatch;

        public UiComponent(Game game) : base(game) {
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime) {
            foreach (var uiElement in UiElements.Where(e => e.Enabled))
                uiElement.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            SpriteBatch.Begin(SpriteSortMode.Deferred, blendState:BlendState.AlphaBlend);

            foreach (var uiElement in UiElements.Where(e => e.Visible))
                uiElement.Draw(gameTime);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing) {
            foreach (var uiElement in UiElements)
                uiElement.Dispose();
            base.Dispose(disposing);
        }
    }
}
