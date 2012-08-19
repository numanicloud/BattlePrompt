using System;

namespace Nac.Geometory
{
	public static class Collision
	{
		public static bool DetectShape( IShape shape1, IShape shape2 )
		{
			return Detect( (dynamic)shape1, (dynamic)shape2 );
		}

		public static bool Detect( Vector2D point1, Vector2D point2 )
		{
			return (int)point1.X == (int)point2.X && (int)point1.Y == (int)point2.Y;
		}
		public static bool Detect( Vector2D point, Rectangle rect )
		{
			return rect.LeftTop.X <= point.X && point.X <= rect.RightBottom.X &&
				rect.LeftTop.Y <= point.Y && point.Y <= rect.RightBottom.Y;
		}
		public static bool Detect( Vector2D point, Circle circle )
		{
			return Math.Pow( circle.Radius, 2 ) >= ( point - circle.Center ).SquaredLength;
		}
		public static bool Detect( Vector2D point, Segment segment )
		{
			Vector2D AP = point - segment.Begin;
			Vector2D BP = point - segment.End;
			return Math.Abs( Vector2D.Dot( AP, BP ) - ( -AP.Length * BP.Length ) ) <= 1.2;
		}
		public static bool Detect( Vector2D point, Polygon poly )
		{
			if( !Detect( point, poly.BoundingBox ) ) return false;

			int count = 0;
			int i = poly.Vertexes.Count - 1;
			for( int j = 0; j < poly.Vertexes.Count; i = j, j++ )
			{
				double a = point.Y - poly.Vertexes[i].Y;
				double b = point.Y - poly.Vertexes[j].Y;
				if( a == 0 ) a += Single.Epsilon;
				if( b == 0 ) b += Single.Epsilon;

				if( a * b < 0 )
				{
					double c = Vector2D.Wedge( poly.Vertexes[j] - poly.Vertexes[i], point - poly.Vertexes[i] );
					double d = poly.Vertexes[i].Y - poly.Vertexes[j].Y;
					if( c * d <= 0 )
						++count;
				}
			}

			return count % 2 == 1;
		}

		public static bool Detect( Circle circle, Vector2D point )
		{
			return Detect( point, circle );
		}
		public static bool Detect( Circle circle1, Circle circle2 )
		{
			return Math.Pow( circle1.Radius + circle2.Radius, 2 ) > ( circle1.Center - circle2.Center ).SquaredLength;
		}
		public static bool Detect( Circle circle, Rectangle rect )
		{
			return Detect( circle, rect.PolygonRect );
		}
		public static bool Detect( Circle circle, Segment segment )
		{
			if( Detect( circle, segment.Begin ) ) return true;
			if( Detect( circle, segment.End ) ) return true;

			if( Vector2D.Dot( circle.Center - segment.Begin, circle.Center - segment.End ) <= 0 )
			{
				if( Math.Abs( Vector2D.Wedge( ( segment.End - segment.Begin ).Unit, circle.Center - segment.Begin ) ) <= circle.Radius )
					return true;
			}
			return false;
		}
		public static bool Detect( Circle circle, Polygon poly )
		{
			int i = poly.Vertexes.Count - 1;
			for( int j = 0; j < poly.Vertexes.Count; i = j, j++ )
			{
				if( Detect( circle, new Segment( poly.Vertexes[j], poly.Vertexes[i] ) ) )
					return true;
			}
			return Detect( circle.Center, poly );
		}

		public static bool Detect( Segment segment, Vector2D point )
		{
			return Detect( point, segment );
		}
		public static bool Detect( Segment segment, Circle circle )
		{
			return Detect( circle, segment );
		}
		public static bool Detect( Segment segment, Rectangle rect )
		{
			return Detect( segment, rect.PolygonRect );
		}
		public static bool Detect( Segment segment1, Segment segment2 )
		{
			double a = Vector2D.Wedge( segment1.End - segment1.Begin, segment2.Begin - segment1.Begin );
			double b = Vector2D.Wedge( segment1.End - segment1.Begin, segment2.End - segment1.Begin );
			if( a * b <= 0 )
			{
				double c = Vector2D.Wedge( segment2.End - segment2.Begin, segment1.Begin - segment2.Begin );
				double d = Vector2D.Wedge( segment2.End - segment2.Begin, segment1.End - segment2.Begin );
				if( c * d <= 0 )
					return true;
			}
			return false;
		}
		public static bool Detect( Segment segment, Polygon poly )
		{
			if( Detect( segment.Begin, poly ) ) return true;
			if( Detect( segment.End, poly ) ) return true;

			int i = poly.Vertexes.Count - 1;
			for( int j = 0; j < poly.Vertexes.Count; i = j, j++ )
			{
				if( Detect( segment, new Segment( poly.Vertexes[i], poly.Vertexes[j] ) ) )
					return true;
			}
			return false;
		}

		public static bool Detect( Rectangle rect, Vector2D point )
		{
			return Detect( point, rect );
		}
		public static bool Detect( Rectangle rect, Circle circle )
		{
			return Detect( circle, rect );
		}
		public static bool Detect( Rectangle rect, Segment segment )
		{
			return Detect( segment, rect );
		}
		public static bool Detect( Rectangle rect1, Rectangle rect2 )
		{
			return rect1.Left <= rect2.Right &&
				rect1.Top <= rect2.Bottom &&
				rect2.Left <= rect1.Right &&
				rect2.Top <= rect1.Bottom;
		}
		public static bool Detect( Rectangle rect, Polygon poly )
		{
			return Detect( rect.PolygonRect, poly );
		}

		public static bool Detect( Polygon poly, Vector2D point )
		{
			return Detect( point, poly );
		}
		public static bool Detect( Polygon poly, Circle circle )
		{
			return Detect( circle, poly );
		}
		public static bool Detect( Polygon poly, Segment segment )
		{
			return Detect( segment, poly );
		}
		public static bool Detect( Polygon poly, Rectangle rect )
		{
			if( !Detect( poly.BoundingBox, rect ) ) return false;
			return Detect( rect, poly );
		}
		public static bool Detect( Polygon poly1, Polygon poly2 )
		{
			if( !Detect( poly1.BoundingBox, poly2.BoundingBox ) ) return false;

			for( int i = 0; i < poly1.Vertexes.Count; i++ )
			{
				if( Detect( poly2, poly1.Vertexes[i] ) )
					return true;
			}
			for( int i = 0; i < poly2.Vertexes.Count; i++ )
			{
				if( Detect( poly1, poly2.Vertexes[i] ) )
					return true;
			}

			int j = poly1.Vertexes.Count - 1;
			for( int i = 0; i < poly1.Vertexes.Count; j = i, i++ )
			{
				if( Detect( new Segment( poly1.Vertexes[i], poly1.Vertexes[j] ), poly2 ) )
					return true;
			}

			return false;
		}

		/* 動く図形の動作方法を変えるので、いったんコメントアウト。改めて実装するときのためのメモにする。
		private static double GetDist( MovePoint point1, MovePoint point2 )
		{
			//前のフレーム中に衝突したかどうかで判定する。
			Vector2D a = point2.P - point1.P;
			Vector2D b = ( point2.V + point2.A ) - ( point1.V + point1.A );
			double time = -Vector2D.Dot( a, b ) / b.SquaredLength;

			if( time < -1 ) return ( a - b ).Length;		//前フレームの始めより前
			else if( time > 0 ) return a.Length;			//前のフレームの終わりより後
			else return Math.Abs( Vector2D.Wedge( a, b ) / b.Length );
		}
		private static double GetSqDist( MovePoint point1, MovePoint point2 )
		{
			//前のフレーム中に衝突したかどうかで判定する。
			Vector2D a = point2.P - point1.P;
			Vector2D b = ( point2.V + point2.A ) - ( point1.V + point1.A );
			double time = -Vector2D.Dot( a, b ) / b.SquaredLength;

			if( time < -1 ) return ( a-b ).SquaredLength;		//前フレームの始めより前
			else if( time > 0 ) return a.SquaredLength;			//前のフレームの終わりより後
			else return Math.Pow( Vector2D.Wedge( a, b ), 2 ) / b.SquaredLength;
		}

		public static bool Detect( MovePoint point1, MovePoint point2 )
		{
			return (int)GetSqDist( point1, point2 ) == 0;
		}
		public static bool Detect( MovePoint point, MoveCircle circle )
		{
			return GetSqDist( point, circle ) <= circle.R * circle.R;
		}
		public static bool Detect( MovePoint point1, Vector2D point2 )
		{
			return Detect( point1, new MovePoint( point2, Vector2D.Zero, Vector2D.Zero ) );
		}
		public static bool Detect( MovePoint point, Circle circle )
		{
			return Detect( point, new MoveCircle( circle, Vector2D.Zero, Vector2D.Zero ) );
		}

		public static bool Detect( MoveCircle circle, MovePoint point )
		{
			return Detect( point, circle );
		}
		public static bool Detect( MoveCircle circle1, MoveCircle circle2 )
		{
			return GetSqDist( circle1, circle2 ) <= Math.Pow( circle1.R + circle2.R, 2 );
		}
		public static bool Detect( MoveCircle circle, Vector2D point )
		{
			return Detect( point, circle );
		}
		public static bool Detect( MoveCircle circle1, Circle circle2 )
		{
			return Detect( circle2, circle1 );
		}
		//*/
	}
}
