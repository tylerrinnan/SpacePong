using Microsoft.Xna.Framework;

namespace Pong.Screens
{
	public class MainMenuScreen : MenuScreen
	{
		public MainMenuScreen() : base("Main Menu")
		{
			MenuEntry onePlayerEntry = new MenuEntry("One Player");
			MenuEntry twoPlayerEntry = new MenuEntry("Two Player");
			MenuEntry networkPlayEntry = new MenuEntry("Network Play");
			MenuEntry optionsEntry = new MenuEntry("Options");
			MenuEntry exitMenuEntry = new MenuEntry("Exit");

			onePlayerEntry.Selected += OnePlayerGameMenuEntrySelected;
			twoPlayerEntry.Selected += TwoPlayerGameMenuEntrySelected;
			networkPlayEntry.Selected += NetworkPlayMenuEntrySelected;
			optionsEntry.Selected += OptionsMenuEntrySelected;
			exitMenuEntry.Selected += OnCancel;

			MenuEntries.Add(onePlayerEntry);
			MenuEntries.Add(twoPlayerEntry);
			MenuEntries.Add(networkPlayEntry);
			MenuEntries.Add(optionsEntry);
			MenuEntries.Add(exitMenuEntry);
		}

		private void OnePlayerGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.AddScreen(new OnePlayerGame(), e.PlayerIndex);
		}

		private void TwoPlayerGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.AddScreen(new TwoPlayerGame(), e.PlayerIndex);
		}

		private void NetworkPlayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.AddScreen(new NetworkSetupScreen(), e.PlayerIndex);
		}

		private void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
		}

		/// <summary>
		/// When the user cancels the main menu, ask if they want to exit the sample.
		/// </summary>
		protected override void OnCancel(PlayerIndex playerIndex)
		{
			ScreenManager.Game.Exit();
		}
	}
}
