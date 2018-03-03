namespace RealmGenerator.Models
{
    using System.Xml.Serialization;

    using Realms;

    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class CommunityEntity : RealmObject
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}