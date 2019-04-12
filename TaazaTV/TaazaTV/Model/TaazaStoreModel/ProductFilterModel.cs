using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class ProductFilterModel
    {
        public BrandList[] brand_list { get; set; }
        public double min_price { get; set; }
        public double max_price { get; set; }
    }

    public partial class BrandList
    {
        public int brand_id { get; set; }
        public string brand_title { get; set; }
        public string image { get; set; }
    }
}
