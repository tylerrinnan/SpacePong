using System.Threading;
using GameStateManagement;
using Microsoft.Xna.Framework.Input;

namespace Pong.PowerUps
{
	public class MultiBall : IPowerUp
	{
		public MultiBall()
		{
			IsActive = false;
			ActivationKey = new InputAction(
				new Buttons[] {}, 
				new Keys[] {Keys.F},
				true
			);
		}
		
		public InputAction ActivationKey { get; private set; }

		public Timer Duration { get; set; }

		public bool IsActive { get; set; }
	}
}
