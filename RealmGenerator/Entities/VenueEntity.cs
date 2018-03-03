namespace RealmGenerator.Models
{
    using System.Xml.Serialization;

    using Realms;

    [XmlType("Venue")]
    public class VenueEntity : RealmObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string MapUrl { get; set; }
    }
}