namespace RealmGenerator
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    using RealmGenerator.Entities;

    public class AuditHelper
    {
        private static string auditPath = "C:\\Users\\User\\source\\repos\\Audit\\db";

        public static T LoadFromFile<T>(string path)
        {
            XmlReader xmlReader = XmlReader.Create(path);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            return (T)xmlSerializer.Deserialize(xmlReader);
        }

        public static IEnumerable<SpeakerEntity> GetSpeakers()
        {
            foreach (var speakerPath in Directory.EnumerateFiles(
                Path.Combine(auditPath, "speakers"),
                "*.xml",
                SearchOption.AllDirectories))
            {
                yield return LoadFromFile<SpeakerEntity>(speakerPath);
            }
        }

        public static IEnumerable<VenueEntity> GetVenues()
        {
            foreach (var venuePath in Directory.EnumerateFiles(
                Path.Combine(auditPath, "venues"),
                "*.xml",
                SearchOption.AllDirectories))
            {
                yield return LoadFromFile<VenueEntity>(venuePath);
            }
        }
    }
}
