namespace RealmGenerator
{
    using Realms;

    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new RealmConfiguration("C:\\Users\\User\\Desktop\\Audit.realm");

            var realm = Realm.GetInstance(config);

            // Update and persist objects with a thread-safe transaction
            realm.Write(() => { realm.Add(new Speaker { Name = "Pavel" }); });
        }
    }
}
