using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlackListAddSQL
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("DefaultConnection")       //Server =ms-sql-9.in-solve.ru;Initial catalog=1gb_blacklist;User ID=1gb_torov-lab;Password=zd5b59c84yzx")
        {

        }

        public DbSet<CityName> GetCityNames { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<FeedBack> feedBacks { get; set; }
        public DbSet<FsspCode> FsspCodes { get; set; }
        public DbSet<ImgUrl> imgUrls { get; set; }

    }
}
