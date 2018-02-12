namespace ResApp.Orders
{
    public partial class OrderView
    {
        public OrderView(IOrderViewModel orderViewModel)
        {
            InitializeComponent();
            DataContext = orderViewModel;
        }

        public IOrderViewModel ViewModel { get { return DataContext as IOrderViewModel; } }
    }

}
