using System;
using System.Collections.Generic;
using Behaviorals;
using HeartOfGold.MonoGame.Behaviors;
using HeartOfGold.MonoGame.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.UiElements {
    public abstract class UiElement : IBehavioral<UiElement>, IDrawable, IUpdateable, IDisposable {
        protected readonly UiComponent UiComponent;

        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Offset { get; set; } = Vector2.Zero;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 ComputedPosition => Position + Offset;
        public Vector2 Size { get; protected set; }
        public float Scale { get; set; } = 1f;
        public float Rotation { get; set; } = 0f;
        public SpriteEffects Effects { get; set; }
        public Color Color { get; set; } = Color.White;

        public Color HoverColor { get; set; } = Color.Gold;
        public bool IsMouseOver { get; private set; }
        public bool IsMouseDown { get; private set; }

        protected UiElement(UiComponent uiComponent) {
            UiComponent = uiComponent;
        }

        public abstract void Draw(GameTime gameTime);
        public int DrawOrder { get; protected set; }
        public bool Visible { get; set; } = true;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        private Vector2 lastMousePos = Vector2.Zero;
        public virtual void Update(GameTime gameTime) {
            var pre = IsMouseOver;
            var cpos = ComputedPosition;
            var nmp = MainGame.InputMonitor.MousePosition;
            IsMouseOver = nmp.X >= cpos.X
                        && nmp.X < cpos.X + Size.X
                        && nmp.Y >= cpos.Y
                        && nmp.Y < (cpos.Y + Size.Y);
            if (pre && !IsMouseOver) {
                MouseLeave?.Invoke();
                Trigger(UiElementBehaviorTriggers.MouseLeave.I());
            }
            if (!pre && IsMouseOver) {
                MouseEnter?.Invoke();
                Trigger(UiElementBehaviorTriggers.MouseEnter.I());
            }
            if (nmp != lastMousePos) {
                MouseMove?.Invoke();
                Trigger(UiElementBehaviorTriggers.MouseMove.I());
            }

            var leftMouseDown = MainGame.InputMonitor.IsLeftMouseDown;
            if (IsMouseOver && leftMouseDown && !IsMouseDown) {
                IsMouseDown = true;
                MouseDown?.Invoke();
                Trigger(UiElementBehaviorTriggers.MouseDown.I());
            }
            if (IsMouseOver && !leftMouseDown && IsMouseDown) {
                IsMouseDown = false;
                MouseUp?.Invoke();
                Trigger(UiElementBehaviorTriggers.MouseUp.I());
            }

            lastMousePos = nmp;
        }

        public bool Enabled { get; set; } = true;
        public int UpdateOrder { get; protected set; }
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public virtual void Dispose() { }

        public event Action MouseEnter;
        public event Action MouseLeave;
        public event Action MouseMove;
        public event Action MouseDown;
        public event Action MouseUp;

        public MultiMap<int, Behaviour<UiElement>> Behaviours { get; } = new MultiMap<int, Behaviour<UiElement>>();
        public Dictionary<string, object> AttachedProperties { get; } = new Dictionary<string, object>();

        public virtual void Trigger(int trigger) {
            if (!Behaviours.ContainsKey(trigger)) return;
            foreach (var behaviour in Behaviours[trigger])
                behaviour.Action.Invoke(this);
        }
    }
}