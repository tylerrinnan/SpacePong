using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GameStateManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Collidable;
using Pong.PowerUps;

namespace Pong.Screens
{
	class PlayGameScreen : GameScreen
	{
		private ContentManager _content;
		private InputAction _pauseAction;
		protected List<Ball> Balls;
		protected Paddle PlayerOne;
		protected Paddle PlayerTwo;
		protected PowerUpManager PowerUpManager;

		protected static class Constants
		{
			public const int PaddleSpeed = 15;
			public const int BallBounceIncrease = 45;
		}

		protected static class Values
		{
			public static int PlayerOneScore = 0;
			public static int PlayerTwoScore = 0;
			public static int TotalCycle = 0;
			public static float TimePassedForAnimation;
		}

		protected static class Textures
		{
			public static Texture2D BackgroundWallpaper;
			public static Texture2D PlayerOnePaddle;
			public static Texture2D PlayerTwoPaddle;
			public static Texture2D BallSpriteMap;
			public static Texture2D MultiBallSpriteMap;
		}

		protected static class SoundEffectInstances
		{
			public static SoundEffectInstance PongBallBounceFx;
			public static SoundEffectInstance BackgroundMusicFx;
			public static SoundEffectInstance SpeedupFx;
		}

		protected static class Fonts
		{
			public static SpriteFont ScoreFont;
		}

		protected static class Vectors
		{
			public static Vector2 PaddleSize;
			public static Vector2 PlayerTwoScorePosition;
			public static Vector2 PlayerOneScorePosition;
			public static Vector2 DefaultBallPosition;
		}

//		public MultiBall MultiBall;
		// private static readonly Vector2 PongBallDefaultSpeed = new Vector2(150, 1);

		//multi ball
		private bool _isMultiball = false;

		public PlayGameScreen()
		{
			TransitionOnTime = TimeSpan.FromSeconds(1.0);
			TransitionOffTime = TimeSpan.FromSeconds(0.5);
			

			_pauseAction = new InputAction(
				new Buttons[] {Buttons.Start, Buttons.Back}, 
				new Keys[] {Keys.Escape},
				true);
		}

		public override void Activate(bool instancePreserved)
		{
			if (!instancePreserved)
			{
				LoadContent();
				ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);
				StartGame();
			}
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			if (IsActive)
			{
				UpdateBallPosition(gameTime);
				UpdateForCollision();

				Values.TimePassedForAnimation += gameTime.ElapsedGameTime.Milliseconds;
				const int timeRequired = 50;
				if (Values.TimePassedForAnimation > timeRequired)
				{
					Values.TimePassedForAnimation -= timeRequired;
					foreach (var ball in Balls)
					{
						ball.Source.X += ball.Width;

						if (ball.Source.X >= ball.SpriteMap.Width)
						{
							ball.Source.X = 0;
						}
					}
					
//					if (_isMultiball)
//					{
//						MultiBall.Source.X += MultiBall.Width;
//						if (MultiBall.Source.X >= MultiBall.SpriteMap.Width)
//						{
//							MultiBall.Source.X = 0;
//						}
//					}
				}
			}
			base.Update(gameTime, otherScreenHasFocus, false);
		}

		public override void Draw(GameTime gameTime)
		{
			var spriteBatch = ScreenManager.SpriteBatch;
			Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
			Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

			spriteBatch.Begin();
			spriteBatch.Draw(Textures.BackgroundWallpaper, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
			spriteBatch.Draw(Textures.PlayerOnePaddle, PlayerOne.Position, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
			spriteBatch.Draw(Textures.PlayerTwoPaddle, PlayerTwo.Position, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
			spriteBatch.DrawString(Fonts.ScoreFont, Values.PlayerOneScore.ToString(), Vectors.PlayerOneScorePosition, Color.Orange);
			spriteBatch.DrawString(Fonts.ScoreFont, Values.PlayerTwoScore.ToString(), Vectors.PlayerTwoScorePosition, Color.Orange);

			foreach (var ball in Balls)
			{
				spriteBatch.Draw(ball.SpriteMap, ball.Position, ball.Source, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
			}

			spriteBatch.End();
		}

		public virtual void UpdatePlayerTwoPosition(KeyboardState keyboardState)
		{
			// To be implemented in derived class.
		}

		public void UpdatePlayerOnePosition(KeyboardState keyboardState)
		{
			if (keyboardState.IsKeyDown(Keys.Up))
			{
				PlayerOne.Position.Y -= Constants.PaddleSpeed;
				if (PlayerOne.Position.Y < 0)
				{
					PlayerOne.Position.Y = 0;
				}
			}

			if (keyboardState.IsKeyDown(Keys.Down))
			{
				var screenHeight = ScreenManager.Game.Window.ClientBounds.Height;
				PlayerOne.Position.Y += Constants.PaddleSpeed;
				if (PlayerOne.Position.Y + Vectors.PaddleSize.Y > screenHeight)
				{
					PlayerOne.Position.Y = screenHeight - Vectors.PaddleSize.Y;
				}
			}
		}

		public override void HandleInput(GameTime gameTime, InputState input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}

			int playerIndex = (int) ControllingPlayer.Value;

			KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
			GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

			//Pause game if controller pad unplugs
			bool gamePadDisconnected = !gamePadState.IsConnected &&
						   input.GamePadWasConnected[playerIndex];

			PlayerIndex player;
			if (_pauseAction.Evaluate(input, ControllingPlayer, out player) || gamePadDisconnected)
			{
				ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
			}
			
			PowerUpManager.ScreenForPowerUpActivation(input, ControllingPlayer);
			PowerUpManager.TriggerActivePowerUps(Balls);
			UpdatePlayerOnePosition(keyboardState);
			UpdatePlayerTwoPosition(keyboardState);
			
		}

		public override void Unload()
		{
			_content.Unload();
		}
		
		private void SetStartLocations()
		{
			var window = ScreenManager.Game.Window;
			var initialBall = Balls[0];
			Vectors.PaddleSize = new Vector2(Textures.PlayerOnePaddle.Width, Textures.PlayerOnePaddle.Height);
			PlayerOne.Position = new Vector2(0, 0);
			PlayerTwo.Position = new Vector2(window.ClientBounds.Width - Vectors.PaddleSize.X, 0);
			Vectors.PlayerOneScorePosition = new Vector2(window.ClientBounds.Width / 2f - 50 , 0);
			Vectors.PlayerTwoScorePosition = new Vector2(window.ClientBounds.Width / 2f + 50 - Fonts.ScoreFont.MeasureString("0").X, 0);
			Vectors.DefaultBallPosition = new Vector2(window.ClientBounds.Width / 2f - initialBall.Width / 2f, window.ClientBounds.Height / 2f - initialBall.Height / 2f);
			initialBall.Position = Vectors.DefaultBallPosition;
		}

		private void UpdateForCollision()
		{
			this.UpdateForPaddleCollision();
			this.UpdateForBoundsCollision();
		}
		
		private void UpdateForPaddleCollision()
		{

			foreach (var ball in Balls)
			{
				var ballHitbox = ball.Hitbox;
				Rectangle p1Hitbox = new Rectangle((int)PlayerOne.Position.X, (int)PlayerOne.Position.Y, Textures.PlayerOnePaddle.Width, Textures.PlayerOnePaddle.Height);
				Rectangle p2Hitbox = new Rectangle((int)PlayerTwo.Position.X, (int)PlayerTwo.Position.Y, Textures.PlayerTwoPaddle.Width, Textures.PlayerTwoPaddle.Height);
				bool isCollision = false;

				if (!_isMultiball && (ball.CollidesWith(p1Hitbox) && ball.Speed.X < 0) || (ball.CollidesWith(p2Hitbox) && ball.Speed.X > 0))
				{
					SoundEffectInstances.PongBallBounceFx.Play();
					Rectangle context = ball.CollidesWith(p1Hitbox) ? p1Hitbox : p2Hitbox;

					var keyboardState = Keyboard.GetState();
					if (keyboardState.IsKeyDown(Keys.Up) && context == p1Hitbox ||
						keyboardState.IsKeyDown(Keys.W) && context == p2Hitbox)
					{
						ball.Speed.Y -= 60;
					}

					if (keyboardState.IsKeyDown(Keys.Down) && context == p1Hitbox ||
						keyboardState.IsKeyDown(Keys.S) && context == p2Hitbox)
					{
						ball.Speed.Y += 60;
					}

					Rectangle locationOfIntersection;
					Rectangle.Intersect(ref ballHitbox, ref context, out locationOfIntersection);
					if ((locationOfIntersection.Y == context.Y && ball.Speed.Y > 0) ||
						locationOfIntersection.Y + locationOfIntersection.Height == context.Y + p1Hitbox.Height && ball.Speed.Y < 0)
					{
						//Hitting corners sharpens bounce angle
						ball.Speed.Y = ball.Speed.Y > 0 ? ball.Speed.Y += 30 : ball.Speed.Y -= 30;
						ball.Speed.Y *= -1;
					}
					else if (locationOfIntersection.Y == context.Y && ball.Speed.Y < 0 ||
							 locationOfIntersection.Y + locationOfIntersection.Height == context.Y + p1Hitbox.Height &&
							 ball.Speed.Y > 0)
					{
						ball.Speed.Y = ball.Speed.Y > 0 ? ball.Speed.Y += 30 : ball.Speed.Y -= 30;
					}

					ball.Speed.X = ball.Speed.X > 0
						? ball.Speed.X += Constants.BallBounceIncrease
						: ball.Speed.X -= Constants.BallBounceIncrease;

					ball.Speed.X *= -1;
					Values.TotalCycle += 1;

					if (Values.TotalCycle == 20)
					{
						SoundEffectInstances.SpeedupFx.Play();

					}
				}
			}
		}

		private void UpdateForBoundsCollision()
		{
			//Remove a ball from array if it exceeds horizontal bounds
			for(int i = Balls.Count - 1; i >= 0; --i)
			{
				var ball = Balls[i];
				int maxX = ScreenManager.GraphicsDevice.Viewport.Width + ball.Width;
				int maxY = ScreenManager.GraphicsDevice.Viewport.Height - ball.Height;

				// Check for vertical bounce
				if (ball.Position.Y > maxY || ball.Position.Y < 0)
				{
					SoundEffectInstances.PongBallBounceFx.Play();
					ball.Speed.Y *= -1;
				}

				// Check for Score
				if (ball.Position.X > maxX || ball.Position.X < 0 - ball.Width)
				{
					SoundEffectInstances.SpeedupFx.Stop();
					Score(ball.Position.X > maxX ? 0 : 1);
					Balls.RemoveAt(i);
				}
			}

			if (Balls.Count == 0)
			{
				RenewBallList();
			}
		}

		private void UpdateBallPosition(GameTime gameTime)
		{
			foreach (var ball in Balls)
			{
				ball.Position += ball.Speed*(float) gameTime.ElapsedGameTime.TotalSeconds;
			}
		}

		private void StartGame()
		{
			//Create Paddles
			PlayerOne = new Paddle(Textures.PlayerOnePaddle);
			PlayerOne.PowerUps.Add(new MultiBall());
			PlayerTwo = new Paddle(Textures.PlayerTwoPaddle);

			//Create PowerUpManager to handle special actions
			PowerUpManager = new PowerUpManager(PlayerOne, PlayerTwo, _content);

			//Initialize game ball and list
			RenewBallList();

			//Assign Positions for collidable entities
			SetStartLocations();

			SoundEffectInstances.BackgroundMusicFx.IsLooped = true;
			SoundEffectInstances.BackgroundMusicFx.Volume = .25f;
			SoundEffectInstances.BackgroundMusicFx.Play();
		}

		/// <summary>
		/// Method to handle updating score and counter.
		/// </summary>
		/// <param name="scoreLocation">Integer 0 (player one) or 1 (player two) representing which player scored.</param>
		private void Score(int scoreLocation)
		{
			Values.TotalCycle = 0;

			if (scoreLocation == 0)
			{
				++Values.PlayerOneScore;
			}
			else if (scoreLocation == 1)
			{
				++Values.PlayerTwoScore;
			}
			else
			{
				throw new InvalidEnumArgumentException("scoreLocation");
			}
		}

		private void LoadContent()
		{
			if (_content == null)
			{
				_content = new ContentManager(ScreenManager.Game.Services, @"Content\Gameplay");
			}

			Textures.PlayerOnePaddle = _content.Load<Texture2D>("Red_Left_Paddle");
			Textures.PlayerTwoPaddle = _content.Load<Texture2D>("Blue_Right_Paddle");
			Textures.BackgroundWallpaper = _content.Load<Texture2D>(@"Background");
			Textures.BallSpriteMap = _content.Load<Texture2D>("Ball_Anim_Sheet");
			Textures.MultiBallSpriteMap = _content.Load<Texture2D>("MultiBall_Sprite_Sheet");

			SoundEffectInstances.PongBallBounceFx = _content.Load<SoundEffect>(@"Sounds\BallBounce").CreateInstance();
			SoundEffectInstances.BackgroundMusicFx = _content.Load<SoundEffect>(@"Sounds\BackgroundMusic").CreateInstance();
			SoundEffectInstances.SpeedupFx = _content.Load<SoundEffect>(@"Sounds\SpeedupSound").CreateInstance();

			Fonts.ScoreFont = _content.Load<SpriteFont>(@"Score\Fonts\Score");
		}

		private void SetBallSpawnPositionAndSpeed(Ball ball, int desiredSpeed)
		{
			Random rand = new Random();
			var yLengthRandom = rand.Next(1, (int)(desiredSpeed - desiredSpeed * .1));
			ball.Speed.Y = yLengthRandom;
			ball.Speed.X = (float)MathHalp.MathHalp.ReturnSideWithKnownHypotenuse(desiredSpeed, yLengthRandom);

			ball.Speed.X *= rand.NextDouble() >= 0.5 ? 1 : -1;
			ball.Speed.Y *= rand.NextDouble() >= 0.5 ? 1 : -1;

			ball.Position = Vectors.DefaultBallPosition;
			ball.IsBrandNewBall = false;
		}

		private void RenewBallList()
		{
			if (Balls == null)
			{
				Balls = new List<Ball>();
			}

			var ball = new Ball(Textures.BallSpriteMap, false);
			SetBallSpawnPositionAndSpeed(ball, 200);
			Balls.Add(ball);
		}

		private void UpdateForPowerUp()
		{
			
		}
	}
}
