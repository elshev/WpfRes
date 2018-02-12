using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ResApp.Entities
{
    public enum OrderStatus
    {
        Pending,
        Paid,
        Deleted
    }

    public class Order : EntityBase
    {
        private OrderStatus status;
        public OrderStatus Status
        {
            get { return status; }
            set
            {
                if (status == value) return;
                status = value;
                RaisePropertyChanged(() => Status);
            }
        }

        private IEnumerable<Product> products = new ObservableCollection<Product>();
        public IEnumerable<Product> Products
        {
            get { return products; }
            set
            {
                if (products == value) return;
                products = value;
                RaisePropertyChanged(() => Products);
            }
        }
    }
}