using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FoundationTextConverter.Model
{
    public class DatBinaryReader : BinaryReader
    {
        public DatBinaryReader(FileStream fileStream, Encoding encoding)
            : base(fileStream, encoding)
        {
        }

        public string ReadNullTerminatedString()
        {
            List<byte> stringBytes = new List<byte>();
            int currentByte;

            while ((currentByte = ReadByte()) != 0x00)
            {
                stringBytes.Add((byte)currentByte);
            }

            return Encoding.UTF8.GetString(stringBytes.ToArray());
        }

        public DatFile ReadDatFile()
        {
            var file = new DatFile();

            file.Header = ReadUInt32();
            file.NumberOfEntries = ReadUInt32();
            file.Zeros = ReadUInt64();

            return file;
        }

        public EntryDescriptor ReadEntryDescriptor()
        {
            var descriptor = new EntryDescriptor();

            descriptor.Offset = ReadUInt32();
            descriptor.Zero = ReadUInt32();

            return descriptor;
        }

        public Entry ReadEntry()
        {
            var entry = new Entry();

            string line = ReadNullTerminatedString();
            var lines = line.Split(new[] {' '}, 2);

            if (lines.Length == 2)
            {
                entry.Identifier = lines[0];
                entry.Content = lines[1];
            }
            else
            {
                entry.Identifier = line;
            }

            return entry;
        }
    }
}
