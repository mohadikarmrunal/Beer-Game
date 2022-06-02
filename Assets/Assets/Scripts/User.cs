namespace Assets.Scripts
{
    [System.Serializable]
    public class User
    {
        public string authToken;
        public string id;
        public string email;
        public string username;
        public string name;

        public User(string authToken, string id, string email, string username, string name)
        {
            this.authToken = authToken;
            this.id = id;
            this.email = email;
            this.username = username;
            this.name = name;
        }
    }
}
