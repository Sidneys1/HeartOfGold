using System;
using GFSM;
using Microsoft.Xna.Framework;

namespace HeartOfGold.MonoGame.States {
    internal abstract class GameState : State<GameState>, IGameComponent, IDrawable, IUpdateable {
        protected readonly MainGame Game;
        protected GameState(FiniteStateMachine<GameState> stateMachine, MainGame game) : base(stateMachine) {
            Game = game;
        }

        public override void Enter() {
            MainGame.Instance.Components.Add(this);
            var e = new EventArgs();
            DrawOrderChanged?.Invoke(this, e);
            UpdateOrderChanged?.Invoke(this, e);
        }
            
        public bool Enabled { get; } = true;
        public bool Visible { get; } = true;
        public int UpdateOrder { get; } = Int32.MinValue;
        public int DrawOrder { get; } = Int32.MinValue;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public virtual void Initialize() {}
        public abstract void Update(GameTime time);
        public abstract void Draw(GameTime gameTime);
    }
}