using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nac.Resource
{
	public class ParamaterReader
	{
		public Dictionary<string, string> ParamString { get; private set; }
		public Dictionary<string, decimal> ParamDecimal { get; private set; }
		public ParamaterReader( string path )
		{
			Load( new CsvReader( path ) );
		}
		public ParamaterReader( string path, Encoding encoding )
		{
			Load( new CsvReader( path, encoding ) );
		}
		private void Load( CsvReader reader )
		{
			ParamString = new Dictionary<string, string>();
			ParamDecimal = new Dictionary<string, decimal>();
			using( var file = reader )
			{
				foreach( var item in file.GetRecodes() )
				{
					decimal result;
					if( decimal.TryParse( item[1], out result ) )
						ParamDecimal.Add( item[0], result );
					else
						ParamString.Add( item[0], item[1] );
				}
			}
		}
	}
}
