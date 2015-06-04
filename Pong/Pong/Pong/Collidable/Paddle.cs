using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.PowerUps;

namespace Pong.Collidable
{
	public class Paddle
	{
		private static class Specs
		{
			public const int Height = 38;
			public const int Width = 38;
		}

		public Paddle(Texture2D spriteMap)
		{
			this.SpriteMap = spriteMap;
			this.Height = Specs.Height;
			this.Width = Specs.Width;
			this.Source = new Rectangle(0, 0, this.Width, this.Height);
		}

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

		public List<IPowerUp> PowerUps = new List<IPowerUp>();

		public Vector2 Speed;
		public Vector2 Position;

		public bool CollidesWith(Rectangle foreignHtbox)
		{
			return this.Hitbox.Intersects(foreignHtbox);
		}

//		public Vector2 DefaultPosition;
//		public bool InPlay;
	}
}