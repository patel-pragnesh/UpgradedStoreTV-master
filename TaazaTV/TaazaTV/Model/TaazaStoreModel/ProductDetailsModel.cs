using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TaazaTV.Model.TaazaStoreModel
{
    class ProductDetailsModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public string[] resourceList { get; set; }
        public Product_Details_Data data { get; set; }
    }

    public class Product_Details_Data
    {
        public Store_Product_Details product_details { get; set; }
        public List<Product_Sku_Options> product_options { get; set; }
    }

    public class Store_Product_Details
    {
        public string product_id { get; set; }
        public string product_name { get; set; }
        public string product_slug { get; set; }
        public string product_description { get; set; }
        public string total_skus { get; set; }
        public string price_range { get; set; }
        public List<Store_Product_Images> images { get; set; }
        public Product_Sku_Variants[] sku_variants { get; set; }
        public string seller_name { get; set; }
        public string seller_details { get; set; }
    }

    public class Product_Sku_Variants
    {
        public string product_id { get; set; }
        public string sku_id { get; set; }
        public string sku { get; set; }
        public string description { get; set; }
        public string regular_price { get; set; }
        public string sale_price { get; set; }
        public List<Product_Sku_Options> options { get; set; }
        public int[] variant_option_ids { get; set; }
        public Store_Product_Images[] images { get; set; }
    }

    public class Product_Sku_Options : INotifyPropertyChanged
    {
        private int _variant_id;
        public int variant_id
        {
            get
            {
                return _variant_id;
            }
            set
            {
                _variant_id = value;
                OnPropertyChanged();
            }
        }


        private string _attribute_name;
        public string attribute_name
        {
            get
            {
                return _attribute_name;
            }
            set
            {
                _attribute_name = value;
                OnPropertyChanged();
            }
        }


        private List<Product_Variant_Options> _variant_options;
        public List<Product_Variant_Options> variant_options
        {
            get
            {
                return _variant_options;
            }
            set
            {
                _variant_options = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }



    public class Product_Variant_Options : INotifyPropertyChanged
    {
        private int _variant_id;
        public int variant_id
        {
            get
            {
                return _variant_id;
            }
            set
            {
                _variant_id = value;
                OnPropertyChanged();
            }
        }


        private bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }


        private string _background_color = "White";
        public string background_color
        {
            get
            {
                return _background_color;
            }
            set
            {
                _background_color = value;

                OnPropertyChanged();
            }
        }


        private int _variant_option_id;
        public int variant_option_id
        {
            get
            {
                return _variant_option_id;
            }
            set
            {
                _variant_option_id = value;
                OnPropertyChanged();
            }
        }


        private string _attribute_value_name;
        public string attribute_value_name
        {
            get
            {
                return _attribute_value_name;
            }
            set
            {
                _attribute_value_name = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
