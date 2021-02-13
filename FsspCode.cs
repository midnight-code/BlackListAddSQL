using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    [Table("dt_FsspRegion")]
    public class FsspCode
    {
        [Column ("id")]
        public int ID { get; set; }
        [Column("regumber")]
        public int Regumber { get; set; }
        [Column("regname")]
        public string Name { get; set; }
    }
}
