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
            var floatBehavior = new Behaviour<UiComponent.UiElement>(element => element.Offset = _offset*((float)element.AttachedProperties["floatiness"]*50));

            _content.LoadContent();

            Game.Components.Add(_ui);

            var logo = new ImageElement(_ui, _content.Textures["logo"]) {
                Position = new Vector2(150, 150),
                Origin = new Vector2(64,64)
            };
            _ui.UiElements.Add(logo);
            logo.AttachedProperties.Add("floatiness", 0.5f);
            logo.Behaviours.Add(UiElementBehaviorTriggers.MouseMove.I(), floatBehavior);

            var titleText = new TextElement(_ui, _content.Fonts["title"]) {
                Position = new Vector2(250, 150 - _content.Fonts["title"].MeasureString("Heart of Gold").Y / 2f),
                Color = Color.Gold,
                HoverColor = Color.Gold,
                Text = "Heart of Gold"
            };
            _ui.UiElements.Add(titleText);
            titleText.AttachedProperties.Add("floatiness", 0.5f);
            titleText.Behaviours.Add(UiElementBehaviorTriggers.MouseMove.I(), floatBehavior);

            var startText = new TextElement(_ui, _content.Fonts["menu"]) {
                Position = new Vector2(250, 250),
                Text = "Start"
            };
            _ui.UiElements.Add(startText);
            startText.AttachedProperties.Add("floatiness", 0.75f);
            startText.Behaviours.Add(UiElementBehaviorTriggers.MouseMove.I(), floatBehavior);

            startText.MouseEnter += () => startText.AttachedProperties["floatiness"] = 1f;
            startText.MouseLeave += () => startText.AttachedProperties["floatiness"] = 0.75f;
            base.Enter();
        }

        private Vector2 _offset;

        public override void Update(GameTime time) {
            if (MainGame.InputMonitor.IsKeyPressed(Keys.Escape)) MetaGame.StateMachine.Transition("exit");

            _offset = (MainGame.InputMonitor.MousePosition / new Vector2(MainGame.Instance.Window?.ClientBounds.Width??1, MainGame.Instance.Window?.ClientBounds.Height??1)) - new Vector2(0.5f, 0.5f);
        }

        public override void Draw(GameTime time) {
            MainGame.Instance.GraphicsDevice.Clear(Color.Black);

            _sb.Begin();
            {
                _sb.Draw(_content.Textures["map"], _mapPos + _offset * 10, Color.White);
                _sb.Draw(_content.Textures["clouds"], _mapPos + _offset * 20, Color.White);
            }
            _sb.End();
        }
    }
}