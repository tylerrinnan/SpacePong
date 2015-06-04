using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong.Screens
{
	class NetworkSetupScreen : MenuScreen
	{
		public NetworkSetupScreen() : base("Network Play")
		{
			MenuEntry joinEntry = new MenuEntry("Join Game");
			MenuEntry hostEntry = new MenuEntry("Host Game");

			joinEntry.Selected += JoinGameEntrySelected;
			hostEntry.Selected += HostGameEntrySelected;

			MenuEntries.Add(joinEntry);
			MenuEntries.Add(hostEntry);
		}

		public void JoinGameEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			//TODO: IMPLEMENT
		}

		public void HostGameEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			//TODO: IMPLEMENT
		}
	}
}
