using System.IO;

namespace FoundationTigerTool.Model
{
    internal class TigerBinaryWriter : BinaryWriter
    {
        public TigerBinaryWriter(Stream stream)
            : base(stream) { }

        public void Write(TigerFile file)
        {
            Write(file.Signature);
            Write(file.Patch);
            Write(file.FileVersion);
            Write(file.NumberOfFiles);
            Write(file.Unknown1);
            Write(file.Unknown2);
            Write(file.Platform);
        }

        public void Write(TigerEntry entry)
        {
            Write(entry.Hash);
            Write(entry.Unknown);
            Write(entry.FileSize);
            Write(entry.ResourceType);
            Write(entry.Flags);
            Write(entry.Offset);
        }
    }
}