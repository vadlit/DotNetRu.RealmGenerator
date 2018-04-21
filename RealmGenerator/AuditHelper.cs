namespace RealmGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class AuditHelper
    {
        public static readonly string AuditPath = $@"C:\Users\{Environment.UserName}\source\repos\Audit";

        public static T LoadFromFile<T>(string path)
        {
            XmlReader xmlReader = XmlReader.Create(path);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            return (T)xmlSerializer.Deserialize(xmlReader);
        }

        public static IEnumerable<T> GetEntities<T>(string folderName)
        {
            foreach (var filePath in Directory.EnumerateFiles(
                Path.Combine(AuditPath, "db", folderName),
                "*.xml",
                SearchOption.AllDirectories))
            {
                yield return LoadFromFile<T>(filePath);
            }
        }

        public static byte[] LoadImage(string directoryName, string entityId, string fileName)
        {
            return File.ReadAllBytes(Path.Combine(AuditPath, "db", directoryName, entityId, fileName));
        }
    }
}
