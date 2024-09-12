using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.DAL
{
    public class Setting
    {
        public static IpDetails ipDetail = new IpDetails();
    }

    public class IpDetails
    {
        public string URL { get; set; }
        public string Key { get; set; }
    }
}
