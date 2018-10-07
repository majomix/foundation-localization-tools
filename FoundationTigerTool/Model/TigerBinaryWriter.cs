using System.IO;

namespace FoundationTigerTool.Model
{
    internal class TigerBinaryWriter : BinaryWriter
    {
        public TigerBinaryWriter(Stream stream)
            : base(stream) { }
    }
}