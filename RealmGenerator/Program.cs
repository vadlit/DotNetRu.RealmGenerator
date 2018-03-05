namespace RealmGenerator
{
    using System.Collections.Generic;

    using AutoMapper;

    using RealmGenerator.Entities;
    using RealmGenerator.RealmModels;

    using Realms;

    public class Program
    {
        public static void Main(string[] args)
        {
            IList<int> dummy = new List<int>();

            var config = new RealmConfiguration("C:\\Users\\User\\Desktop\\Audit\\Audit.realm");

            var realm = Realm.GetInstance(config);

            Mapper.Initialize(
                cfg =>
                    {
                        cfg.CreateMap<SpeakerEntity, Speaker>();
                        cfg.CreateMap<VenueEntity, Venue>();
                        cfg.CreateMap<FriendEntity, Friend>();
                        cfg.CreateMap<CommunityEntity, Community>();
                        cfg.CreateMap<TalkEntity, Talk>().AfterMap(
                            (src, dest) =>
                                {
                                    foreach (string speakerId in src.SpeakerIds)
                                    {
                                        var speaker = realm.Find<Speaker>(speakerId);

                                        dest.Speakers.Add(speaker);
                                    }
                                });
                    });

            realm.AddEntities<SpeakerEntity, Speaker>("speakers");
            realm.AddEntities<FriendEntity, Friend>("friends");
            realm.AddEntities<VenueEntity, Venue>("venues");
            realm.AddEntities<CommunityEntity, Community>("communities");
            realm.AddEntities<TalkEntity, Talk>("talks");
        }
    }
}
