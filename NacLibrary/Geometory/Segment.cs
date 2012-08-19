using System;

namespace Nac.Geometory
{
	public struct Segment : IShape
	{
		public Vector2D Begin;
		public Vector2D End;
		public Vector2D BeginToEnd
		{
			get { return Vector2D.GetDistance( Begin, End ); }
			set { End = Begin + value; }
		}

		public Vector2D LeftTop
		{
			get { return new Vector2D( Math.Min( Begin.X, End.X ), Math.Min( Begin.Y, End.Y ) ); }
			set
			{
				Vector2D distance = Vector2D.GetDistance( LeftTop, value );
				Begin += distance;
				End += distance;
			}
		}
		public Vector2D RightBottom
		{
			get { return new Vector2D( Math.Max( Begin.X, End.X ), Math.Max( Begin.Y, End.Y ) ); }
			set
			{
				Vector2D distance = Vector2D.GetDistance( RightBottom, value );
				Begin += distance;
				End += distance;
			}
		}

		public Segment( float startX, float startY, float lastX, float lastY )
		{
			Begin = new Vector2D( startX, startY );
			End = new Vector2D( lastX, lastY );
		}
		public Segment( Vector2D start, Vector2D last )
		{
			this.Begin = start;
			this.End = last;
		}
	}

}
