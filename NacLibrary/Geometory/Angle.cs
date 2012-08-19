using System;

namespace Nac.Geometory
{
	/// <summary>
	/// 角度を表す構造体。
	/// </summary>
	public struct Angle
	{
		private float oneRound_;

		/// <summary>
		/// 角度を自動的に [0, 2π] に収めるかどうかの真偽値を取得または設定します。
		/// </summary>
		public bool DoWrap { get; set; }

		public float OneRound
		{
			get { return oneRound_; }
			set
			{
				oneRound_ = value;
				if( DoWrap )
					Wrap();
			}
		}
		public OneRound OneRoundAngle
		{
			get { return new OneRound( OneRound ); }
			set { OneRound = value.Value; }
		}
		/// <summary>
		/// 角度を度数法を用いて取得または設定します。
		/// </summary>
		public float Degree
		{
			get { return OneToDeg( oneRound_ ); }
			set { OneRound = DegToOne( value ); }
		}
		/// <summary>
		/// 角度を弧度法を用いて取得または設定します。
		/// </summary>
		public float Radian
		{
			get { return OneToRad( oneRound_ ); }
			set { OneRound = RadToOne( value ); }
		}

		public static Angle FromOneRound( float oneRound, bool doAutoWrap = true )
		{
			return new Angle { DoWrap = doAutoWrap, OneRound = oneRound };
		}
		public static Angle FromDegree( float degree, bool doAutoWrap = true )
		{
			return new Angle { DoWrap = doAutoWrap, Degree = degree };
		}
		public static Angle FromRadian( float radian, bool doAutoWrap = true )
		{
			return new Angle { DoWrap = doAutoWrap, Radian = radian };
		}

		#region メソッド
		/// <summary>
		/// 角度を 0度 ～ 360度のうちに収めます。
		/// </summary>
		public void Wrap()
		{
			while( Math.Abs( oneRound_ ) > 1.0 )
				oneRound_ -= Math.Sign( oneRound_ ) * 1.0f;
		}

		public static Angle Asin( float d )
		{
			if( d < -1 || d > 1 )
				throw new ArgumentOutOfRangeException( "Asinの引数'd'は -1≦d≦1 です" );
			return Angle.FromRadian( (float)Math.Asin( d ) );
		}
		public static Angle Acos( float d )
		{
			if( d < -1 || d > 1 )
				throw new ArgumentOutOfRangeException( "Acosの引数'd'は -1≦d≦1 です" );
			return Angle.FromRadian( (float)Math.Acos( d ) );
		}
		public static Angle Atan( float y, float x )
		{
			return Angle.FromRadian( (float)Math.Atan2( y, x ) );
		}

		public static float OneToDeg( float oneRound )
		{
			return oneRound * 360;
		}
		public static float OneToRad( float oneRound )
		{
			return oneRound * (float)Math.PI * 2;
		}
		public static float DegToOne( float degree )
		{
			return degree / 360;
		}
		public static float DegToRad( float degree )
		{
			return degree * (float)Math.PI / 180;
		}
		public static float RadToOne( float radian )
		{
			return radian / (float)Math.PI / 2;
		}
		public static float RadToDeg( float radian )
		{
			return radian / (float)Math.PI * 180;
		}

		public static Angle GetRandomAngle()
		{
			Random random = new Random();
			return Angle.FromOneRound( (float)random.NextDouble() );
		}
		public static Angle GetRandomAngle( Random random )
		{
			return Angle.FromOneRound( (float)random.NextDouble() );
		}
		#endregion

		#region 演算
		public static Angle operator -( Angle op )
		{
			return FromDegree( -op.Degree );
		}
		public static Angle operator +( Angle lop, Angle rop )
		{
			return FromDegree( lop.Degree + rop.Degree );
		}
		public static Angle operator -( Angle lop, Angle rop )
		{
			return FromDegree( lop.Degree - rop.Degree );
		}
		public static Angle operator *( Angle lop, float rop )
		{
			return FromDegree( lop.Degree * rop );
		}
		public static Angle operator *( float lop, Angle rop )
		{
			return FromDegree( lop * rop.Degree );
		}
		public static Angle operator /( Angle lop, float rop )
		{
			return FromDegree( lop.Degree / rop );
		}
		#endregion
	}

	public struct OneRound
	{
		float value_;
		public float Value
		{
			get { return value_; }
			set
			{
				value_ = value;
				if( DoAutoWrap ) Wrap();
			}
		}
		public Angle Angle
		{
			get { return Angle.FromOneRound( value_ ); }
			set { value_ = value.OneRound; }
		}
		public bool DoAutoWrap { get; set; }

		public OneRound( float oneRound, bool doAutoWrap = true )
			: this()
		{
			this.DoAutoWrap = doAutoWrap;
			Value = oneRound;
		}
		public void Wrap()
		{
			value_ %= 1.0f;
		}

		public static OneRound operator -( OneRound op )
		{
			return new OneRound( -op.Value );
		}
		public static OneRound operator +( OneRound lop, OneRound rop )
		{
			return new OneRound( lop.Value + rop.Value );
		}
		public static OneRound operator -( OneRound lop, OneRound rop )
		{
			return new OneRound( lop.Value - rop.Value );
		}
		public static OneRound operator *( OneRound lop, float rop )
		{
			return new OneRound( lop.Value * rop );
		}
		public static OneRound operator *( float lop, OneRound rop )
		{
			return new OneRound( lop * rop.Value );
		}
		public static OneRound operator /( OneRound lop, float rop )
		{
			return new OneRound( lop.Value / rop );
		}
		public static implicit operator OneRound( float op )
		{
			return new OneRound( op );
		}
		public static implicit operator Angle( OneRound op )
		{
			return Angle.FromOneRound( op.Value );
		}
	}
}
