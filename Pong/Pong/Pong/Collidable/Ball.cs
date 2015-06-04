using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Collidable
{
	public class Ball
	{
		private static class Specs
		{
			public const int Height = 38;
			public const int Width = 38;
			public const int MultiBallHeight = 19;
			public const int MultiBallWidth = 19;
		}

		public Ball(Texture2D spriteMap, bool isMultiball)
		{
			this.SpriteMap = spriteMap;
			this.Height = isMultiball ? Specs.MultiBallHeight : Specs.Height;
			this.Width = isMultiball ? Specs.MultiBallWidth : Specs.Width;
			this.Source = new Rectangle(0, 0, this.Width, this.Height);
		}

		/// <summary>
		/// Constructor for calculations, not rendering
		/// </summary>
		public Ball()
		{
			this.Height = Specs.Height;
			this.Width = Specs.Width;
			this.Source = new Rectangle(0, 0, this.Width, this.Height);
		}

		public bool IsBrandNewBall = true;
		public int Height { get; private set; }
		public int Width { get; private set; }
		public Texture2D SpriteMap { get; private set; }
		public Rectangle Source;

		public Rectangle Hitbox
		{
			get
			{
				return new Rectangle((int)this.Position.X, (int)this.Position.Y, this.Width, this.Height);
			}
		} 
		public Vector2 Speed = new Vector2();
		public Vector2 Position = new Vector2();

		public bool CollidesWith(Rectangle foreignHtbox)
		{
			return this.Hitbox.Intersects(foreignHtbox);
		}

//		public Vector2 DefaultPosition;
//		public bool InPlay;
	}
}