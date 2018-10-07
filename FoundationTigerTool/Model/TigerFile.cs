using System;
using System.Collections.Generic;

namespace FoundationTigerTool.Model
{
    public class TigerFile
    {
        public byte[] Signature;
        public UInt32 Patch;
        public UInt32 FileVersion;
        public UInt32 NumberOfFiles;
        public UInt32 Unknown1;
        public UInt32 Unknown2;
        public byte[] Platform;
        public List<TigerEntry> TigerEntries;

        public TigerFile()
        {
            TigerEntries = new List<TigerEntry>();
        }
    }

    public class TigerEntry
    {
        public UInt64 Hash;
        public UInt64 Unknown;
        public UInt32 FileSize;
        public UInt32 ResourceType;
        public UInt32 Flags;
        public UInt32 Offset;
        public string Changed;
    }
}
