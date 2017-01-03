using System;
using System.Collections.Generic;
using System.Linq;
using HeartOfGold.MonoGame.Helpers;
using HeartOfGold.MonoGame.Helpers.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.Components {
    public class DebugMonitor : DrawableGameComponent {
        private readonly MovingAverage<Frame> _fpsAvg;
        private readonly uint _fpslen;
        private readonly SpriteBatch _spriteBatch;
        private readonly MovingAverage<double> _tpsAvg;

        public readonly List<Func<string>> DebugLines = new List<Func<string>>();

        public Color FontColor { get; set; } = Color.White;
        public bool Contrast { get; set; }

        private string _dbgstr = string.Empty;
        private SpriteFont _font;
        private bool _isTick;
        private Texture2D _solidcol;

        private long _workingSet;
        private int _workingSetUpdate=-1;

        static DebugMonitor() {
            ContentEngine.GlobalContent.RequestFont("Debug", "Fonts/FPS Font");
        }

        public DebugMonitor(Game game, uint fpsLength, uint tpsLength = 0) : base(game) {
            _fpsAvg = new MovingAverage<Frame>(fpsLength, arg => arg.FrameTime);
            _tpsAvg = new MovingAverage<double>(tpsLength == 0 ? fpsLength : tpsLength, d => d);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _fpslen = fpsLength;
            DrawOrder = int.MaxValue;
        }
        
        protected override void LoadContent() {
            _font = ContentEngine.GlobalContent.Fonts["Debug"];
            
            _solidcol = new Texture2D(GraphicsDevice, 1, 1);
            _solidcol.SetData(new[] {Color.White});
        }

        public override void Update(GameTime gameTime) {
            if (!Enabled) return;
            _tpsAvg.AddSample(gameTime.ElapsedGameTime.TotalSeconds);
            _isTick = true;
            _dbgstr = string.Join(Environment.NewLine, DebugLines.Select(o => o.Invoke()));

            var gsec = (int)gameTime.TotalGameTime.TotalSeconds;
            if (gsec > _workingSetUpdate) {
                _workingSetUpdate = gsec;
                _workingSet = Environment.WorkingSet;
            }
        }

        public void DrawStuff() {
            var opac = 255.0;
            for (uint i = 0; i < _fpsAvg.InnerArray.Length; i++) {
                var val = _fpsAvg.InnerArray.Get(i);
                var t = val.FrameTime/(1/30.0);
                var x = (int) (i*4) + 100;
                var h = (int) (10*t);
                //var y = 0;

                _spriteBatch.Draw(_solidcol, new Rectangle(x, 0, 3, h),
                    new Color(val.IsLoad ? Color.Green : Color.Red, (int) opac));
                opac -= 255.0/_fpslen;
            }
        }

        public override void Draw(GameTime gameTime) {
            if (!Enabled || !Visible) return;
            _fpsAvg.AddSample(new Frame(gameTime.ElapsedGameTime.TotalSeconds, _isTick));

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            DrawStuff();
            var str = $"{1/_fpsAvg.Average:F2}fps\n{1/_tpsAvg.Average:F2}tps\nWS:{DataHelpers.NormalizeFileSize(_workingSet)}\n{_dbgstr}";
            if (Contrast) _spriteBatch.DrawString(_font, str, Vector2.One, Color.Black);
            _spriteBatch.DrawString(_font, str, Vector2.Zero, gameTime.IsRunningSlowly ? Color.Red : FontColor);
            _spriteBatch.End();

            _isTick = false;
        }

        internal struct Frame {
            public Frame(double f, bool i) {
                FrameTime = f;
                IsLoad = i;
            }

            public double FrameTime;
            public bool IsLoad;
        }
    }
}