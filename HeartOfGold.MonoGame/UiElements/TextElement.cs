using HeartOfGold.MonoGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.UiElements {


    internal class TextElement : UiComponent.UiElement {
        private readonly SpriteFont _font;
        private string _text;

        public string Text {
            get { return _text; }
            set {
                _text = value;
                Size = _font.MeasureString(_text);
            }
        }

        public TextElement(UiComponent uiComponent, SpriteFont font) : base(uiComponent) {
            _font = font;
        }
        
        public override void Draw(GameTime gameTime) {
            UiComponent.SpriteBatch.DrawString(_font, Text, Position + Offset, MouseOver ? HoverColor : Color, Rotation, Origin, Scale, Effects, 0);
        }
    }
}
