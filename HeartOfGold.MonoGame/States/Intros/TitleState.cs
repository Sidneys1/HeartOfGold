using System;
using GFSM;
using HeartOfGold.MonoGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame.States.Intros {
    internal class TitleState : GameState {
        private const double TIME = 5;
        private const double IN = 1.5;
        private const double IN_LEN = 1.5;
        private const double OUT = 3.5;
        private const double OUT_LEN = 1.5;

        private readonly ContentEngine _content = new ContentEngine();
        private TimeSpan _elapsed = TimeSpan.Zero;
        private readonly SpriteBatch _sb;
        private bool _escWasPressed;
        private Vector2 _pos,_strPos;
        private Color _col = Color.Transparent;
        private Color _strCol = Color.Transparent;
        private Color _bgCol = Color.White;

        private string DebugLine() => $"Title time: {_elapsed.TotalSeconds - TIME:F2}s";

        public TitleState(FiniteStateMachine<GameState> stateMachine, MainGame game) : base(stateMachine, game) {
            _content.Game = Game;
            _content.RequestTexture("hog", "Sprites/hog");
            _content.RequestFont("title", "Fonts/title");
            _sb = MainGame.Instance.SpriteBatch;
        }

        public override void Enter() {
            _content.LoadContent();
            MainGame.DebugMonitor.DebugLines.Add(DebugLine);

            var tex = _content.Textures["hog"];
            _pos = new Vector2(Game.Window.ClientBounds.Width / 2f - tex.Width / 2f, Game.Window.ClientBounds.Height / 2f - (tex.Height + 20));

            var size = _content.Fonts["title"].MeasureString("Heart of Gold");
            _strPos = new Vector2(Game.Window.ClientBounds.Width / 2f - size.X / 2f, Game.Window.ClientBounds.Height / 2f);

            base.Enter();
        }

        public override void Update(GameTime time) {
            _elapsed += time.ElapsedGameTime;

            if (_elapsed.TotalSeconds < IN) {
                _col = Color.Lerp(Color.Transparent, Color.White, (float)(_elapsed.TotalSeconds / IN_LEN));
                _strCol = Color.Lerp(Color.Transparent, Color.Black, (float)(_elapsed.TotalSeconds / IN_LEN));
            } else if (_elapsed.TotalSeconds > OUT) {
                _col = Color.Lerp(Color.White, Color.Transparent, (float)((_elapsed.TotalSeconds - OUT) / OUT_LEN));
                _bgCol = Color.Lerp(Color.White, Color.Black, (float)((_elapsed.TotalSeconds - OUT) / OUT_LEN));
            }

            // Eventually we exit...
            if (_elapsed.TotalSeconds >= TIME || (_escWasPressed && !MainGame.InputMonitor.IsKeyPressed(Keys.Escape))) {
                MetaGame.StateMachine.Transition("next");
                _content.UnloadContent();
                MainGame.DebugMonitor.DebugLines.Remove(DebugLine);
                MainGame.DebugMonitor.FontColor = Color.White;
                MetaGame.StateMachine.States.Remove(this);
                MainGame.Instance.Components.Remove(this);
            }
            _escWasPressed = MainGame.InputMonitor.IsKeyPressed(Keys.Escape);
        }

        public override void Draw(GameTime time) {
            MainGame.Instance.GraphicsDevice.Clear(_bgCol);

            _sb.Begin(blendState: BlendState.AlphaBlend);

            _sb.Draw(_content.Textures["hog"], _pos, _col);
            _sb.DrawString(_content.Fonts["title"], "Heart of Gold", _strPos, _strCol);

            _sb.End();
        }
    }
}