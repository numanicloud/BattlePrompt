using System;
using System.Collections.Generic;
using System.Linq;

namespace Nac.Geometory
{
	public struct Polygon : IShape
	{
		public List<Vector2D> Vertexes { get; private set; }
		public Rectangle BoundingBox
		{
			get
			{
				return new Rectangle(
					Vertexes.Min<Vector2D>( v => v.X ),
					Vertexes.Min<Vector2D>( v => v.Y ),
					Vertexes.Max<Vector2D>( v => v.X ),
					Vertexes.Max<Vector2D>( v => v.Y )
					);
			}
		}
		public Vector2D LeftTop
		{
			get { return BoundingBox.LeftTop; }
			set
			{
				Vector2D distance = value - LeftTop;
				Vertexes = Vertexes.Select( x => x + distance ).ToList();
			}
		}
		public Vector2D RightBottom
		{
			get { return BoundingBox.RightBottom; }
			set
			{
				Vector2D distance = value - RightBottom;
				Vertexes = Vertexes.Select( x => x + distance ).ToList();
			}
		}

		public Polygon( int num )
			: this()
		{
			Vertexes = new List<Vector2D>();
			for( int i = 0; i < num; i++ )
				Vertexes.Add( new Vector2D() );
		}
		public Polygon( params float[] pos )
			: this()
		{
			if( pos.Length % 2 != 0 )
				throw new ArgumentException( "座標は ( x, y ) の組のため、引数の数は２の倍数個である必要があります。" );

			this.Vertexes = new List<Vector2D>();
			for( int i = 0; i+1 < pos.Length; i += 2 )
				this.Vertexes.Add( new Vector2D( pos[i], pos[i + 1] ) );
		}
		public Polygon( params Vector2D[] pos )
			: this()
		{
			this.Vertexes = new List<Vector2D>();
			foreach( var item in pos )
				this.Vertexes.Add( item );
		}
	}
}
