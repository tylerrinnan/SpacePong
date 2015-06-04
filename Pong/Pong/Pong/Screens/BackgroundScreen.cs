using System;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Screens
{
	/// <summary>
	/// The background screen sits behind all the other menu screens.
	/// It draws a background image that remains fixed in place regardless
	/// of whatever transitions the screens on top of it may be doing.
	/// </summary>
	public class BackgroundScreen : GameScreen
	{
		private ContentManager _content;
		private Texture2D _backgroundTexture;
		private SoundEffect _backgroundMusicFX;

		public BackgroundScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
		}

		public override void Activate(bool instancePreserved)
		{
			if (!instancePreserved)
			{
				if (_content == null)
				{
					_content = new ContentManager(ScreenManager.Game.Services, "Content");
				}
				_backgroundTexture = _content.Load<Texture2D>(@"UI\Background\PongBackground");
				_backgroundMusicFX = _content.Load<SoundEffect>(@"UI\Menu\Sounds\BackGroundMenu");

				var backgroundMusic = _backgroundMusicFX.CreateInstance();
				backgroundMusic.IsLooped = true;
				backgroundMusic.Play();
			}
		}

		public override void Unload()
		{
			_content.Unload();
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);
		}

		public override void Draw(GameTime gameTime)
		{
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
			Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

			spriteBatch.Begin();
			spriteBatch.Draw(_backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
			spriteBatch.End();
		}
	}
}
