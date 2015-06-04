using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Pong.Collidable;

namespace Pong.Screens
{
	class OnePlayerGame : PlayGameScreen
	{
		public OnePlayerGame() : base()
		{
			
		}

		public override void Activate(bool instancePreserved)
		{
			base.Activate(instancePreserved);
		}

		public override void UpdatePlayerTwoPosition(KeyboardState state)
		{
			float speed = Constants.PaddleSpeed;
			Ball closestToScore = new Ball();

			//TODO: find fastest speed towards bounds
			foreach (var ball in Balls)
			{
				if (PlayerTwo.Position.X - ball.Position.X < PlayerTwo.Position.X - closestToScore.Position.X)
				{
					closestToScore = ball;
				}
			}

			float pongBallCenter = closestToScore.Position.Y + closestToScore.Height / 2f;
			float playerTwoPaddleCenter = PlayerTwo.Position.Y + Textures.PlayerTwoPaddle.Height/2f;
			if (pongBallCenter < playerTwoPaddleCenter)
			{
				var absDistance = Math.Abs(pongBallCenter - playerTwoPaddleCenter);
				if (absDistance < Constants.PaddleSpeed)
				{
					speed = absDistance;
				}
				PlayerTwo.Position.Y -= speed;
			}

			if (pongBallCenter > playerTwoPaddleCenter)
			{
				var absDistance = Math.Abs(pongBallCenter - playerTwoPaddleCenter);
				if (absDistance < Constants.PaddleSpeed)
				{
					speed = absDistance;
				}
				PlayerTwo.Position.Y += speed;
			}

			if (PlayerTwo.Position.Y + Vectors.PaddleSize.Y > ScreenManager.Game.Window.ClientBounds.Height)
			{
				PlayerTwo.Position.Y = ScreenManager.Game.Window.ClientBounds.Height - Vectors.PaddleSize.Y;
			}

			if (PlayerTwo.Position.Y < 0)
			{
				PlayerTwo.Position.Y = 0;
			}
		}
	}
}
