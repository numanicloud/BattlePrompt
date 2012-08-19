using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nac.Geometory;

namespace Nac.Helpers
{
	public static class MathHelper
	{
		/// <summary>
		/// 黄金比の値。
		/// </summary>
		public const float GoldenRate = 1.618033989f;
		/// <summary>
		/// 黄金角。
		/// </summary>
		public static readonly Angle GoldenAngle = Angle.FromDegree( 360 / (float)Math.Pow( GoldenRate, 2 ) );

		/// <summary>
		/// <paramref name="lop"/> から <paramref name="rop"/> だけ選ぶ組み合わせ、または二項係数を計算します。
		/// </summary>
		/// <param name="lop">左オペランド。</param>
		/// <param name="rop">右オペランド。</param>
		/// <returns></returns>
		public static int Conbination( int lop, int rop )
		{
			int result = Permutation( lop, rop ) / rop.Factorial();
			return result;
		}
		/// <summary>
		/// <paramref name="lop"/> から <paramref name="rop"/> だけ選んだ順列を計算します。
		/// </summary>
		/// <param name="lop">左オペランド。</param>
		/// <param name="rop">右オペランド。</param>
		/// <returns></returns>
		public static int Permutation( int lop, int rop )
		{
			return lop.Factorial() / ( lop - rop ).Factorial();
		}
		/// <summary>
		/// 階乗を計算します。
		/// </summary>
		/// <param name="num">階乗する値。</param>
		/// <returns></returns>
		public static int Factorial( this int num )
		{
			if( num == 0 ) return 1;
			else return num * Factorial( num - 1 );
		}

		public static T Clamp<T>( T value, T min, T max ) where T : IComparable
		{
			if( value.CompareTo( max ) == 1 )
				return max;
			else if( value.CompareTo( min ) == -1 )
				return min;
			else
				return value;
		}
		public static T Ceiling<T>( T value, T max ) where T : IComparable
		{
			return value.CompareTo( max ) == 1 ? max : value;
		}
		public static T Floor<T>( T value, T min ) where T : IComparable
		{
			return value.CompareTo( min ) == -1 ? min : value;
		}
		public static bool IsInside<T>( T value, T min, T max ) where T : IComparable
		{
			return value.CompareTo( min ) != -1 && value.CompareTo( max ) != 1;
		}

		public static IEnumerable<float> OneSequence( float diff )
		{
			for( float f = 0.0f; f < 1.0f; f += diff )
				yield return f;
			yield return 1.0f;
		}

		public static bool AboutEqual( double x, double y, double tolerance = 1e-12 )
		{
			return Math.Abs( x - y ) < tolerance;
		}
		public static bool AboutEqual( float x, float y, float tolerance = (float)1e-12 )
		{
			return Math.Abs( x - y ) < tolerance;
		}

		public static Vector2D GetCenter( this IShape shape )
		{
			return ( shape.LeftTop + shape.RightBottom ) / 2;
		}
		public static Shape Bind<Shape>( Shape shape, Rectangle Binding ) where Shape : IShape
		{
			var min = new Vector2D();
			var max = new Vector2D();

			min.X = Floor( shape.LeftTop.X, Binding.Left );
			min.Y = Floor( shape.LeftTop.Y, Binding.Top );
			shape.LeftTop = min;

			max.X = Ceiling( shape.RightBottom.X, Binding.Right );
			max.Y = Ceiling( shape.RightBottom.Y, Binding.Bottom );
			shape.RightBottom = max;

			return shape;
		}
	}
}
