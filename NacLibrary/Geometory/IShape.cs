using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nac.Geometory
{
	/// <summary>
	/// 図形を表すインターフェース。
	/// </summary>
	public interface IShape
	{
		/// <summary>
		/// 図形を囲む最小の矩形の左上の座標を取得します。
		/// </summary>
		Vector2D LeftTop { get; set; }
		/// <summary>
		/// 図形を囲む最小の矩形の右下の座標を取得します。
		/// </summary>
		Vector2D RightBottom { get; set; }
	}
}
