using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nac.Helpers;

namespace Nac
{
	public class ClampedValue<T> : ICloneable where T : IComparable, IComparable<T>
	{
		private T max_;
		public T Max
		{
			get { return max_; }
			set
			{
				if( max_.CompareTo( value ) != 0 )
				{
					OnMaxChanging.TryInvoke( value );
					max_ = value;
					value_ = MathHelper.Ceiling( value_, max_ );
				}
			}
		}

		private T min_;
		public T Min
		{
			get { return min_; }
			set
			{
				if( min_.CompareTo( value ) != 0 )
				{
					OnMinChanging.TryInvoke( value );
					min_ = value;
					value_ = MathHelper.Floor( value_, min_ );
				}
			}
		}

		private T value_;
		public T Value
		{
			get { return value_; }
			set
			{
				if( value_.CompareTo( Min ) == 0 && value.CompareTo( Min ) < 0 ) return;
				if( value_.CompareTo( Max ) == 0 && value.CompareTo( Max ) > 0 ) return;
				OnValueChanging.TryInvoke( value );
				value_ = MathHelper.Clamp( value, Min, Max );
			}
		}

		public bool Loop { get; set; }
		public event Action<T> OnMaxChanging;
		public event Action<T> OnMinChanging;
		public event Action<T> OnValueChanging;

		public ClampedValue( T value, T min, T max )
		{
			this.Max = max;
			this.Min = min;
			this.Value = value;
		}
		public object Clone()
		{
			var obj = new ClampedValue<T>( Value, Min, Max );
			obj.OnMaxChanging = OnMaxChanging;
			obj.OnMinChanging = OnMinChanging;
			obj.OnValueChanging = OnValueChanging;
			return obj;
		}
		public override string ToString()
		{
			return string.Format( "{0} ∈ [{1}, {2}]", Value, Min, Max );
		}

		public static implicit operator T( ClampedValue<T> op )
		{
			return op.Value;
		}
	}
}
