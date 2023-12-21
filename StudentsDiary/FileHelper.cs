using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public class FileHelper<T> where T : new()
    {
        private string filePath;

        public FileHelper(string filePath)
        {
            this.filePath = filePath;
        }

        public void SerializeToFile(T students)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var streamWriter = new StreamWriter(filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }

        }

        public T DeserializeFromFile()
        {
            if (!File.Exists(filePath))
            {
                return new T();
            }
            var serializer = new XmlSerializer(typeof(T));
            using (var streamreader = new StreamReader(filePath))
            {
                var studnets = (T)serializer.Deserialize(streamreader);
                streamreader.Close();
                return studnets;
            }
        }
    }
}
