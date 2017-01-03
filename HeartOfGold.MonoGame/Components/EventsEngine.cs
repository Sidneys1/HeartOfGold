using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace HeartOfGold.MonoGame.Components {
    public class EventsEngine : GameComponent {
        private readonly SortedList<double, Action> _events = new SortedList<double, Action>(5000,
            new PriorityCompararer());

        private double _lastTime;

        public EventsEngine(Game game) : base(game) {}

        public override void Initialize() {
            var dbgm = Game.Components.OfType<DebugMonitor>().FirstOrDefault();
            dbgm?.DebugLines.Add(() => $"Events: {_events.Count:N0}");
        }

        public void RegisterEvent(double delta, Action e) => _events.Add(_lastTime + delta, e);

        public override void Update(GameTime gameTime) {
            _lastTime = gameTime.TotalGameTime.TotalMilliseconds;
            if (!Enabled) return;

            while (_events.Count > 0 && _events.Keys[0] <= _lastTime) {
                _events.Values[0].Invoke();
                _events.RemoveAt(0);
            }

            base.Update(gameTime);
        }

        public class PriorityCompararer : IComparer<double> {
            public int Compare(double x, double y) => x < y ? -1 : 1;
        }
    }
}