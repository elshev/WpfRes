using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Input;
using Common.Res;
using ResApp.Entities;
using ResApp.Infrastructure;
using ResApp.Orders;

namespace ResApp
{
    public interface IMainViewModel
    {
        void ChangeLanguage(CultureInfo cultureInfo);
    }

    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public MainViewModel()
        {
            RefreshOrders();
            ShowOrderCommand = new RelayCommand(ShowOrder, () => SelectedOrder != null);
        }

        private void RefreshOrders()
        {
            orders = new ObservableCollection<Order>(new OrderRepository().GetOrders());
            RaisePropertyChanged(() => Orders);
            if (SelectedOrder == null)
                SelectedOrder = orders[0];
        }

        private ObservableCollection<Order> orders;
        public ObservableCollection<Order> Orders { get { return orders; } }

        public ICommand ShowOrderCommand { get; private set; }

        private Order selectedOrder;
        public Order SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                if (selectedOrder == value) return;
                selectedOrder = value;
                RaisePropertyChanged(() => SelectedOrder);
            }
        }

        private void ShowOrder()
        {
            var order = SelectedOrder;
            ViewManager.Instance.ShowEntityView(order);
            RefreshOrders();
        }

        public void ChangeLanguage(CultureInfo cultureInfo)
        {
            CultureManager.Instance.UICulture = cultureInfo;
        }
    }
}