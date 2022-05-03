using System;

namespace CRMSystem.Settings.Entities
{
    public class User
    {
        public string Login { get; }
        public string Password { get; }

        public UserStoreType StoreType { get; }



        public User(string login, string password,
            UserStoreType storeType)
        {
            Login = login;
            Password = password;

            StoreType = storeType;
        }



        public bool IsTemporary()
        {
            return StoreType == UserStoreType.Temporary;
        }
    }
}
