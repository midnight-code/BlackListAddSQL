using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    [Table ("dt_Drivers")]
    public class Driver
    {
        [Column ("id")]
        public int ID { get; set; }


        [Column ("firstname")]
        [Required]
        [Display(Name ="Фамилия")]
        public string FirstName { get; set; }



        [Column ("lastname")]
        public string LastName { get; set; }
        [Column ("secondname")]
        public string SecondName { get; set; }
        [Column ("inn")]
        public int INN { get; set; }
        [Column ("id_passport")]
        public int PassportID { get; set; }
        [Column ("id_driverslicense")]
        public int DriverslicenseID { get; set; }
        [Column ("blacklist")]
        public bool BlackList { get; set; }
        [Column ("avatar")]
        public int Avatar { get; set; }
    }
}
