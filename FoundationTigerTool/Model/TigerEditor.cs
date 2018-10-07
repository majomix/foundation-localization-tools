using System;
using System.IO;

namespace FoundationTigerTool.Model
{
    internal class TigerEditor
    {
        public TigerFile TigerFile;

        public void LoadFileStructure(TigerBinaryReader reader)
        {
            TigerFile = reader.ReadTigerFile();

            for (var i = 0; i < TigerFile.NumberOfFiles; i++)
            {
                TigerFile.TigerEntries.Add(reader.ReadTigerEntry());
            }
        }

        public void ExtractFile(TigerEntry entry, string directory, TigerBinaryReader reader)
        {
            reader.BaseStream.Seek(entry.Offset, SeekOrigin.Begin);
            var content = reader.ReadBytes((int) entry.FileSize);

            string compoundName = directory + @"\" + entry.Hash.ToString("X16");
            Directory.CreateDirectory(Path.GetDirectoryName(compoundName) ?? throw new InvalidOperationException());

            using (BinaryWriter writer = new BinaryWriter(File.Open(compoundName, FileMode.Create)))
            {
                writer.Write(content);
            }
        }

        public void AppendDataEntry(TigerBinaryWriter writer, TigerEntry entry)
        {
            writer.BaseStream.Seek(0, SeekOrigin.End);
            entry.Offset = (uint) writer.BaseStream.Position;

            var size = (int)new FileInfo(entry.Changed).Length;
            entry.FileSize = (uint)size;

            using (BinaryReader importReader = new BinaryReader(File.Open(entry.Changed, FileMode.Open)))
            {
                writer.Write(importReader.ReadBytes(size));
            }
        }

        public void UpdateTigerFileStructure(TigerBinaryWriter writer)
        {
            writer.BaseStream.Seek(0, SeekOrigin.Begin);

            writer.Write(TigerFile);

            foreach (var entry in TigerFile.TigerEntries)
            {
                writer.Write(entry);
            }
        }
    }
}