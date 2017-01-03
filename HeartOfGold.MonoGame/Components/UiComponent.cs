using System;
using System.Collections.Generic;
using System.Linq;
using Behaviorals;
using HeartOfGold.MonoGame.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.Components {
    public class UiComponent : DrawableGameComponent {
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
            public bool MouseOver { get; private set; }
            
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
                var pre = MouseOver;
                var cpos = ComputedPosition;
                var nmp = MainGame.InputMonitor.MousePosition;
                MouseOver = nmp.X >= cpos.X
                            && nmp.X < cpos.X + Size.X
                            && nmp.Y >= cpos.Y
                            && nmp.Y < (cpos.Y + Size.Y);
                if (pre && !MouseOver) {
                    MouseLeave?.Invoke();
                    Trigger(UiElementBehaviorTriggers.MouseLeave.I());
                }
                if (!pre && MouseOver) {
                    MouseEnter?.Invoke();
                    Trigger(UiElementBehaviorTriggers.MouseEnter.I());
                }
                if (nmp != lastMousePos) {
                    MouseMove?.Invoke();
                    Trigger(UiElementBehaviorTriggers.MouseMove.I());
                }
            }

            public bool Enabled { get; set; } = true;
            public int UpdateOrder { get; protected set; }
            public event EventHandler<EventArgs> EnabledChanged;
            public event EventHandler<EventArgs> UpdateOrderChanged;
            public virtual void Dispose() { }

            public event Action MouseEnter;
            public event Action MouseLeave;
            public event Action MouseMove;

            public MultiMap<int, Behaviour<UiElement>> Behaviours { get; } = new MultiMap<int, Behaviour<UiElement>>();
            public Dictionary<string, object> AttachedProperties { get; } = new Dictionary<string, object>();

            public virtual void Trigger(int trigger) {
                if (!Behaviours.ContainsKey(trigger)) return;
                foreach (var behaviour in Behaviours[trigger])
                    behaviour.Action.Invoke(this);
            }
        }

        public readonly List<UiElement> UiElements = new List<UiElement>();
        public readonly SpriteBatch SpriteBatch;

        public UiComponent(Game game) : base(game) {
            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }

        public override void Update(GameTime gameTime) {
            foreach (var uiElement in UiElements.Where(e => e.Enabled))
                uiElement.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime) {
            SpriteBatch.Begin(SpriteSortMode.Deferred, blendState:BlendState.AlphaBlend);

            foreach (var uiElement in UiElements.Where(e => e.Visible))
                uiElement.Draw(gameTime);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing) {
            foreach (var uiElement in UiElements)
                uiElement.Dispose();
            base.Dispose(disposing);
        }
    }
}
