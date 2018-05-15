using System.IO;
using UnityEngine;

namespace FallingSloth.SaveManager
{
    public abstract class BaseSaveManager<T> where T: SaveData, new()
    {
        protected string dataPath
        {
            get
            {
                return Application.persistentDataPath + "/" + filename;
            }
        }

        protected string filename = "save.sav";
        
        public BaseSaveManager(string filename)
        {
            this.filename = filename;
        }

        T _data;
        protected T data
        {
            get
            {
                if (_data == null) LoadData();
                return _data;
            }
            set
            {
                _data = value;
            }
        }

        private void LoadData()
        {
            if (File.Exists(dataPath))
            {
                using (FileStream stream = new FileStream(dataPath, FileMode.Open))
                {
                    try
                    {
                        DeserialiseData(stream);
                    }
                    catch
                    {
                        data = new T();
                    }
                }
            }
            else
            {
                data = new T();
            }
        }

        public void SaveData()
        {
            FileMode mode = (File.Exists(dataPath)) ? FileMode.Open : FileMode.Create;
            using (FileStream stream = new FileStream(dataPath, mode))
            {
                SerialiseData(stream);
            }
        }

        protected abstract void SerialiseData(FileStream stream);
        protected abstract void DeserialiseData(FileStream stream);
    }
}
