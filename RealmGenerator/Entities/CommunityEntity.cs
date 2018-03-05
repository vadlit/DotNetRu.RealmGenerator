namespace RealmGenerator.Entities
{
    using System.Xml.Serialization;

    [XmlType("Community")]
    public class CommunityEntity
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}