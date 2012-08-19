using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nac.Geometory
{
	using MyMath = Nac.Helpers.MathHelper;

	public struct Bezier
	{
		public Vector2D this[float paramater]
		{
			get { return GetPoint( paramater ); }
		}

		IEnumerable<Vector2D> ControllPoints
		{
			get
			{
				yield return Begin;
				foreach( var item in MidiumPoints )
					yield return item;
				yield return End;
			}
		}
		public List<Vector2D> MidiumPoints { get; private set; }
		public Vector2D Begin { get; set; }
		public Vector2D End { get; set; }

		public Bezier( Vector2D begin, Vector2D end, params Vector2D[] controlls )
			: this()
		{
			MidiumPoints = new List<Vector2D>( controlls );
			this.Begin = begin;
			this.End = end;
		}
		public Vector2D GetPoint( float paramater )
		{
			if( paramater < 0 || paramater > 1 )
				throw new ArgumentException( "ベジエ曲線は paramater ∈ [0, 1] の区間内で定義されます。" );

			Vector2D result = Vector2D.Zero;
			int n = ControllPoints.Count() - 1;
			for( int i = 0; i <= n; i++ )
			{
				result += ControllPoints.ElementAt( i ) * MyMath.Conbination( n, i ) * (float)System.Math.Pow( paramater, i ) * (float)System.Math.Pow( 1 - paramater, n - i );
			}
			return result;
		}
	}
}
