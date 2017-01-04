using Behaviorals;
using HeartOfGold.MonoGame.States;
using HeartOfGold.MonoGame.UiElements;
using Microsoft.Xna.Framework;

namespace HeartOfGold.MonoGame.Behaviors {
    internal static class FloatBehavior {
        private const string FLOATINESS = "floatiness";
        private const string BACKUP_FLOATINESS = "backup_floatiness";
        private const string POPUP_FLOATINESS = "popup_floatiness";
        private const float TOTAL_FLOAT = 50f;
        private static Vector2 _offset = Vector2.Zero;

        private static readonly Behaviour<UiElement> MouseMoveBehaviour = 
            new Behaviour<UiElement>(element => element.Offset = _offset * ((float)element.AttachedProperties[FLOATINESS] * TOTAL_FLOAT));
        private static readonly Behaviour<UiElement> MouseEnterBehavior = new Behaviour<UiElement>(element => {
            element.AttachedProperties[BACKUP_FLOATINESS] = element.AttachedProperties[FLOATINESS];
            element.AttachedProperties[FLOATINESS] = element.AttachedProperties[POPUP_FLOATINESS];
        });
        private static readonly Behaviour<UiElement> MouseLeaveBehavior = 
            new Behaviour<UiElement>(element => element.AttachedProperties[FLOATINESS] = element.AttachedProperties[BACKUP_FLOATINESS]);


        public static Behaviour<GameState> UpdateBehaviour = new Behaviour<GameState>(gs => {
            _offset = MainGame.InputMonitor.MousePosition /
                      new Vector2(MainGame.Instance.Window?.ClientBounds.Width ?? 1,
                          MainGame.Instance.Window?.ClientBounds.Height ?? 1) - new Vector2(0.5f, 0.5f);
        });

        public static void AddFloatBehavior(this GameState g) => g.Behaviours.Add(GameStateBehaviorTriggers.Update.I(), UpdateBehaviour);

        public static T AddFloatBehavior<T>(this T e, float floatiness) where T : UiElement  {
            e.Behaviours.Add(UiElementBehaviorTriggers.MouseMove.I(), MouseMoveBehaviour);
            e.AttachedProperties.Add(FLOATINESS, floatiness);
            return e;
        }

        public static T AddFloatPopupBehavior<T>(this T e, float popupFloatiness) where T : UiElement {
            e.Behaviours.Add(UiElementBehaviorTriggers.MouseEnter.I(), MouseEnterBehavior);
            e.Behaviours.Add(UiElementBehaviorTriggers.MouseLeave.I(), MouseLeaveBehavior);
            e.AttachedProperties.Add(POPUP_FLOATINESS, popupFloatiness);
            return e;
        }
    }
}
