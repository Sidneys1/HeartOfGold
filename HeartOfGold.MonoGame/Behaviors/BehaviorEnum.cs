namespace HeartOfGold.MonoGame.Behaviors {
    internal enum UiElementBehaviorTriggers : int {
        MouseMove,
        MouseEnter,
        MouseLeave
    }

    internal static class EnumExtension {
        public static int I(this UiElementBehaviorTriggers trigger) => (int) trigger;
    }
}
