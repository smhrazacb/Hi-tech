using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoTest.Entities
{
    public class ProductDetail
    {
        public string Desc { get; set; }
        public string DataSheetUrl { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }

    }
}
