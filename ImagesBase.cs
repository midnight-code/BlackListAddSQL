using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    [Table("dt_ImageDoc")]
    public class ImagesBase
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("driver_id")]
        public int DriverID { get; set; }
        [Column("screen")]
        public object ScreenImage { get; set; }
        [Column("screen_format")]
        public string ScreenFormat { get; set; }
    }
}
