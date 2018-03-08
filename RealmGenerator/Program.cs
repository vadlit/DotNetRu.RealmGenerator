﻿namespace RealmGenerator
{
    using System.Collections.Generic;
    using System.IO;

    using AutoMapper;

    using RealmGenerator.Entities;
    using RealmGenerator.RealmModels;

    using Realms;

    public class Program
    {
        public static void Main(string[] args)
        {
            IList<int> dummy = new List<int>();

            string realmDirectoryPath = @"C:\Users\User\Source\Repos\App\DotNetRu.DataStore.Audit";

            CleanDirectory(realmDirectoryPath);

            var config = new RealmConfiguration(Path.Combine(realmDirectoryPath, "Audit.realm"));

            var realm = Realm.GetInstance(config);

            Mapper.Initialize(
                cfg =>
                    {
                        cfg.CreateMap<SpeakerEntity, Speaker>().AfterMap(
                            (src, dest) =>
                                {
                                    var speakerId = src.Id;

                                    dest.AvatarSmall = AuditHelper.LoadImage("speakers", speakerId, "avatar.small.jpg");
                                    dest.Avatar = AuditHelper.LoadImage("speakers", speakerId, "avatar.jpg");
                                });
                        cfg.CreateMap<VenueEntity, Venue>();
                        cfg.CreateMap<FriendEntity, Friend>().AfterMap(
                            (src, dest) =>
                                {
                                    var friendId = src.Id;

                                    dest.LogoSmall = AuditHelper.LoadImage("friends", friendId, "logo.small.png");
                                    dest.Logo = AuditHelper.LoadImage("friends", friendId, "logo.png");
                                });
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
                        cfg.CreateMap<MeetupEntity, Meetup>().AfterMap(
                            (src, dest) =>
                                {
                                    foreach (string talkId in src.TalkIds)
                                    {
                                        var talk = realm.Find<Talk>(talkId);
                                        dest.Talks.Add(talk);
                                    }

                                    foreach (string friendId in src.FriendIds)
                                    {
                                        var friend = realm.Find<Friend>(friendId);
                                        dest.Friends.Add(friend);
                                    }
                                });
                    });

            realm.AddEntities<SpeakerEntity, Speaker>("speakers");
            realm.AddEntities<FriendEntity, Friend>("friends");
            realm.AddEntities<VenueEntity, Venue>("venues");
            realm.AddEntities<CommunityEntity, Community>("communities");
            realm.AddEntities<TalkEntity, Talk>("talks");
            realm.AddEntities<MeetupEntity, Meetup>("meetups");
        }

        private static void CleanDirectory(string realmDirectoryPath)
        {
            File.Delete(Path.Combine(realmDirectoryPath, "Audit.realm"));
            File.Delete(Path.Combine(realmDirectoryPath, "Audit.realm.lock"));
        }
    }
}
