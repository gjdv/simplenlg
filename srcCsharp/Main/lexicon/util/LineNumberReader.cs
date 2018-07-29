/*
 * Ported to C# by Gert-Jan de Vries
 */

using System;
using System.IO;

namespace SimpleNLG.Main.lexicon.util
{
    // taken from https://stackoverflow.com/questions/4180475/is-there-a-c-sharp-equivalent-of-javas-linenumberreader
    class LineNumberReader : TextReader
    {
        private readonly TextReader _reader;
        private int _b;
        private int _line;

        public LineNumberReader(TextReader reader)
        {
            _reader = reader ?? throw new ArgumentNullException("Reader not set in constructor of LineNumberReader");
        }

        public int Line => _line;

        public override int Peek()
        {
            return _reader.Peek();
        }

        public override int Read()
        {
            int b = _reader.Read();
            if ((_b == '\n') || (_b == '\r' && b != '\n')) _line++;
            return _b = b;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _reader.Dispose();
        }
    }
}
