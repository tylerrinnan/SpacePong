﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameStateManagement;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Pong.Screens
{
	/// <summary>
	/// Base class for screens that contain a menu of options. The user can
	/// move up and down to select an entry, or cancel to back out of the screen.
	/// </summary>
	public abstract class MenuScreen : GameScreen
	{
		private List<MenuEntry> _menuEntries = new List<MenuEntry>();
		private int _selectedEntry = 0;
		private string _menuTitle;

		private InputAction _menuUp;
		private InputAction _menuDown;
		private InputAction _menuSelect;
		private InputAction _menuCancel;
		private ContentManager _content;
		private SoundEffect _menuNavigateFX;
		private SoundEffect _menuSelectFX;

		/// <summary>
		/// Gets the list of menu entries, so derived classes can add
		/// or change the menu contents.
		/// </summary>
		protected IList<MenuEntry> MenuEntries
		{
			get { return _menuEntries; }
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public MenuScreen(string menuTitle)
		{
			this._menuTitle = menuTitle;

			TransitionOnTime = TimeSpan.FromSeconds(0.5);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);

			_menuUp = new InputAction(
				new Buttons[] { Buttons.DPadUp, Buttons.LeftThumbstickUp },
				new Keys[] { Keys.Up },
				true);
			_menuDown = new InputAction(
				new Buttons[] { Buttons.DPadDown, Buttons.LeftThumbstickDown },
				new Keys[] { Keys.Down },
				true);
			_menuSelect = new InputAction(
				new Buttons[] { Buttons.A, Buttons.Start },
				new Keys[] { Keys.Enter, Keys.Space },
				true);
			_menuCancel = new InputAction(
				new Buttons[] { Buttons.B, Buttons.Back },
				new Keys[] { Keys.Escape },
				true);
		}

		/// <summary>
		/// Responds to user input, changing the selected entry and accepting
		/// or cancelling the menu.
		/// </summary>
		public override void HandleInput(GameTime gameTime, InputState input)
		{
			// For input tests we pass in our ControllingPlayer, which may
			// either be null (to accept input from any player) or a specific index.
			// If we pass a null controlling player, the InputState helper returns to
			// us which player actually provided the input. We pass that through to
			// OnSelectEntry and OnCancel, so they can tell which player triggered them.
			PlayerIndex playerIndex;

			bool playNavigationSound = false;
			// Move to the previous menu entry?
			if (_menuUp.Evaluate(input, ControllingPlayer, out playerIndex))
			{
				playNavigationSound = true;
				_selectedEntry--;

				if (_selectedEntry < 0)
					_selectedEntry = _menuEntries.Count - 1;
			}

			// Move to the next menu entry?
			if (_menuDown.Evaluate(input, ControllingPlayer, out playerIndex))
			{
				playNavigationSound = true;
				_selectedEntry++;

				if (_selectedEntry >= _menuEntries.Count)
					_selectedEntry = 0;
			}

			if (playNavigationSound)
			{
				_menuNavigateFX.Play();
			}

			if (_menuSelect.Evaluate(input, ControllingPlayer, out playerIndex))
			{
				OnSelectEntry(_selectedEntry, playerIndex);
			}
			else if (_menuCancel.Evaluate(input, ControllingPlayer, out playerIndex))
			{
				OnCancel(playerIndex);
			}


		}


		/// <summary>
		/// Handler for when the user has chosen a menu entry.
		/// </summary>
		protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
		{
			_menuEntries[entryIndex].OnSelectEntry(playerIndex);
			_menuSelectFX.Play();
		}


		/// <summary>
		/// Handler for when the user has cancelled the menu.
		/// </summary>
		protected virtual void OnCancel(PlayerIndex playerIndex)
		{
			ExitScreen();
		}


		/// <summary>
		/// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
		/// </summary>
		protected void OnCancel(object sender, PlayerIndexEventArgs e)
		{
			OnCancel(e.PlayerIndex);
		}

		/// <summary>
		/// Allows the screen the chance to position the menu entries. By default
		/// all menu entries are lined up in a vertical list, centered on the screen.
		/// </summary>
		protected virtual void UpdateMenuEntryLocations()
		{
			// Make the menu slide into place during transitions, using a
			// power curve to make things look more interesting (this makes
			// the movement slow down as it nears the end).
			float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

			// start at Y = 175; each X value is generated per entry
			Vector2 position = new Vector2(0f, 175f);

			// update each menu entry's location in turn
			for (int i = 0; i < _menuEntries.Count; i++)
			{
				MenuEntry menuEntry = _menuEntries[i];

				// each entry is to be centered horizontally
				position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

				if (ScreenState == ScreenState.TransitionOn)
					position.X -= transitionOffset * 256;
				else
					position.X += transitionOffset * 512;

				// set the entry's position
				menuEntry.Position = position;

				// move down for the next entry the size of this entry
				position.Y += menuEntry.GetHeight(this);
			}
		}

		public override void Activate(bool instancePreserved)
		{
			if (!instancePreserved)
			{
				if (_content == null)
				{
					_content = new ContentManager(ScreenManager.Game.Services, @"Content\UI\Menu");
				}
				_menuNavigateFX = _content.Load<SoundEffect>(@"Sounds\MenuNavigate");
				_menuSelectFX = _content.Load<SoundEffect>(@"Sounds\MenuSelect");
			}
		}

		/// <summary>
		/// Updates the menu.
		/// </summary>
		public override void Update(GameTime gameTime, bool otherScreenHasFocus,
													   bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			// Update each nested MenuEntry object.
			for (int i = 0; i < _menuEntries.Count; i++)
			{
				bool isSelected = IsActive && (i == _selectedEntry);

				_menuEntries[i].Update(this, isSelected, gameTime);
			}
		}


		/// <summary>
		/// Draws the menu.
		/// </summary>
		public override void Draw(GameTime gameTime)
		{
			// make sure our entries are in the right place before we draw them
			UpdateMenuEntryLocations();

			GraphicsDevice graphics = ScreenManager.GraphicsDevice;
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;

			spriteBatch.Begin();

			// Draw each menu entry in turn.
			for (int i = 0; i < _menuEntries.Count; i++)
			{
				MenuEntry menuEntry = _menuEntries[i];

				bool isSelected = IsActive && (i == _selectedEntry);

				menuEntry.Draw(this, isSelected, gameTime);
			}

			// Make the menu slide into place during transitions, using a
			// power curve to make things look more interesting (this makes
			// the movement slow down as it nears the end).
			float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

			// Draw the menu title centered on the screen
			Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
			Vector2 titleOrigin = font.MeasureString(_menuTitle) / 2;
			Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
			float titleScale = 1.25f;

			titlePosition.Y -= transitionOffset * 100;

			spriteBatch.DrawString(font, _menuTitle, titlePosition, titleColor, 0,
								   titleOrigin, titleScale, SpriteEffects.None, 0);

			spriteBatch.End();
		}
	}
}
