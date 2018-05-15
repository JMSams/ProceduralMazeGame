using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FallingSloth.SaveManager
{
    public class BinarySaveDataManager<T> : BaseSaveManager<T> where T : SaveData, new()
    {
        BinaryFormatter formatter;

        public BinarySaveDataManager(string filename) : base(filename) { }

        protected override void DeserialiseData(FileStream stream)
        {
            if (formatter == null) formatter = new BinaryFormatter();

            data = (T)formatter.Deserialize(stream);
        }

        protected override void SerialiseData(FileStream stream)
        {
            if (formatter == null) formatter = new BinaryFormatter();

            formatter.Serialize(stream, data);
        }
    }
}
