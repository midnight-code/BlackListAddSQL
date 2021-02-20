using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    [Table("dt_ImagUrl")]
    public class ImgUrl
    {
        [Column("id")]
        public int ID { get; set; }
        [Column("driver_id")]
        public int DriverID { get; set; }
        [Column("img_url")]
        public string ImagesUrl { get; set; }
        [Column("thumb")]
        public bool Thumb { get; set; } = false;
    }
}
