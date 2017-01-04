namespace HeartOfGold.MonoGame.Behaviors {
    internal enum UiElementBehaviorTriggers : int {
        MouseMove,
        MouseEnter,
        MouseLeave,
        MouseDown,
        MouseUp
    }

    internal enum GameStateBehaviorTriggers : int {
        Update
    }

    internal static class EnumExtension {
        public static int I(this UiElementBehaviorTriggers trigger) => (int) trigger;
        public static int I(this GameStateBehaviorTriggers trigger) => (int)trigger;
    }
}
