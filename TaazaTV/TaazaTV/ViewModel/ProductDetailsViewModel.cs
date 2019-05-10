using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TaazaTV.Model.TaazaStoreModel;

namespace TaazaTV.ViewModel
{
    class ProductDetailsViewModel : INotifyPropertyChanged
    {
        public string _name, _description, _seller, _sellerdesc, _price, _offerPrice, _pricerange, _productID, _skuID;

        public List<Store_Product_Images> _carImages;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public string Seller
        {
            get
            {
                return _seller;
            }
            set
            {
                _seller = value;
                OnPropertyChanged();
            }
        }

        public string SellerDesc
        {
            get
            {
                return _sellerdesc;
            }
            set
            {
                _sellerdesc = value;
                OnPropertyChanged();
            }
        }

        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }

        public string SkuID
        {
            get
            {
                return _skuID;
            }
            set
            {
                _skuID = value;
                OnPropertyChanged();
            }
        }

        public string ProductID
        {
            get
            {
                return _productID;
            }
            set
            {
                _productID = value;
                OnPropertyChanged();
            }
        }

        public string OfferPrice
        {
            get
            {
                return _offerPrice;
            }
            set
            {
                _offerPrice = value;
                OnPropertyChanged();
            }
        }

        public string PriceRange
        {
            get
            {
                return _pricerange;
            }
            set
            {
                _pricerange = value;
                OnPropertyChanged();
            }
        }

        public List<Store_Product_Images> CarImages
        {
            get

            {
                return _carImages;
            }
            set
            {
                _carImages = value;
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
