using System;

namespace Nac.Geometory
{
	public struct Circle : IShape
	{
		public Vector2D Center;
		public float Radius { get; set; }

		public Vector2D LeftTop
		{
			get { return Center - new Vector2D( Radius ); }
			set { Center = value + new Vector2D( Radius ); }
		}
		public Vector2D RightBottom
		{
			get { return Center + new Vector2D( Radius ); }
			set { Center = value - new Vector2D( Radius ); }
		}
		public Circle Unit
		{
			get { return new Circle( Center, 1.0f ); }
		}

		public Circle( float centerX, float centerY, float radius )
			: this()
		{
			Center = new Vector2D( centerX, centerY );
			this.Radius = radius;
		}
		public Circle( Vector2D center, float radius )
			: this()
		{
			Center = center;
			this.Radius = radius;
		}

		public override string ToString()
		{
			return string.Format( "p = {0}, r = {1}", Center.ToString(), Radius );
		}
		public string ToString( string format )
		{
			if( format == null )
				return ToString();
			else
				return string.Format( "p = {0}, r = {1:"+format+"}", Center.ToString( format ), Radius );
		}
	}

}
