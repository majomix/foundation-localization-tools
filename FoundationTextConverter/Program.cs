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
            string file = Directory.GetCurrentDirectory();

            OptionSet options = new OptionSet()
                .Add("import", value => export = false)
                .Add("file=", value => file = value);

            options.Parse(args);

            TextConverter converter = new TextConverter();

            converter.LoadDatFile(file);

            if (export)
            {
                converter.ConvertToTxt(file);
            }
            else
            {
                converter.ConvertToDat(file);
            }
        }
    }
}
