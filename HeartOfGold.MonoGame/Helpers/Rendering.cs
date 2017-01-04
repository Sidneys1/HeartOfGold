using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.Helpers {
    internal class IsometricRendering {
        #region Classes

        public class IsometricTile {
            public IsometricTile(Texture2D texture) {
                Texture = texture;
            }
            public readonly Texture2D Texture;
            public readonly List<Texture2D> Props = new List<Texture2D>();
        }

        #endregion

        public IsometricTile[,] Tiles { get; }
        public Vector2 TileSize { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 ScreenOffset { get; set; }
        public Vector2 Half { get; }
        public float Scale { get; set; } = 2;
        public Matrix RotationMatrix = Matrix.CreateRotationZ(0.7854f);
        public SpriteBatch SpriteBatch { get; }

        public IsometricRendering(int width, int height, Vector2 tileSize, SpriteBatch spriteBatch) {
            TileSize = tileSize;
            SpriteBatch = spriteBatch;
            Tiles = new IsometricTile[width,height];
            Half = new Vector2(width/2f, height/2f);
            Position = Half*TileSize;
        }

        public void Render(GameTime time) {
            var offset = Matrix.CreateTranslation(new Vector3(-(Position + ScreenOffset), 0f)) + Matrix.CreateScale(Scale);
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, transformMatrix: offset);
            {
                var height = Tiles.GetLength(1);
                var width = Tiles.GetLength(0);
                for (var slice = 0; slice < height + width - 1; ++slice) {
                    var z1 = slice < width ? 0 : slice - width + 1;
                    var z2 = slice < height ? 0 : slice - height + 1;
                    for (var j = slice - z2; j >= z1; --j) {
                        var x = Tiles[j,slice - j];
                        var pos = new Vector2(j, slice - j);
                        pos -= Half;
                        pos = Vector2.Transform(pos, RotationMatrix);
                        pos += Half;
                        pos *= TileSize;
                        SpriteBatch.Draw(x.Texture, pos, null, Color.White, 0f, TileSize/2, 1f, SpriteEffects.None, 0);
                        foreach (var texture2D in x.Props)
                            SpriteBatch.Draw(texture2D, pos, null, Color.White, 0f, TileSize/2, 1f, SpriteEffects.None, 0);
                    }
                }
            }
            SpriteBatch.End();
        }
    }
}
