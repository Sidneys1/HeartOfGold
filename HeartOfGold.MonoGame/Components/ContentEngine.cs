using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HeartOfGold.MonoGame.Components {
    public class ContentEngine : IGameComponent {
        static ContentEngine(){GlobalContent=new ContentEngine();}

        public Game Game { get; set; }
        private readonly Dictionary<string, SpriteFont> _loadedFonts = new Dictionary<string, SpriteFont>();
        private readonly Dictionary<string, Texture2D> _loadedTextures = new Dictionary<string, Texture2D>();
        private IDictionary<string, string> _fonts = new Dictionary<string, string>(0);

        private bool _loaded;
        private IDictionary<string, string> _textures = new Dictionary<string, string>(0);

        public static readonly ContentEngine GlobalContent;

        public IReadOnlyDictionary<string, SpriteFont> Fonts => _loadedFonts;
        public IReadOnlyDictionary<string, Texture2D> Textures => _loadedTextures;

        public void Initialize() {
            if (Game == null) throw new Exception("Parent Game has not been set!");
        }

        public event Action LoadProgress;
        public double Progress { get; private set; }

        public void LoadContent() {
            double total = _fonts.Count + _textures.Count;
            var count = 0;

            foreach (var keyValuePair in _fonts) {
                _loadedFonts[keyValuePair.Key] = Game.Content.Load<SpriteFont>(keyValuePair.Value);
                count++;
                Progress = count/total;
                LoadProgress?.Invoke();
            }
            _fonts = null;
            foreach (var keyValuePair in _textures) {
                _loadedTextures[keyValuePair.Key] = Game.Content.Load<Texture2D>(keyValuePair.Value);
                count++;
                Progress = count/total;
                LoadProgress?.Invoke();
            }
            _textures = null;
            _loaded = true;
        }

        public void UnloadContent() {
            foreach (var loadedTexture in _loadedTextures)
                loadedTexture.Value.Dispose();
        }

        public void RequestTexture(IDictionary<string, string> textures) {
            foreach (var texture in textures)
                RequestTexture(texture.Key, texture.Value);
        }

        public void RequestFont(IDictionary<string, string> fonts) {
            foreach (var font in fonts)
                RequestFont(font.Key, font.Value);
        }

        public void RequestTexture(string key, string path) {
            if (!_loaded) {
                if (_textures.ContainsKey(key)) return;
                _textures.Add(key, path);
            } else {
                if (_loadedTextures.ContainsKey(key)) return;
                _loadedTextures.Add(key, Game.Content.Load<Texture2D>(path));
            }
        }

        public void RequestFont(string key, string path) {
            if (!_loaded) {
                if (_fonts.ContainsKey(key)) return;
                _fonts.Add(key, path);
            } else {
                if (_loadedFonts.ContainsKey(key)) return;
                _loadedFonts.Add(key, Game.Content.Load<SpriteFont>(path));
            }
        }
    }
}