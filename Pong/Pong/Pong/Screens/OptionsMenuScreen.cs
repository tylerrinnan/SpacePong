using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pong.Screens
{
	class OptionsMenuScreen : MenuScreen
	{
		private bool drawSelectors = false;
		public OptionsMenuScreen() : base("Options")
		{
			MenuEntry gameVolume = new MenuEntry("Game Volume");
			MenuEntry exitEntry = new MenuEntry("Back");

			gameVolume.Selected += OnGameVolumeSelected;
			exitEntry.Selected += OnCancel;

			MenuEntries.Add(gameVolume);
			MenuEntries.Add(exitEntry);
		}

		private void OnGameVolumeSelected(object sender, PlayerIndexEventArgs e)
		{
			drawSelectors = true;
		}
	}
}
