using System.Text;
using System.IO;
using System;

namespace Nac.Helpers
{
	public static class StreamHelper
	{
		public static readonly Encoding sjis = Encoding.GetEncoding( "Shift-Jis" );

		public static string AddIndex( string path, int maxIndex = 100 )
		{
			FileInfo f = new FileInfo( path );
			path = path.Remove( path.IndexOf( f.Extension ) );
			for( int i = 0; i < maxIndex; i++ )
			{
				string fileName = path + i.ToString() + f.Extension;
				if( !File.Exists( fileName ) ) return fileName;
			}
			throw new ApplicationException();
		}
	}
}
