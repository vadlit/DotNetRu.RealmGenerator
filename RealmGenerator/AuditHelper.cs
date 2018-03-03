namespace RealmGenerator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using RealmGenerator.Models;

    public class AuditHelper
    {
        private static string auditPath = "C:\\Users\\User\\source\\repos\\Audit\\db";

        public static IEnumerable<SpeakerEntity> GetSpeakers()
        {
            foreach (var speakerPath in Directory.EnumerateFiles(
                Path.Combine(auditPath, "speakers"),
                "*.xml",
                SearchOption.AllDirectories))
            {
                XmlReader xReader = XmlReader.Create(speakerPath);

                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SpeakerEntity));

                yield return (SpeakerEntity)xmlSerializer.Deserialize(xReader);
            }
        }
    }
}
