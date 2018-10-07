using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using FoundationTigerTool.Model;
using FoundationTigerTool.ViewModels.Commands;
using NDesk.Options;

namespace FoundationTigerTool.ViewModels
{
    internal class OneTimeRunViewModel : BaseViewModel
    {
        private string _targetDirectory;
        public bool? Export { get; set; }
        public ICommand ExtractByParameterCommand { get; private set; }
        public ICommand ImportByParameterCommand { get; private set; }

        public OneTimeRunViewModel()
        {
            ParseCommandLine();
            Model = new TigerEditor();

            ImportByParameterCommand = new ImportByParameterCommand();
            ExtractByParameterCommand = new ExtractByParameterCommand();
        }

        public void ParseCommandLine()
        {
            OptionSet options = new OptionSet()
                .Add("export", value => Export = true)
                .Add("import", value => Export = false)
                .Add("tiger=", value => LoadedFilePath = CreateFullPath(value, false))
                .Add("dir=", value => _targetDirectory = CreateFullPath(value, true));

            options.Parse(Environment.GetCommandLineArgs());
        }

        public void Extract()
        {
            if (_targetDirectory != null && LoadedFilePath != null)
            {
                LoadStructure();
                ExtractFile(_targetDirectory);
            }
        }

        public void Import()
        {
            if (_targetDirectory != null && Directory.Exists(_targetDirectory) && LoadedFilePath != null)
            {
                LoadStructure();
                ResolveNewFiles(_targetDirectory);
                SaveStructure();
            }
        }

        private string CreateFullPath(string path, bool isDirectory)
        {
            if (!String.IsNullOrEmpty(path) && !path.Contains(':'))
            {
                path = Directory.GetCurrentDirectory() + @"\" + path.Replace('/', '\\');
            }

            return (isDirectory || File.Exists(path)) ? path : null;
        }
    }
}
