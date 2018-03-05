namespace RealmGenerator
{
    using Realms;

    using Mapper = AutoMapper.Mapper;

    public static class RealmExtensions
    {
        public static void AddEntities<TEntity, TRealmType>(this Realm realm, string folderPath)
            where TRealmType : RealmObject
        {
            foreach (TEntity entity in AuditHelper.GetEntities<TEntity>(folderPath))
            {
                var realmObject = Mapper.Map<TRealmType>(entity);

                realm.Write(() => { realm.Add(realmObject); });
            }
        }
    }
}
