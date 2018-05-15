using System.IO;
using UnityEngine;

namespace FallingSloth.SaveManager
{
    public class JSONSaveDataManager<T> : BaseSaveManager<T> where T: SaveData, new()
    {
        public JSONSaveDataManager(string filename) : base(filename) { }

        protected override void DeserialiseData(FileStream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                data = JsonUtility.FromJson<T>(json);
            }
        }

        protected override void SerialiseData(FileStream stream)
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string json = JsonUtility.ToJson(data);
                writer.Write(json);
            }
        }
    }
}
