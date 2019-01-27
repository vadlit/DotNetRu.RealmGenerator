using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AutoMapper;

using LibGit2Sharp;

using RealmGenerator.Entities;
using RealmGenerator.RealmModels;
using Realms;

namespace RealmGenerator
{    
    public class Program
    {
        public static void Main(string[] args)
        {
            IList<int> dummy = new List<int>();

            var checkoutMaster = true;
            var commitHash = "45d858e17a2f804bfd3691f260531638758508cd";

            var auditRepoPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "Audit");
            var filesLoader = new AuditFilesLoader(auditRepoPath);

            Console.WriteLine("Downloading audit from Git...");            
            Repository.Clone("https://github.com/DotNetRu/Audit.git", auditRepoPath);

            Console.WriteLine("Checking out...");
            var auditVersion = new AuditVersion();

            using (var auditRepo = new Repository(auditRepoPath))
            {
                if (checkoutMaster)
                {
                    auditVersion.CommitHash = auditRepo.Head.Commits.First().Sha;
                }
                else
                {
                    auditVersion.CommitHash = commitHash;
                }

                var commit = auditRepo.Commits.Single(x => x.Sha == auditVersion.CommitHash);
                Commands.Checkout(auditRepo, commit);
            }            

            Console.WriteLine("Generating realm for {0}...", auditVersion.CommitHash);
            string realmDirectoryPath = $@"C:\Users\{Environment.UserName}\Source\Repos\App\DotNetRu.DataStore.Audit";
            Directory.CreateDirectory(realmDirectoryPath);
            CleanDirectory(realmDirectoryPath);

            var config = new RealmConfiguration(Path.Combine(realmDirectoryPath, "Audit.realm"));

            var realm = Realm.GetInstance(config);

            realm.Write(() => { realm.Add(auditVersion); });

            InitializeAudoMapper(realm, filesLoader);

            realm.AddEntities<SpeakerEntity, Speaker>(filesLoader, "speakers");
            realm.AddEntities<FriendEntity, Friend>(filesLoader, "friends");
            realm.AddEntities<VenueEntity, Venue>(filesLoader, "venues");
            realm.AddEntities<CommunityEntity, Community>(filesLoader, "communities");
            realm.AddEntities<TalkEntity, Talk>(filesLoader, "talks");
            realm.AddEntities<MeetupEntity, Meetup>(filesLoader, "meetups");

            Console.WriteLine("Finished! Realm generated at {0}. Press any key to exit.", realmDirectoryPath);
            Console.ReadKey();
        }

        private static void InitializeAudoMapper(Realm realm, AuditFilesLoader filesLoader)
        {
            Mapper.Initialize(
                cfg =>
                {
                    cfg.CreateMap<SpeakerEntity, Speaker>().AfterMap(
                        (src, dest) =>
                        {
                            dest.Avatar = filesLoader.LoadImage("speakers", src.Id, "avatar.jpg");
                        });
                    cfg.CreateMap<VenueEntity, Venue>();
                    cfg.CreateMap<FriendEntity, Friend>().AfterMap(
                        (src, dest) =>
                        {
                            var friendId = src.Id;

                            dest.LogoSmall = filesLoader.LoadImage("friends", friendId, "logo.small.png");
                            dest.Logo = filesLoader.LoadImage("friends", friendId, "logo.png");
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

                            if (src.SeeAlsoTalkIds != null)
                            {
                                foreach (string talkId in src.SeeAlsoTalkIds)
                                {                                    
                                    dest.SeeAlsoTalksIds.Add(talkId);
                                }
                            }
                        });
                    cfg.CreateMap<SessionEntity, Session>().AfterMap(
                        (src, dest) =>
                            {
                                dest.Talk = realm.Find<Talk>(src.TalkId);
                            });
                    cfg.CreateMap<MeetupEntity, Meetup>()
                        .ForMember(
                            dest => dest.Sessions,
                            o => o.MapFrom((src, dest, sessions, context) => context.Mapper.Map(src.Sessions, dest.Sessions)))
                        .AfterMap(
                            (src, dest) =>
                                {
                                    foreach (string friendId in src.FriendIds)
                                    {
                                        var friend = realm.Find<Friend>(friendId);
                                        dest.Friends.Add(friend);
                                    }

                                    dest.Venue = realm.Find<Venue>(src.VenueId);
                                });
                });
        }

        private static void CleanDirectory(string realmDirectoryPath)
        {
            File.Delete(Path.Combine(realmDirectoryPath, "Audit.realm"));
            File.Delete(Path.Combine(realmDirectoryPath, "Audit.realm.lock"));
        }
    }
}
