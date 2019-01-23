using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RealmGenerator
{    
    public class AuditFilesLoader
    {        
        public AuditFilesLoader(string auditDirectoryPath)
        {
            AuditDirectoryPath = auditDirectoryPath;
        }

        public string AuditDirectoryPath { get; }

        public static T LoadFromFile<T>(string path)
        {
            XmlReader xmlReader = XmlReader.Create(path);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            return (T)xmlSerializer.Deserialize(xmlReader);
        }

        public IEnumerable<T> GetEntities<T>(string folderName)
        {
            foreach (var filePath in Directory.EnumerateFiles(
                Path.Combine(AuditDirectoryPath, "db", folderName),
                "*.xml",
                SearchOption.AllDirectories))
            {
                yield return LoadFromFile<T>(filePath);
            }
        }

        public byte[] LoadImage(string directoryName, string entityId, string fileName)
        {
            var imagePath = Path.Combine(AuditDirectoryPath, "db", directoryName, entityId, fileName);
            if (!File.Exists(imagePath))
            {
                return null;
            }
            return File.ReadAllBytes(imagePath);
        }
    }
}
