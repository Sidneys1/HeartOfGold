using HeartOfGold.MonoGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.UiElements {
    internal class ImageElement : UiComponent.UiElement {
        private readonly Texture2D _texture;
        //public new Vector2 Scale { get; set; }
        public string Text { get; set; }

        public ImageElement(UiComponent uiComponent, Texture2D texture) : base(uiComponent) {
            _texture = texture;
        }
        
        public override void Draw(GameTime gameTime) {
            UiComponent.SpriteBatch.Draw(_texture, Position + Offset, null, Color, Rotation, Origin, Scale, Effects, 0);
        }
    }
}
