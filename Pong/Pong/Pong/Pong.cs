using System;
using System.Collections.Generic;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Pong.Screens;
//using Pong.UI.Menu;

namespace Pong
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Pong : Microsoft.Xna.Framework.Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		private ScreenManager _screenManager;
		private ScreenFactory _screenFactory;
		//private BaseMenu _baseMenu;

		public Pong()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			TargetElapsedTime = TimeSpan.FromTicks(333333);

			_screenFactory = new ScreenFactory();
			Services.AddService(typeof(IScreenFactory), _screenFactory);
			_screenManager = new ScreenManager(this);
			Components.Add(_screenManager);

			_graphics.PreferredBackBufferHeight = 768;
			_graphics.PreferredBackBufferWidth = 1024;
			_graphics.IsFullScreen = false;

			AddInitialScreens();
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			_graphics.GraphicsDevice.Clear(Color.Black);

			// The real drawing happens inside the screen manager component.
			base.Draw(gameTime);
		}

		private void AddInitialScreens()
		{
			// Activate the first screens.
			_screenManager.AddScreen(new BackgroundScreen(), null);
			_screenManager.AddScreen(new MainMenuScreen(), null);
		}
	}
}
