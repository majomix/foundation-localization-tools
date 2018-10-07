using System;
using System.ComponentModel;
using System.IO;
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
            using (TigerBinaryReader reader = new TigerBinaryReader(File.Open(LoadedFilePath, FileMode.Open)))
            {
                //foreach (Entry entry in entryCollection)
                //{
                Model.ExtractFile(directory, reader);
                //CurrentProgress = (int)(currentSize * 100.0 / totalSize);
                //}
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
                    }
                }
            }
        }

        public void SaveStructure(string path)
        {
            using (TigerBinaryReader reader = new TigerBinaryReader(File.Open(LoadedFilePath, FileMode.Open)))
            {
                using (TigerBinaryWriter writer = new TigerBinaryWriter(File.Open(path, FileMode.Create)))
                {
                    //foreach (Entry entry in entries)
                    //{
                    Model.SaveDataEntry(reader, writer);
                    //CurrentProgress = (int)(currentSize * 100.0 / totalSize);
                    //CurrentFile = entry.Name;
                    //}

                    Model.SaveIndex(writer);
                }
            }

            OnPropertyChanged("Model");
        }

        public string GenerateRandomName()
        {
            Random generator = new Random();
            return Path.ChangeExtension(LoadedFilePath, @".tmp_" + generator.Next().ToString());
        }
    }
}