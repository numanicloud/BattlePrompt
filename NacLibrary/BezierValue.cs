using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nac
{
	using MyMath = Nac.Helpers.MathHelper;

	public struct BezierValue
	{
		List<float> weights { get; set; }
		IEnumerable<float> controll
		{
			get
			{
				yield return 0;
				foreach( var item in weights )
					yield return item;
				yield return 1;
			}
		}

		public float this[float paramater]
		{
			get { return GetValue( paramater ); }
		}

		public BezierValue( float beginWeight, float endWeight )
			: this()
		{
			weights = new List<float>();
			for( int i = 0; i < beginWeight; i++ )
				weights.Add( 0.5f - MyMath.Clamp( ( beginWeight - i ) / 2.0f, 0, 0.5f ) );
			for( int i = 0; i < endWeight; i++ )
				weights.Add( 0.5f + MyMath.Clamp( ( endWeight - i ) / 2.0f, 0, 0.5f ) );
		}

		public float GetValue( float paramater )
		{
			paramater = MyMath.Clamp( paramater, 0, 1 );

			float result = 0;
			int n = controll.Count() - 1;
			for( int i = 0; i <= n; i++ )
			{
				result += controll.ElementAt( i ) * MyMath.Conbination( n, i ) * (float)System.Math.Pow( paramater, i ) * (float)System.Math.Pow( 1 - paramater, n - i );
			}
			return result;
		}

	}
}
