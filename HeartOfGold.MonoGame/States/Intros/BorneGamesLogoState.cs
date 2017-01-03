using System;
using GFSM;
using HeartOfGold.MonoGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame.States.Intros {
    internal class BorneGamesLogoState : GameState {
        private const double TIME = 5;
        private const double IN = 1.5;
        private const double IN_LEN = 1.5;
        private const double OUT = 3.5;
        private const double OUT_LEN = 1.5;

        private readonly ContentEngine _content = new ContentEngine();
        private TimeSpan _elapsed = TimeSpan.Zero;
        private readonly SpriteBatch _sb;
        private bool _escWasPressed;
        private Vector2 _pos;
        private Color _col = Color.Transparent;

        private string DebugLine() => $"Logo time: {_elapsed.TotalSeconds-TIME:F2}s";

        public BorneGamesLogoState(FiniteStateMachine<GameState> stateMachine, MainGame game) : base(stateMachine, game) {
            _content.RequestTexture("logo", "Sprites/logo");
            _sb = MainGame.Instance.SpriteBatch;
        }

        public override void Enter() {
            _content.Game = Game;
            _content.LoadContent();
            MainGame.DebugMonitor.DebugLines.Add(DebugLine);

            var tex = _content.Textures["logo"];
            _pos = new Vector2(Game.Window.ClientBounds.Width / 2f - tex.Width / 2f, Game.Window.ClientBounds.Height / 2f - tex.Height/2f);

            base.Enter();
        }
            
        public override void Update(GameTime time) {
            _elapsed += time.ElapsedGameTime;

            if (_elapsed.TotalSeconds < IN) {
                _col = Color.Lerp(Color.Transparent, Color.White, (float) (_elapsed.TotalSeconds/IN_LEN));
            } else if (_elapsed.TotalSeconds > OUT) {
                _col = Color.Lerp(Color.White, Color.Transparent, (float)((_elapsed.TotalSeconds-OUT)/OUT_LEN));
            }

            // Eventually we exit...
            if (_elapsed.TotalSeconds >= TIME || (_escWasPressed && !MainGame.InputMonitor.IsKeyPressed(Keys.Escape))) {
                MetaGame.StateMachine.Transition("next");
                _content.UnloadContent();
                MainGame.DebugMonitor.DebugLines.Remove(DebugLine);
                MetaGame.StateMachine.States.Remove(this);
                MainGame.Instance.Components.Remove(this);
            }
            _escWasPressed = MainGame.InputMonitor.IsKeyPressed(Keys.Escape);
        }

        public override void Draw(GameTime time) {
            MainGame.Instance.GraphicsDevice.Clear(Color.White);

            _sb.Begin(blendState:BlendState.AlphaBlend);

            _sb.Draw(_content.Textures["logo"], _pos, _col);

            _sb.End();
        }
    }
}