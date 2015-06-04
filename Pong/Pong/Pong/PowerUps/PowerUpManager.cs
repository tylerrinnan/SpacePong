using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Collidable;
using Microsoft.Xna.Framework;

namespace Pong.PowerUps
{
	public class PowerUpManager
	{
		private Paddle _playerOne;
		private Paddle _playerTwo;
		private ContentManager _content;

		public PowerUpManager(Paddle playerOne, Paddle playerTwo, ContentManager content)
		{
			this._playerOne = playerOne;
			this._playerTwo = playerTwo;
			this._content = content;
		}

		public void ScreenForPowerUpActivation(InputState inputState, PlayerIndex? controllingPlayer)
		{
			Paddle playerPaddle = controllingPlayer != null && controllingPlayer.Value == PlayerIndex.One
				? _playerOne
				: _playerTwo;

			foreach (var powerup in playerPaddle.PowerUps)
			{
				PlayerIndex player;
				if(powerup.ActivationKey.Evaluate(inputState, controllingPlayer, out player))
				{
					powerup.IsActive = true;
				}
			}
		}

		public void TriggerActivePowerUps(List<Ball> balls)
		{

			for (int i = _playerOne.PowerUps.Count - 1; i >= 0; --i)
			{
				IPowerUp powerup = _playerOne.PowerUps[i];
				if (powerup.IsActive)
				{
					if (powerup.GetType() == typeof (MultiBall))
					{
						TriggerMultiBall(balls);
						_playerOne.PowerUps.RemoveAt(i);
					}
				}
			}
		}

		public void TriggerMultiBall(List<Ball> balls)
		{
			Vector2 originalPosition = balls[0].Position;
			Vector2 originalSpeed = balls[0].Speed;
			balls.Clear();

			balls.Add(new Ball(_content.Load<Texture2D>("MultiBall_Sprite_Sheet"), true)
			{
				Position = originalPosition, Speed = originalSpeed
			});
			balls.Add(new Ball(_content.Load<Texture2D>("MultiBall_Sprite_Sheet"), true)
			{
				Position = originalPosition, Speed = originalSpeed
			});
			balls.Add(new Ball(_content.Load<Texture2D>("MultiBall_Sprite_Sheet"), true)
			{
				Position = originalPosition, Speed = originalSpeed
			});
		}
	}
}
