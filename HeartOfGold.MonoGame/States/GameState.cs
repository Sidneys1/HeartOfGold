using System;
using System.Collections.Generic;
using Behaviorals;
using GFSM;
using HeartOfGold.MonoGame.Behaviors;
using Microsoft.Xna.Framework;

namespace HeartOfGold.MonoGame.States {
    internal abstract class GameState : State<GameState>, IGameComponent, IDrawable, IUpdateable, IBehavioral<GameState> {
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
        public int UpdateOrder { get; } = int.MinValue;
        public int DrawOrder { get; } = int.MinValue;

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        public virtual void Initialize() {}
        public virtual void Update(GameTime time) { Trigger(GameStateBehaviorTriggers.Update.I()); }
        public abstract void Draw(GameTime gameTime);

        public MultiMap<int, Behaviour<GameState>> Behaviours { get; } = new MultiMap<int, Behaviour<GameState>>();
        public Dictionary<string, object> AttachedProperties { get; } = new Dictionary<string, object>();
        public void Trigger(int trigger) {
            if (!Behaviours.ContainsKey(trigger))
                return;
            foreach (var behaviour in Behaviours[trigger])
                behaviour.Action.Invoke(this);
        }
    }
}