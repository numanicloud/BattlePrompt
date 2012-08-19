using System;

namespace Nac.Geometory
{
	public struct Rectangle : IShape
	{
		#region フィールド・プロパティ
		public Vector2D Position;
		public Vector2D Size;

		public float Left
		{
			get { return Position.X; }
			set { Position.X = value; }
		}
		public float Top
		{
			get { return Position.Y; }
			set { Position.Y = value; }
		}
		public float Right
		{
			get { return ( Position + Size ).X; }
			set { Size.X = value - Position.X; }
		}
		public float Bottom
		{
			get { return ( Position + Size ).Y; }
			set { Size.Y = value - Position.Y; }
		}
		public Vector2D LeftTop
		{
			get { return Position; }
			set { Position = value; }
		}
		public Vector2D RightBottom
		{
			get { return Position + Size; }
			set { Position = value - Size; }
		}

		public Polygon PolygonRect
		{
			get
			{
				var	polygonRect_ = new Polygon(
						new Vector2D( Left, Top ),
						new Vector2D( Right, Top ),
						new Vector2D( Right, Bottom ),
						new Vector2D( Left, Bottom )
						);
				return polygonRect_;
			}
		}
		#endregion

		public Rectangle( float x, float y, float width, float height )
			: this()
		{
			if( width < 0 )
			{
				x += width;
				width = -width;
			}
			if( height < 0 )
			{
				y += height;
				height = -height;
			}

			Position = new Vector2D( x, y );
			Size = new Vector2D( width, height );
		}
		public Rectangle( Vector2D position, Vector2D size )
			: this( position.X, position.Y, size.X, size.Y )
		{
		}

		public Rectangle Extend( float value )
		{
			var ex = new Vector2D( value );
			return new Rectangle( Position - ex, Size + ex*2 );
		}
	}

}
