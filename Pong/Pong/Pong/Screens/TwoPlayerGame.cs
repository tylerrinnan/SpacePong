using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Pong.Screens
{
	class TwoPlayerGame : PlayGameScreen
	{
		public TwoPlayerGame() : base()
		{
		}

		public override void UpdatePlayerTwoPosition(KeyboardState keyboardState)
		{
			if (keyboardState.IsKeyDown(Keys.W))
			{
				PlayerTwo.Position.Y -= Constants.PaddleSpeed;
				if (PlayerTwo.Position.Y < 0)
				{
					PlayerTwo.Position.Y = 0;
				}
			}

			if (keyboardState.IsKeyDown(Keys.S))
			{
				var screenHeight = ScreenManager.Game.Window.ClientBounds.Height;
				PlayerTwo.Position.Y += Constants.PaddleSpeed;
				if (PlayerTwo.Position.Y + Vectors.PaddleSize.Y > screenHeight)
				{
					PlayerTwo.Position.Y = screenHeight - Vectors.PaddleSize.Y;
				}
			}
		}
	}
}
