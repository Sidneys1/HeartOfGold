using System;
using GFSM;
using HeartOfGold.MonoGame.Components;
using HeartOfGold.MonoGame.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HeartOfGold.MonoGame.States.Game {
    internal class MainGameState : GameState {
        private SpriteBatch SpriteBatch { get; }
        private readonly IsometricRendering _iso;
        private readonly ContentEngine _content;
        private readonly Random _rand = new Random();

        private const int W = 30;
        private const int H = 30;

        public MainGameState(FiniteStateMachine<GameState> stateMachine, MainGame game) : base(stateMachine, game) {
            SpriteBatch = MainGame.Instance.SpriteBatch;
            _content = new ContentEngine {Game = game};
            _iso = new IsometricRendering(W,H,new Vector2(180,103), SpriteBatch) {
                ScreenOffset =  new Vector2(-(Game.Graphics.PreferredBackBufferWidth/2f), (Game.Graphics.PreferredBackBufferHeight/2f))
            };
            _content.RequestTexture("a", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneUneven_S");
            _content.RequestTexture("b", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneTile_S");
            _content.RequestTexture("c", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneSteps_S");
            _content.RequestTexture("d", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneSideUneven_S");
            _content.RequestTexture("e", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneSide_S");
            _content.RequestTexture("f", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneRight_S");
            _content.RequestTexture("g", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneMissingTiles_S");
            _content.RequestTexture("h", "Sprites/Tiles/Dungeon/Isometric/Foundation/stoneLeft_S");
            _content.RequestTexture("i", "Sprites/Tiles/Dungeon/Isometric/Foundation/stone_S");
            _content.RequestTexture("j", "Sprites/Tiles/Dungeon/Isometric/Foundation/dirtTiles_S");
            _content.RequestTexture("k", "Sprites/Tiles/Dungeon/Isometric/Foundation/dirt_S");
            _content.RequestTexture("l", "Sprites/Tiles/Dungeon/Isometric/Floor/planksHole_S");
            _content.RequestTexture("m", "Sprites/Tiles/Dungeon/Isometric/Floor/planksBroken_S");
            _content.RequestTexture("n", "Sprites/Tiles/Dungeon/Isometric/Floor/bridgeBroken_S");
            _content.RequestTexture("o", "Sprites/Tiles/Dungeon/Isometric/Floor/bridge_S");

            _content.RequestTexture("0", "Sprites/Tiles/Dungeon/Isometric/Props/woodenPile_S");
            _content.RequestTexture("1", "Sprites/Tiles/Dungeon/Isometric/Props/woodenCrates_S");
            _content.RequestTexture("2", "Sprites/Tiles/Dungeon/Isometric/Props/woodenCrate_S");
            _content.RequestTexture("3", "Sprites/Tiles/Dungeon/Isometric/Props/tableShortChairs_S");
            _content.RequestTexture("4", "Sprites/Tiles/Dungeon/Isometric/Props/tableShort_S");
            _content.RequestTexture("5", "Sprites/Tiles/Dungeon/Isometric/Props/tableRoundChairs_S");
            _content.RequestTexture("6", "Sprites/Tiles/Dungeon/Isometric/Props/tableRound_S");
            _content.RequestTexture("7", "Sprites/Tiles/Dungeon/Isometric/Props/tableChairsBroken_S");
            _content.RequestTexture("8", "Sprites/Tiles/Dungeon/Isometric/Props/chestOpen_S");
            _content.RequestTexture("9", "Sprites/Tiles/Dungeon/Isometric/Props/chestClosed_S");
            _content.RequestTexture("10", "Sprites/Tiles/Dungeon/Isometric/Props/chair_S");
            _content.RequestTexture("11", "Sprites/Tiles/Dungeon/Isometric/Props/barrelsStacked_S");
            _content.RequestTexture("12", "Sprites/Tiles/Dungeon/Isometric/Props/barrels_S");
            _content.RequestTexture("13", "Sprites/Tiles/Dungeon/Isometric/Props/barrel_S");
        }

        public override void Enter() {
            base.Enter();
            _content.LoadContent();
            var tiles = new[] {
                _content.Textures["a"],
                _content.Textures["b"],
                _content.Textures["c"],
                _content.Textures["d"],
                _content.Textures["e"],
                _content.Textures["f"],
                _content.Textures["g"],
                _content.Textures["h"],
                _content.Textures["i"],
                _content.Textures["j"],
                _content.Textures["k"],
                _content.Textures["l"],
                _content.Textures["m"],
                _content.Textures["n"],
                _content.Textures["o"],
            };
            var props = new[] {
                _content.Textures["0"],
                _content.Textures["1"],
                _content.Textures["2"],
                _content.Textures["3"],
                _content.Textures["4"],
                _content.Textures["5"],
                _content.Textures["6"],
                _content.Textures["7"],
                _content.Textures["8"],
                _content.Textures["9"],
                _content.Textures["10"],
                _content.Textures["11"],
                _content.Textures["12"],
                _content.Textures["13"],
            };
            for (var x = 0; x <W; x++) {
                for (var y = 0; y < H; y++) {
                    var isometricTile = new IsometricRendering.IsometricTile(tiles[_rand.Next(tiles.Length)]);
                    if (_rand.Next(3) == 0)
                        isometricTile.Props.Add(props[_rand.Next(props.Length)]);
                    _iso.Tiles[x, y] = isometricTile;
                }
            }

            MainGame.DebugMonitor.Contrast = true;
            MainGame.DebugMonitor.FontColor = Color.Gray;
            MainGame.DebugMonitor.DebugLines.Add(() => $"Pos: {_iso.Position}\nZom: {_iso.Scale:F10}\nTil: {_iso.TileSize}");
        }

        private bool _mmDown;
        private Vector2 _mmLast;
        private int _scLast;
        public override void Update(GameTime time) {
            if (MainGame.InputMonitor.IsMiddleMouseDown && !_mmDown) {
                _mmLast = MainGame.InputMonitor.MousePosition;
                _mmDown = true;
            } else if (_mmDown && !MainGame.InputMonitor.IsMiddleMouseDown) {
                _mmDown = false;
            }

            if (_mmDown) {
                var mmCur = MainGame.InputMonitor.MousePosition;
                var mdiff = mmCur - _mmLast;
                _iso.Position -= mdiff;
                _mmLast = mmCur;
            }

            var scCur = MainGame.InputMonitor.ScrollWheel;
            var scDiff = (scCur - _scLast) / 120;
            if (scDiff != 0) {
                if (_iso.Scale <= 1) {
                    if (_iso.Scale <= 0.1f) {
                        if (scDiff > 0)
                            _iso.Scale *= 10;
                        else
                            _iso.Scale /= 10;
                    }else if (scDiff > 0 || _iso.Scale > 0.01f)
                        _iso.Scale += scDiff/10f;
                }
                else _iso.Scale += scDiff;
                _scLast = scCur;
            }

            base.Update(time);
        }

        public override void Draw(GameTime gameTime) {
            MainGame.Instance.GraphicsDevice.Clear(Color.Magenta);

            _iso.Render(gameTime);
        }
    }
}
