namespace RealmGenerator
{
    using System.Linq;

    using AutoMapper;

    using RealmGenerator.Models;

    using Realms;

    public class Program
    {
        public static void Main(string[] args)
        {
            Mapper.Initialize(cfg => { cfg.CreateMap<SpeakerEntity, SpeakerModel>(); });

            var config = new RealmConfiguration("C:\\Users\\User\\Desktop\\Audit.realm");

            var realm = Realm.GetInstance(config);

            var speakers = AuditHelper.GetSpeakers().ToList();

            foreach (SpeakerEntity speakerEntity in speakers)
            {
                var speakerModel = Mapper.Map<SpeakerModel>(speakerEntity);

                realm.Write(() => { realm.Add(speakerModel); });
            }
        }
    }
}
