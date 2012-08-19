using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Nac.Resource
{
	public class CsvReader : IDisposable
	{
		private TextFieldParser file;
		public FileInfo Info { get; private set; }
		public bool IsEOF
		{
			get { return file.EndOfData; }
		}
		public bool Trim
		{
			get { return file.TrimWhiteSpace; }
			set { file.TrimWhiteSpace = value; }
		}

		public CsvReader( string path )
		{
			Info = new FileInfo( path );
			file = new TextFieldParser( path );
			file.TextFieldType = FieldType.Delimited;
			file.Delimiters = new string[] { "," };
		}
		public CsvReader( string path, Encoding encoding )
		{
			Info = new FileInfo( path );
			file = new TextFieldParser( path, encoding );
			file.TextFieldType = FieldType.Delimited;
			file.Delimiters = new string[] { "," };
		}

		public void Initialize()
		{
			bool trim = this.Trim;
			file.Dispose();
			file = new TextFieldParser( Info.FullName );
			file.TextFieldType = FieldType.Delimited;
			file.Delimiters = new string[] { "," };
			file.TrimWhiteSpace = trim;
		}
		public void Dispose()
		{
			file.Close();
			file.Dispose();
		}
		public string[] ReadRecode()
		{
			return file.ReadFields();
		}
		public IEnumerable<string[]> GetRecodes()
		{
			while( !IsEOF )
				yield return file.ReadFields();
		}

		public string RelPath( string path )
		{
			return Info.DirectoryName + @"\" + path;
		}
	}
}
