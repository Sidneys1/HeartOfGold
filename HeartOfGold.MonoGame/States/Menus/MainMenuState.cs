using Behaviorals;
using GFSM;
using HeartOfGold.MonoGame.Behaviors;
using HeartOfGold.MonoGame.Components;
using HeartOfGold.MonoGame.UiElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame.States.Menus {
    internal class MainMenuState : GameState {
        private readonly ContentEngine _content;
        private readonly UiComponent _ui;
        private readonly SpriteBatch _sb;

        private readonly Vector2 _mapPos = new Vector2(-558.5f, -314.5f);
        
        public MainMenuState(FiniteStateMachine<GameState> stateMachine, MainGame game) : base(stateMachine, game) {
            _sb = Game.SpriteBatch;

            _content = new ContentEngine {Game = Game};
            _content.RequestTexture("logo", "Sprites/hog_128");
            _content.RequestTexture("map", "Sprites/map_background");
            _content.RequestTexture("clouds", "Sprites/map_clouds");
            _content.RequestFont("title", "Fonts/title");
            _content.RequestFont("menu", "Fonts/menu");

            _ui = new UiComponent(Game);
        }

        public override void Enter() {
            _content.LoadContent();

            Game.Components.Add(_ui);
            this.AddFloatBehavior();

            _ui.UiElements.Add(
                new ImageElement(_ui, _content.Textures["map"]) {
                    Position = _mapPos
                }.AddFloatBehavior(0.1f)
            );

            _ui.UiElements.Add(
                new ImageElement(_ui, _content.Textures["clouds"]) {
                    Position = _mapPos
                }.AddFloatBehavior(0.25f)
            );

            _ui.UiElements.Add(
                new ImageElement(_ui, _content.Textures["logo"]) {
                    Position = new Vector2(150, 150),
                    Origin = new Vector2(64, 64)
                }.AddFloatBehavior(0.5f)
            );
            
            _ui.UiElements.Add(
                new TextElement(_ui, _content.Fonts["title"]) {
                    Position = new Vector2(250, 150 - _content.Fonts["title"].MeasureString("Heart of Gold").Y / 2f),
                    Color = Color.Gold,
                    HoverColor = Color.Gold,
                    Text = "Heart of Gold"
                }.AddFloatBehavior(0.5f)
            );

            var startButton = new TextElement(_ui, _content.Fonts["menu"]) {
                Position = new Vector2(250, 250),
                Text = "Start"
            }.AddFloatBehavior(0.75f).AddFloatPopupBehavior(1f);
            _ui.UiElements.Add(startButton);
            startButton.MouseUp += StartRoutine;

            var exitButton = new TextElement(_ui, _content.Fonts["menu"]) {
                Position = new Vector2(250, 300),
                Text = "Exit"
            }.AddFloatBehavior(0.75f).AddFloatPopupBehavior(1f);
            _ui.UiElements.Add(exitButton);
            exitButton.MouseUp += () => MetaGame.StateMachine.Transition("exit");

            base.Enter();
        }

        private void StartRoutine() {
            _content.UnloadContent();
            Game.Components.Remove(_ui);
            Game.Components.Remove(this);
            MetaGame.StateMachine.Transition("start");
        }

        public override void Update(GameTime time) {
            if (MainGame.InputMonitor.IsKeyPressed(Keys.Escape)) MetaGame.StateMachine.Transition("exit");

            base.Update(time);
        }

        public override void Draw(GameTime time) {
            MainGame.Instance.GraphicsDevice.Clear(Color.Black);
        }
    }
}