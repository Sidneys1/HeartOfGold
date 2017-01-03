using GFSM;
using HeartOfGold.MonoGame.States;
using HeartOfGold.MonoGame.States.Intros;
using HeartOfGold.MonoGame.States.Menus;

namespace HeartOfGold.MonoGame {
    internal static class MetaGame {
        public static readonly GameStateMachine StateMachine = new GameStateMachine();

        static MetaGame() {
            var mainMenuState = new MainMenuState(StateMachine, MainGame.Instance);
            StateMachine.States.Add(mainMenuState);

            if (Program.SkipIntro)
                StateMachine.AddTransition(new Transition<GameState>("start", null, mainMenuState));
            else {
                var borneGamesLogoState = new BorneGamesLogoState(StateMachine, MainGame.Instance);
                var titleState = new TitleState(StateMachine, MainGame.Instance);
                StateMachine.States.Add(borneGamesLogoState);
                StateMachine.States.Add(titleState);

                StateMachine.AddTransition(new Transition<GameState>("start", null, borneGamesLogoState));
                StateMachine.AddTransition(new Transition<GameState>("next", borneGamesLogoState, titleState,
                    Mode.PushPop));
                StateMachine.AddTransition(new Transition<GameState>("next", titleState, mainMenuState, Mode.PushPop));
            }

            StateMachine.AddTransition(new Transition<GameState>("exit", mainMenuState, null));
        }
    }
}
