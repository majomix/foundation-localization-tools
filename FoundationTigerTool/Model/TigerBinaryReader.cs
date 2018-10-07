using System.IO;

namespace FoundationTigerTool.Model
{
    internal class TigerBinaryReader : BinaryReader
    {
        public TigerBinaryReader(FileStream fileStream)
            : base(fileStream) { }
    }
}