using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;

namespace Pong.Screens
{
	class PauseMenuScreen : MenuScreen
	{
		public PauseMenuScreen() : base("Paused")
		{
			MenuEntry continueEntry = new MenuEntry("Continue");
			MenuEntry exitEntry = new MenuEntry("Exit Game");

			continueEntry.Selected += OnCancel;
			exitEntry.Selected += ExitGameplay;

			MenuEntries.Add(continueEntry);
			MenuEntries.Add(exitEntry);
		}

		public void ExitGameplay(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.Game.Exit();
		}
	}
}
