using System;

namespace Nac.Geometory
{
	/// <summary>
	/// 平面上のベクトルを表す構造体。
	/// </summary>
	public struct Vector2D : IShape
	{
		#region プロパティ
		/// <summary>
		/// ベクトルのX要素。
		/// </summary>
		public float X { get; set; }
		/// <summary>
		/// ベクトルのY要素。
		/// </summary>
		public float Y { get; set; }

		/// <summary>
		/// ベクトルの大きさ。
		/// </summary>
		public float Length
		{
			get { return (float)Math.Sqrt( X * X + Y * Y ); }
			set
			{
				float rad = Radian;
				X = (float)Math.Cos( rad ) * Math.Abs( value );
				Y = (float)Math.Sin( rad ) * Math.Abs( value );
				if( value < 0 ) OneRound += 0.5f;
			}
		}
		/// <summary>
		/// ベクトルの大きさの２乗の値を取得します。
		/// </summary>
		public float SquaredLength
		{
			get { return X * X + Y * Y; }
		}
		/// <summary>
		/// ベクトルの向きを表すラジアン角。
		/// </summary>
		public float Radian
		{
			get { return (float)Math.Atan2( Y, X ); }
			set
			{
				float len = Length == 0 ? Single.Epsilon*10000 : Length;
				X = (float)Math.Cos( value ) * len;
				Y = (float)Math.Sin( value ) * len;
			}
		}
		/// <summary>
		/// ベクトルの向きを表す度数。
		/// </summary>
		public float Degree
		{
			get { return Angle.RadToDeg( Radian ); }
			set { Radian = Angle.DegToRad( value ); }
		}
		public float OneRound
		{
			get { return Angle.RadToOne( Radian ); }
			set { Radian = Angle.OneToRad( value ); }
		}
		/// <summary>
		/// ベクトルの向きを表すAngleクラス。
		/// </summary>
		public Angle Angle
		{
			get { return Angle.FromRadian( Radian ); }
			set { Radian = value.Radian; }
		}
		/// <summary>
		/// このベクトルを90度回転したベクトルを取得します。
		/// </summary>
		public Vector2D Normal
		{
			get { return new Vector2D( -Y, X ); }
		}
		/// <summary>
		/// このベクトルの単位ベクトルを取得します。
		/// </summary>
		public Vector2D Unit
		{
			get
			{
				if( Length == 0 ) return new Vector2D();
				Vector2D e = new Vector2D( X, Y );
				e.Length = 1.0f;
				return e;
			}
		}

		public Vector2D LeftTop
		{
			get { return this; }
			set { Set( value.X, value.Y ); }
		}
		public Vector2D RightBottom
		{
			get { return this; }
			set { Set( value.X, value.Y ); }
		}
		#endregion

		#region メソッド
		/// <summary>
		/// ベクトルクラスを生成します。
		/// </summary>
		/// <param name="x">ベクトルのX座標の初期値。</param>
		/// <param name="y">ベクトルのY座標の初期値。</param>
		public Vector2D( float x, float y )
			: this()
		{
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// ベクトルクラスを生成します。
		/// </summary>
		/// <param name="angle">ベクトルの角度の初期値。</param>
		/// <param name="length">ベクトルの大きさの初期値。</param>
		public Vector2D( Angle angle, float length )
			: this()
		{
			this.Angle = angle;
			this.Length = length;
		}
		/// <summary>
		/// ベクトルクラスを生成します。
		/// </summary>
		/// <param name="both">ベクトルのすべての要素を初期化する値。</param>
		public Vector2D( float both )
			: this( both, both )
		{
		}

		/// <summary>
		/// このインスタンスのハッシュ コードを返します。
		/// </summary>
		/// <returns>このインスタンスのハッシュ コードである 32 ビット符号付き整数。</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		/// <summary>
		/// このインスタンスと指定したオブジェクトが等しいかどうかを示します。
		/// </summary>
		/// <param name="obj">比較対象のもう 1 つのオブジェクト。</param>
		/// <returns>obj とこのインスタンスが同じ型で、同じ値を表している場合は true。それ以外の場合は false。</returns>
		public override bool Equals( object obj )
		{
			return obj is Vector2D && (Vector2D)obj == this;
		}
		/// <summary>
		/// このベクトルの要素を文字列にして返します。
		/// </summary>
		/// <returns>"( x座標, y座標 )"といった形式でこのベクトルの値を表した文字列。</returns>
		public override string ToString()
		{
			return string.Format( "( {0}, {1} )", X, Y );
		}
		/// <summary>
		/// このベクトルの要素を文字列にして返します。
		/// </summary>
		/// <param name="format">各要素の文字列化の書式設定。</param>
		/// <returns>"( x座標, y座標 )"といった形式でこのベクトルの値を表した文字列。</returns>
		public string ToString( string format )
		{
			if( format == null )
				return ToString();
			else
				return string.Format( "( {0:"+format+"}, {1:"+format+"} )", X, Y );
		}

		/// <summary>
		/// ベクトルの値を新たに設定します。
		/// </summary>
		/// <param name="x">新しい x 要素の値。</param>
		/// <param name="y">新しい y 要素の値。</param>
		public void Set( float x, float y )
		{
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// ベクトルの値を新たに設定します。
		/// </summary>
		/// <param name="angle">新しい向きを表す Angle オブジェクト。</param>
		/// <param name="length">新しい大きさ。</param>
		public void Set( Angle angle, float length )
		{
			this.Length = length;
			this.Angle = angle;
		}
		/// <summary>
		/// ベクトルの値を、向きを表す度数と大きさによって設定します。
		/// </summary>
		/// <param name="degree">新しい向きを表す度数。</param>
		/// <param name="length">新しい大きさ。</param>
		public void SetByDeg( float degree, float length )
		{
			this.Length = length;
			this.Degree = degree;
		}
		/// <summary>
		/// ベクトルの値を、向きを表すラジアン角と大きさによって設定します。
		/// </summary>
		/// <param name="radian">新しい向きを表すラジアン角。</param>
		/// <param name="length">新しい大きさ。</param>
		public void SetByRad( float radian, float length )
		{
			this.Length = length;
			this.Radian = radian;
		}

		/// <summary>
		/// ゼロベクトルを返します。
		/// </summary>
		public static Vector2D Zero { get { return new Vector2D( 0, 0 ); } }
		public static Vector2D One { get { return new Vector2D( 1, 1 ); } }
		public static Vector2D GetRandomVector( float minX, float maxX, float minY, float maxY, Random rand = null )
		{
			if( rand == null )
				rand = new Random();

			var x = (float)rand.NextDouble() * ( maxX - minX ) + minX;
			var y = (float)rand.NextDouble() * ( maxY - minY ) + minY;
			return new Vector2D( x, y );
		}
		/// <summary>
		/// ランダムな向きと大きさのベクトルを生成します。
		/// </summary>
		/// <param name="maxLength">ベクトルの大きさの最大値。</param>
		/// <param name="machine">ベクトルの生成に利用する擬似乱数ジェネレーター。</param>
		/// <returns>生成したベクトル。</returns>
		public static Vector2D GetRandomVector( float maxLength, Random machine = null )
		{
			if( machine == null )
				machine = new Random();
			return new Vector2D( Angle.FromOneRound( (float)machine.NextDouble() ), (float)( machine.NextDouble()*2 - 1 )*maxLength );
		}
		#endregion

		#region 演算
		public static Vector2D GetDistance( Vector2D from, Vector2D to )
		{
			return new Vector2D( to.X - from.X, to.Y - from.Y );
		}

		/// <summary>
		/// 指定した２つのベクトルの内積(ドット積)を計算します。
		/// </summary>
		/// <param name="lop">内積の左オペランド。</param>
		/// <param name="rop">内積の右オペランド。</param>
		/// <returns>内積の演算結果。</returns>
		public static float Dot( Vector2D lop, Vector2D rop )
		{
			return lop.X * rop.X + lop.Y * rop.Y;
		}
		/// <summary>
		/// 指定した２つのベクトルの外積(ウェッジ積)を計算します。
		/// </summary>
		/// <param name="lop">外積の左オペランド。</param>
		/// <param name="rop">外積の右オペランド。</param>
		/// <returns>外積の演算結果。</returns>
		public static float Wedge( Vector2D lop, Vector2D rop )
		{
			return lop.X * rop.Y - lop.Y * rop.X;
		}
		/// <summary>
		/// 指定した２つのベクトルが等しいかどうかを表す真偽値を返します。
		/// </summary>
		/// <param name="left">比較する最初の値です。</param>
		/// <param name="right">比較する 2 番目の値です。</param>
		/// <returns><paramref name="left"/>と<paramref name="right"/>が等しい場合はtrue、それ以外の場合はfalseを返します。</returns>
		public static bool operator ==( Vector2D left, Vector2D right )
		{
			return left.X == right.X && left.Y == right.Y;
		}
		/// <summary>
		/// 指定した２つのベクトルが等しくないかどうかを表す真偽値を返します。
		/// </summary>
		/// <param name="lop">比較する最初の値です。</param>
		/// <param name="rop">比較する 2 番目の値です。</param>
		/// <returns><paramref name="lop"/>と<paramref name="rop"/>が等くない場合はtrue、それ以外の場合はfalseを返します。</returns>
		public static bool operator !=( Vector2D lop, Vector2D rop )
		{
			return lop.X != rop.X || lop.Y != rop.Y;
		}
		/// <summary>
		/// 指定したベクトルの逆ベクトルを求めます。
		/// </summary>
		/// <param name="op">逆ベクトルを求めるオブジェクト。</param>
		/// <returns>指定したベクトルの逆ベクトル。</returns>
		public static Vector2D operator -( Vector2D op )
		{
			return new Vector2D( -op.X, -op.Y );
		}
		/// <summary>
		/// 指定した２つのベクトルの和のベクトルを返します。
		/// </summary>
		/// <param name="lop">加算されるベクトル。</param>
		/// <param name="rop">加算するベクトル。</param>
		/// <returns>加算の結果のベクトル。</returns>
		public static Vector2D operator +( Vector2D lop, Vector2D rop )
		{
			return new Vector2D( lop.X + rop.X, lop.Y + rop.Y );
		}
		/// <summary>
		/// 指定した２つのベクトルの差のベクトルを返します。
		/// </summary>
		/// <param name="lop">減算されるベクトル。</param>
		/// <param name="rop">減算するベクトル。</param>
		/// <returns>減算の結果のベクトル。</returns>
		public static Vector2D operator -( Vector2D lop, Vector2D rop )
		{
			return new Vector2D( lop.X - rop.X, lop.Y - rop.Y );
		}
		/// <summary>
		/// 指定した２つのベクトルの各要素をかけ合わせたベクトルを返します。内積や外積ではありません。
		/// </summary>
		/// <param name="lop">乗算されるベクトル。</param>
		/// <param name="rop">乗算するベクトル。</param>
		/// <returns>乗算の結果のベクトル。</returns>
		public static Vector2D operator *( Vector2D lop, Vector2D rop )
		{
			return new Vector2D( lop.X * rop.X, lop.Y * rop.Y );
		}
		/// <summary>
		/// 指定したベクトルの各要素を、もう一つのベクトルの各要素で割ったベクトルを返します。
		/// </summary>
		/// <param name="lop">除算されるベクトル。</param>
		/// <param name="rop">除算するベクトル。</param>
		/// <returns>除算の結果のベクトル。</returns>
		public static Vector2D operator /( Vector2D lop, Vector2D rop )
		{
			return new Vector2D( lop.X / rop.X, lop.Y / rop.Y );
		}
		/// <summary>
		/// 指定したベクトルを指定した値で実数倍します。
		/// </summary>
		/// <param name="lop">実数倍されるベクトル。</param>
		/// <param name="rop">ベクトルにかける値。</param>
		/// <returns>実数倍した結果のベクトル。</returns>
		public static Vector2D operator *( Vector2D lop, float rop )
		{
			return new Vector2D( lop.X * rop, lop.Y * rop );
		}
		/// <summary>
		/// 指定したベクトルを指定した値で実数倍します。
		/// </summary>
		/// <param name="lop">ベクトルにかける値。</param>
		/// <param name="rop">実数倍されるベクトル。</param>
		/// <returns>実数倍した結果のベクトル。</returns>
		public static Vector2D operator *( float lop, Vector2D rop )
		{
			return new Vector2D( rop.X * lop, rop.Y * lop );
		}
		/// <summary>
		/// 指定したベクトルを指定した実数で割ります。
		/// </summary>
		/// <param name="lop">割られるベクトル。</param>
		/// <param name="rop">ベクトルを割る値。</param>
		/// <returns>割り算の結果のベクトル。</returns>
		public static Vector2D operator /( Vector2D lop, float rop )
		{
			return new Vector2D( lop.X / rop, lop.Y / rop );
		}
		#endregion
	}
}
