using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame.Components {
    public class InputMonitor : GameComponent {
        private readonly Dictionary<Keys, bool> _keyStates = new Dictionary<Keys, bool>();

        public InputMonitor(Game game) : base(game) {}

        public IReadOnlyDictionary<Keys, bool> KeyStates => _keyStates;

        public bool IsLeftMouseDown { get; private set; }
        public bool IsMiddleMouseDown { get; private set; }
        public bool IsRightMouseDown { get; private set; }

        public int ScrollWheel { get; private set; }

        public Vector2 MousePosition { get; private set; }

        public event Action<Keys> KeyPressed;
        public event Action<Keys> KeyReleased;

        public event Action<MouseState> LeftMouseUp;
        public event Action<MouseState> LeftMouseDown;

        public event Action<MouseState> RightMouseUp;
        public event Action<MouseState> RightMouseDown;

        public event Action<MouseState> MiddleMouseUp;
        public event Action<MouseState> MiddleMouseDown;

        public override void Update(GameTime gameTime) {
            if (!Enabled) return;
            var keystate = Keyboard.GetState();
            var mousestate = Mouse.GetState();
            MousePosition = new Vector2(mousestate.X, mousestate.Y);
            if (IsLeftMouseDown && mousestate.LeftButton == ButtonState.Released) {
                IsLeftMouseDown = false;
                LeftMouseUp?.Invoke(mousestate);
            }
            else if (!IsLeftMouseDown && mousestate.LeftButton == ButtonState.Pressed) {
                IsLeftMouseDown = true;
                LeftMouseDown?.Invoke(mousestate);
            }

            if (IsRightMouseDown && mousestate.RightButton == ButtonState.Released) {
                IsRightMouseDown = false;
                RightMouseUp?.Invoke(mousestate);
            }
            else if (!IsRightMouseDown && mousestate.RightButton == ButtonState.Pressed) {
                IsRightMouseDown = true;
                RightMouseDown?.Invoke(mousestate);
            }

            if (IsMiddleMouseDown && mousestate.MiddleButton == ButtonState.Released) {
                IsMiddleMouseDown = false;
                MiddleMouseUp?.Invoke(mousestate);
            }
            else if (!IsMiddleMouseDown && mousestate.MiddleButton == ButtonState.Pressed) {
                IsMiddleMouseDown = true;
                MiddleMouseDown?.Invoke(mousestate);
            }

            ScrollWheel = mousestate.ScrollWheelValue;

            var releasedKeys = _keyStates.Where(k => k.Value && keystate.IsKeyUp(k.Key)).Select(k => k.Key).ToArray();
            foreach (var releasedKey in releasedKeys) {
                _keyStates[releasedKey] = false;
                KeyReleased?.Invoke(releasedKey);
            }

            foreach (var pressedKey in keystate.GetPressedKeys()) {
                if (_keyStates.ContainsKey(pressedKey)) {
                    if (_keyStates[pressedKey]) continue;
                    _keyStates[pressedKey] = true;
                    KeyPressed?.Invoke(pressedKey);
                }
                else _keyStates.Add(pressedKey, true);
            }
        }

        public bool IsKeyPressed(Keys key) {
            bool ret;
            return KeyStates.TryGetValue(key, out ret) && ret;
        }
    }
}