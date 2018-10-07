using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FoundationTextConverter.Model
{
    public class TextConverter
    {
        public DatFile DatFile;

        public void LoadDatFile(string filePath)
        {
            using (DatBinaryReader reader = new DatBinaryReader(File.Open(filePath, FileMode.Open), Encoding.UTF8))
            {
                var file = reader.ReadDatFile();

                for (var i = 0; i < file.NumberOfEntries - 1; i++)
                {
                    file.EntryDescriptors.Add(reader.ReadEntryDescriptor());
                }

                for (var i = 0; i < file.NumberOfEntries - 1; i++)
                {
                    if (reader.BaseStream.Position != file.EntryDescriptors[i].Offset)
                    {
                        Console.WriteLine("Seeking from {0} to {1}", reader.BaseStream.Position, file.EntryDescriptors[i].Offset);
                        reader.BaseStream.Seek(file.EntryDescriptors[i].Offset, SeekOrigin.Begin);
                    }

                    file.EntryDescriptors[i].Entry = reader.ReadEntry();
                }

                DatFile = file;
            }
        }

        public void ConvertToTxt(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(File.Open(filePath + ".txt", FileMode.Create), Encoding.UTF8))
            {
                foreach (var entryDescriptor in DatFile.EntryDescriptors.OrderBy(descriptor => descriptor.Entry.Identifier))
                {
                    if (!string.IsNullOrEmpty(entryDescriptor.Entry.Content))
                    {
                        writer.Write(entryDescriptor.Entry.Identifier);
                        writer.Write("\t");
                        writer.WriteLine(entryDescriptor.Entry.Content.Replace("\n", "\\n").Replace("\r", "\\r"));
                    }
                }
            }
        }

        public void ConvertToDat(string file)
        {
            var dictionary = new Dictionary<string, string>();
            var lines = File.ReadAllLines(file + ".txt");

            foreach (var line in lines)
            {
                var lineParts = line.Split(new[] { '\t' }, 2);

                dictionary.Add(lineParts[0], lineParts[1].Replace("\\n", "\n").Replace("\\r", "\r"));
            }

            using (var writer = new DatBinaryWriter(File.Open(file, FileMode.Create), Encoding.UTF8))
            {
                writer.Write(DatFile);
                writer.Write(DatFile.EntryDescriptors);

                foreach (var entryDescriptor in DatFile.EntryDescriptors)
                {
                    if (entryDescriptor.Offset != 0)
                    {
                        entryDescriptor.Offset = (UInt32)writer.BaseStream.Position;

                        if (dictionary.ContainsKey(entryDescriptor.Entry.Identifier))
                        {
                            entryDescriptor.Entry.Content = dictionary[entryDescriptor.Entry.Identifier];
                        }

                        writer.Write(entryDescriptor.Entry);
                    }
                }

                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                writer.Write(DatFile);
                writer.Write(DatFile.EntryDescriptors);
            }
        }
    }
}
