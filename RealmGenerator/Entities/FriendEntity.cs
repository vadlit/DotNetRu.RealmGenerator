namespace RealmGenerator.Models
{
    using System.Xml.Serialization;

    using Realms;

    [XmlType("Friend")]
    public class FriendEntity : RealmObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }
    }
}