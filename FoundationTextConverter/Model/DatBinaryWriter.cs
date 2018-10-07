using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FoundationTextConverter.Model
{
    public class DatBinaryWriter : BinaryWriter
    {
        public DatBinaryWriter(FileStream fileStream, Encoding encoding)
            : base(fileStream, encoding)
        {
        }

        internal void WriteNullTerminatedString(string value)
        {
            value = value + '\0';
            Write(Encoding.UTF8.GetBytes(value));
        }

        internal void Write(DatFile file)
        {
            Write(file.Header);
            Write(file.NumberOfEntries);
            Write(file.Zeros);
        }

        internal void Write(List<EntryDescriptor> entryDescriptors)
        {
            foreach (var descriptor in entryDescriptors)
            {
                Write(descriptor.Offset);
                Write(descriptor.Zero);
            }
        }

        public void Write(Entry entry)
        {
            var compoundString = string.Join(" ", entry.Identifier, entry.Content);
            WriteNullTerminatedString(compoundString);
        }
    }
}
