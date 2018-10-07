using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using FoundationTigerTool.Model;

namespace FoundationTigerTool.ViewModels
{
    internal abstract class BaseViewModel : INotifyPropertyChanged
    {
        private int _currentProgress = 100;
        private string _loadedFilePath;
        private string _currentFile;
        private bool _hasError;

        public TigerEditor Model { get; protected set; }
        public string LoadedFilePath
        {
            get => _loadedFilePath;
            set
            {
                if (_loadedFilePath != value)
                {
                    _loadedFilePath = value;
                    OnPropertyChanged("LoadedFilePath");
                }
            }
        }
        public string CurrentFile
        {
            get { return _currentFile; }
            protected set
            {
                if (_currentFile != value)
                {
                    _currentFile = value;
                    OnPropertyChanged("CurrentFile");
                }
            }
        }
        public int CurrentProgress
        {
            get { return _currentProgress; }
            protected set
            {
                if (_currentProgress != value)
                {
                    _currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }
        public bool HasError
        {
            get { return _hasError; }
            set
            {
                if (_hasError != value)
                {
                    _hasError = value;
                    OnPropertyChanged("HasError");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler RequestClose;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnRequestClose(EventArgs e)
        {
            RequestClose?.Invoke(this, e);
        }

        public void LoadStructure()
        {
            using (TigerBinaryReader reader = new TigerBinaryReader(File.Open(LoadedFilePath, FileMode.Open)))
            {
                Model.LoadFileStructure(reader);
                OnPropertyChanged(nameof(Model));
            }
        }

        public void ExtractFile(string directory)
        {
            var totalSize = Model.TigerFile.TigerEntries.Sum(entry => entry.FileSize);
            uint currentSize = 0;

            using (TigerBinaryReader reader = new TigerBinaryReader(File.Open(LoadedFilePath, FileMode.Open)))
            {
                foreach (TigerEntry entry in Model.TigerFile.TigerEntries)
                {
                    Model.ExtractFile(entry, directory, reader);
                    CurrentProgress = (int) (currentSize * 100.0 / totalSize);
                    currentSize += entry.FileSize;
                }
            }
        }

        public void ResolveNewFiles(string directory)
        {
            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                string[] tokens = file.Split(new[] { directory + @"\" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string token in tokens)
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        var parsed = UInt64.TryParse(token, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hash);

                        if (parsed)
                        {
                            var entry = Model.TigerFile.TigerEntries.SingleOrDefault(_ => _.Hash == hash);

                            if (entry != null)
                            {
                                entry.Changed = file;
                            }
                        }
                    }
                }
            }
        }

        public void SaveStructure()
        {
            using (TigerBinaryWriter writer = new TigerBinaryWriter(File.Open(LoadedFilePath, FileMode.Open, FileAccess.ReadWrite)))
            {
                long currentSize = 0;
                var entries = Model.TigerFile.TigerEntries.Where(_ => _.Changed != null).ToList();
                long totalSize = entries.Sum(_ => _.FileSize);

                foreach (TigerEntry entry in entries)
                {
                    Model.AppendDataEntry(writer, entry);
                    CurrentProgress = (int)(currentSize * 100.0 / totalSize);
                    CurrentFile = entry.Hash.ToString();
                    currentSize += entry.FileSize;
                }

                Model.UpdateTigerFileStructure(writer);
            }

            OnPropertyChanged("Model");
        }

        public string GenerateRandomName()
        {
            Random generator = new Random();
            return Path.ChangeExtension(LoadedFilePath, @".tmp_" + generator.Next());
        }
    }
}