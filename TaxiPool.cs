﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlackListBlazor.Models
{
    [Table ("dt_TaxiPool")]
    public class TaxiPool
    {
        [Column("id")]
        public int id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        
    }
}
