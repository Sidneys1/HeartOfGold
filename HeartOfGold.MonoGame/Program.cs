using System;

namespace HeartOfGold.MonoGame {
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program {
        public static bool SkipIntro { get; private set; }
        public static bool Debug { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            foreach (var arg in args) {
                switch (arg) {
                    case "-SkipIntro":
                        SkipIntro = true;
                        break;
                    case "-Debug":
                        Debug = true;
                        break;
                }
            }

            using (var game = new MainGame())
                game.Run();
        }
    }
#endif
}
