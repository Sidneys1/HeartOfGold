using System;
using HeartOfGold.MonoGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame {
    public class MainGame : Game {
        private const int FPS = 60;

        public static MainGame Instance { get; private set; }

        public GraphicsDeviceManager Graphics { get; }
        public SpriteBatch SpriteBatch { get; private set; }

        public static DebugMonitor DebugMonitor { get; private set; }
        public static InputMonitor InputMonitor { get; private set; }
        public static EventsEngine EventsEngine { get; private set; }

        public MainGame() {
            if (Instance != null) throw new SingletonException();
            
            Graphics = new GraphicsDeviceManager(this) {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            

            Content.RootDirectory = "Content";
            Window.AllowUserResizing = false;
            Window.Title = "Heart of Gold";

            IsMouseVisible = true;
            TargetElapsedTime = new TimeSpan((long)((TimeSpan.TicksPerSecond) * (1.0 / FPS)));

            Graphics.SynchronizeWithVerticalRetrace = false;

            Components.Add(ContentEngine.GlobalContent);
            ContentEngine.GlobalContent.Game = this;
            
            Instance = this;
            var cres = new Rectangle(0,0,1280, 720);
            Window.ClientSizeChanged += (sender, args) => {
                if (Window.ClientBounds.Width != cres.Width || Window.ClientBounds.Height != cres.Height) {
                    Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
                    Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
                    cres = Window.ClientBounds;
                    Graphics.ApplyChanges();
                }
            };
        }


        
        protected override void Initialize() {
            Components.Add(EventsEngine =new EventsEngine(this));
            Components.Add(InputMonitor = new InputMonitor(this));
            Components.Add(DebugMonitor = new DebugMonitor(this, 60) {Visible = Program.Debug});
            InputMonitor.KeyReleased += keys => {
                if (keys == Keys.F1) DebugMonitor.Visible = !DebugMonitor.Visible;
                //else if (keys == Keys.F11) Graphics.ToggleFullScreen();
            };
            ContentEngine.GlobalContent.LoadContent();
            
            base.Initialize();
        }
        
        protected override void LoadContent() {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();

            MetaGame.StateMachine.Transitioned += transition => {
                if (transition.To == null) Exit();
            };

            MetaGame.StateMachine.Transition("start");
        }
        
        protected override void UnloadContent() {
            ContentEngine.GlobalContent.UnloadContent();
            base.UnloadContent();
        }
    }
}
