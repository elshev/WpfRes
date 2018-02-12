using System.Windows.Input;
using ResApp.Entities;
using ResApp.Infrastructure;

namespace ResApp.Orders
{
    public interface IOrderViewModel
    {
    }
    
    public class OrderViewModel : ViewModelBase, IOrderViewModel
    {
        public OrderViewModel(Order order)
        {
            Order = order;
            SaveCommand = new RelayCommand(SaveOrder);
        }

        private Order order;
        public Order Order
        {
            get { return order; }
            set
            {
                if (order == value) return;
                order = value;
                RaisePropertyChanged(() => Order);
            }
        }

        private Product selectedProduct;
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (selectedProduct == value) return;
                selectedProduct = value;
                RaisePropertyChanged(() => SelectedProduct);
            }
        }

        public ICommand SaveCommand { get; private set; }

        private void SaveOrder()
        {
            new OrderRepository().UpdateOrder(Order);
        }

    }
}
