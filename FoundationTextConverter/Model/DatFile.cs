using System;
using System.Collections.Generic;

namespace FoundationTextConverter.Model
{
    public class DatFile
    {
        public UInt32 Header;
        public UInt32 NumberOfEntries;
        public UInt64 Zeros;
        public List<EntryDescriptor> EntryDescriptors;

        public DatFile()
        {
            EntryDescriptors = new List<EntryDescriptor>();
        }
    }

    public class EntryDescriptor
    {
        public UInt32 Offset;
        public UInt32 Zero;
        public Entry Entry;
    }

    public class Entry
    {
        public string Identifier;
        public string Content;
    }
}
