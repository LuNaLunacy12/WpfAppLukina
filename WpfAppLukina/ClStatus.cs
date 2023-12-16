using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppLukina
{
    
        public class ClStatus
        {
            public ClStatus() { }
            public ClStatus( string namestatus)
            {

                this.NameStatus = namestatus;
               

            }

            public string NameStatus { get; set; }
          
        }
}
