using System.IO;

namespace FoundationTigerTool.Model
{
    internal class TigerBinaryReader : BinaryReader
    {
        public TigerBinaryReader(FileStream fileStream)
            : base(fileStream) { }

        public TigerFile ReadTigerFile()
        {
            var file = new TigerFile();

            file.Signature = ReadBytes(4);
            file.Patch = ReadUInt32();
            file.FileVersion = ReadUInt32();
            file.NumberOfFiles = ReadUInt32();
            file.Unknown1 = ReadUInt32();
            file.Unknown2 = ReadUInt32();
            file.Platform = ReadBytes(32);

            return file;
        }

        public TigerEntry ReadTigerEntry()
        {
            var entry = new TigerEntry();

            entry.Hash = ReadUInt64();
            entry.Unknown = ReadUInt64();
            entry.FileSize = ReadUInt32();
            entry.ResourceType = ReadUInt32();
            entry.Flags = ReadUInt32();
            entry.Offset = ReadUInt32();

            return entry;
        }
    }
}