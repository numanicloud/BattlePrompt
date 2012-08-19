using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nac.Resource
{
	public class CsvWriter : IDisposable
	{
		private StreamWriter file;

		public CsvWriter( string path, bool append )
		{
			if( !File.Exists( path ) ) append = false;
			file = new StreamWriter( path, append );
		}
		public CsvWriter( string path, bool append, Encoding encoding )
		{
			if( !File.Exists( path ) ) append = false;
			file = new StreamWriter( path, append, encoding );
		}

		public void Dispose()
		{
			file.Close();
			file.Dispose();
		}
		public void WriteRecode<Type>( Type[] recode )
		{
			for( int i = 0; i < recode.Length; i++ )
			{
				file.Write( recode[i].ToString() );
				if( i != recode.Length - 1 ) file.Write( "," );
			}
			file.WriteLine();
		}
	}
}
