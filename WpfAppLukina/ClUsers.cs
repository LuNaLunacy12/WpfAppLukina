using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppLukina
{
    public class ClUsers
    {
        public ClUsers() { }
        public ClUsers(int id, string login, string password, string fullname, string email)
        {
            this.ID = id;
            this.Login = login;
            this.Password = password;
            this.FullName = fullname;
            this.Email = email;


        }
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
 
    }
}
