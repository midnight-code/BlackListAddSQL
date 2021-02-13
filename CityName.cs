using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    [Table ("dt_CityName")]
    public class CityName
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column ("region")]
        public string Region { get; set; }
    }
}
