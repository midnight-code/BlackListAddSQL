using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackListAddSQL
{
    public class CityClass1
    {
        [JsonProperty("city")]
        public string Name { get; set; }
        [JsonProperty("region")]
        public string Subject { get; set; }
    }
}
