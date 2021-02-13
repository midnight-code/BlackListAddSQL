using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    [Table("dt_feedback")]
    public class FeedBack
    {
        [Column("id")]
        public int ID { get; set; }

        [Column("id_driver")]
        public int DriverID { get; set; }

        [Column("id_taxpool")]
        public int TaxiPoolID { get; set; }

        [Column("subjest")]
        public string Subjest { get; set; }
        [Column("dateadd")]
        public DateTime DateADD { get; set; }
        [Column ("id_city")]
        public int CityID { get; set; }
    }
}
