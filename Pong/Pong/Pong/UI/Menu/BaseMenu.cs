//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//
//namespace Pong.UI.Menu
//{
//	class BaseMenu
//	{
//		private int _keyState = 0;
//		private KeyboardState _oldState;
//		private SoundEffect _menuNavigateSoundEffect;
//		private SpriteFont _menuFont;
//		private Texture2D _menuSelector;
//		private Vector2 _menuSelectorVector;
//		private Vector2 _playerOneMenuVector;
//		private Vector2 _playerTwoMenuVector;
//		private Vector2 _playerOneFontDimensions;
//		private Vector2 _playerTwoFontDimensions;
//		private const string PlayerOne = "Player One";
//		private const string PlayerTwo = "Player Two";
//		private const int EntitySpacing = 10;
//
//		public BaseMenu()
//		{
//			_oldState = Keyboard.GetState();
//		}
//
//		public void Update()
//		{
//			var newState = Keyboard.GetState();
//
//			if (newState.IsKeyDown(Keys.Down))
//			{
//				if (!_oldState.IsKeyDown(Keys.Down))
//				{
//					if (_keyState == 0)
//					{
//						_keyState = 1;
//						_menuSelectorVector.Y += _playerOneFontDimensions.Y + EntitySpacing;
//						_menuNavigateSoundEffect.Play();
//					}
//				}
//				
//			}
//
//			if (newState.IsKeyDown(Keys.Up))
//			{
//				if (!_oldState.IsKeyDown(Keys.Up))
//				{
//					if (_keyState == 1)
//					{
//						_keyState = 0;
//						_menuSelectorVector.Y -= _playerOneFontDimensions.Y + EntitySpacing;
//						_menuNavigateSoundEffect.Play();
//					}
//				}
//				
//			}
//
//			if (newState.IsKeyDown(Keys.Enter))
//			{
//				if(!_oldState.IsKeyDown(Keys.Enter))
//				{
//					//Start game
//					((GameWindow) Pong.GameService.GetService(typeof (GameWindow))).AllowUserResizing = true;
//				}
//			}
//
//			_oldState = newState;
//		}
//
//		public void Draw()
//		{
//			SpriteBatch spriteBatch = ((SpriteBatch)Pong.GameService.GetService(typeof(SpriteBatch)));
//			spriteBatch.DrawString(_menuFont, PlayerOne, _playerOneMenuVector, Color.Black);
//			spriteBatch.DrawString(_menuFont, PlayerTwo, _playerTwoMenuVector, Color.Black);
//			spriteBatch.Draw(_menuSelector, _menuSelectorVector, Color.White);
//		}
//
//		public void LoadContent()
//		{
//			ContentManager contentManager = ((ContentManager) Pong.GameService.GetService(typeof (ContentManager)));
//			_menuFont = contentManager.Load<SpriteFont>(@"UI/Menu/Fonts/MenuButton");
//			_menuSelector = contentManager.Load<Texture2D>(@"UI/Menu/Images/MenuSelector");
//			_menuNavigateSoundEffect = contentManager.Load<SoundEffect>(@"UI/Menu/Sounds/menuselect");
//		}
//
//		//TODO: FIND A BETTER SPOT TO LOAD DEFAULT VECTOR LOCATION
//		public void Init()
//		{
//			_playerOneFontDimensions = _menuFont.MeasureString(PlayerOne);
//			_playerTwoFontDimensions = _menuFont.MeasureString(PlayerTwo);
//
//			Rectangle clientBounds = ((GameWindow)Pong.GameService.GetService(typeof(GameWindow))).ClientBounds;
//			float x = clientBounds.Width / 2 - _playerOneFontDimensions.X / 2;
//			float y = clientBounds.Height / 2 - _playerOneFontDimensions.Y / 2;
//
//			_playerOneMenuVector = new Vector2(x, y);
//			_playerTwoMenuVector = new Vector2(_playerOneMenuVector.X, _playerOneMenuVector.Y + _playerTwoFontDimensions.Y + EntitySpacing);
//			_menuSelectorVector = new Vector2(_playerOneMenuVector.X - _menuSelector.Width - EntitySpacing, _playerOneMenuVector.Y);
//		}
//	}
//}
