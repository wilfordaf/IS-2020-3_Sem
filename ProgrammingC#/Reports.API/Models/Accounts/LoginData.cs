namespace Reports.API.Models.Accounts
{
    public class LoginData
    {
        public LoginData()
        {
            
        }

        public LoginData(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsEqual(LoginData data)
        {
            return Login == data.Login && Password == data.Password;
        }
    }
}