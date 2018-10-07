using System;
using System.IO;
using FoundationTextConverter.Model;
using NDesk.Options;

namespace FoundationTextConverter
{
    public class Program
    {
        static void Main(string[] args)
        {
            bool export = true;
            string directory = Directory.GetCurrentDirectory();

            OptionSet options = new OptionSet()
                .Add("import", value => export = false)
                .Add("dir=", value => directory = value);

            options.Parse(args);

            TextConverter converter = new TextConverter();

            if (export)
            {
            }
            else
            {
            }
        }
    }
}
