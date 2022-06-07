using Reports.API.Tools;
using System.Collections.Generic;
using System.Linq;
using Reports.API.Models.Accounts;
using Reports.API.Models.Members;

namespace Reports.DB.Database
{
    public class Database
    {
        private static Database _database;

        private Database() { }

        public static Dictionary<LoginData, StaffMemberAccount> Data { get; set; }

        public static Database GetDatabase()
        {
            return _database ??= new Database();
        }

        public static StaffMemberAccount AddStaffMemberAccount(string login, string password, StaffMember staffMember)
        {
            if (Data.Keys.Any(l => l.Login == login))
            {
                throw new ReportsException($"Account with {login} already exists, please choose another one");
            }

            var newAccount = new StaffMemberAccount(staffMember);
            Data.Add(new LoginData(login, password), newAccount);
            return newAccount;
        }

        public static StaffMemberAccount GetAccount(string login, string password)
        {
            var inputData = new LoginData(login, password);
            LoginData key =  Data.Keys.FirstOrDefault(l => l.IsEqual(inputData)) 
                             ?? throw new ReportsException("Account with this data does not exist");
            return Data[key];
        }

        public static bool CheckValidityOfAccountData(string login, string password)
        {
            var inputData = new LoginData(login, password);
            return (Data.Keys.Any(l => l.IsEqual(inputData)));
        }
    }
}