using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    public class PersonClass
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public DateTime DateTimes { get; set; }
        public DateTime DateAddBlack { get; set; }
        public string City { get; set; }
        public string Opisanie { get; set; }
        public string DriversLicenz { get; set; }
        public string Phone { get; set; }
        public string ImagePatch { get; set; }

    }
}
