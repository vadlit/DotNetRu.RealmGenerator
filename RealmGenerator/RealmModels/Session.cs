namespace RealmGenerator.RealmModels
{
    using System;
    using System.Linq;
    using Realms;

    public class Session : RealmObject
    {
        [PrimaryKey]
        public string TalkId { get; set; }

        public Talk Talk { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public Meetup Meetup { get; set; }
    }
}