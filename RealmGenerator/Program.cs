namespace RealmGenerator
{
    using System.Linq;

    using AutoMapper;

    using RealmGenerator.Entities;
    using RealmGenerator.RealmModels;

    using Realms;

    public class Program
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(
                cfg =>
                    {
                        cfg.CreateMap<SpeakerEntity, Speaker>();
                        cfg.CreateMap<VenueEntity, Venue>();
                    });

            var config = new RealmConfiguration("C:\\Users\\User\\Desktop\\Audit.realm");

            var realm = Realm.GetInstance(config);

            foreach (SpeakerEntity speakerEntity in AuditHelper.GetSpeakers())
            {
                var speaker = Mapper.Map<Speaker>(speakerEntity);

                realm.Write(() => { realm.Add(speaker); });
            }

            foreach (VenueEntity venueEntity in AuditHelper.GetVenues())
            {
                var venue = Mapper.Map<Venue>(venueEntity);

                realm.Write(() => { realm.Add(venue); });
            }
        }
    }
}
