using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppLukina
{
    public class ClTasks
    {
    
            public ClTasks() { }
        public ClTasks(int id, string heading, string description, DateTime datecreate, string status, int user)
        {
            this.ID = id;
            this.Heading = heading;
            this.Description = description;
            this.Datecreate = datecreate;
            this.Status = status;
            this.User = user;

        }
        public int ID { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public DateTime Datecreate { get; set; }
        public string Status { get; set; }
        public int User { get; set; }

    }
}
