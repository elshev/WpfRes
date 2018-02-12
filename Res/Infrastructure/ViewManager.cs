using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ResApp.Entities;
using ResApp.Orders;

namespace ResApp.Infrastructure
{
    public class ViewManager
    {
        #region Singleton implementation
        
        static ViewManager() { } // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        private ViewManager() { }
        private static readonly ViewManager instance = new ViewManager();
        public static ViewManager Instance { get { return instance; } }
        
        #endregion

        public void ShowEntityView(EntityBase entity)
        {
            if (entity == null) return;
            Window view = null;
            var order = entity as Order;
            if (order != null)
                view = new OrderView(new OrderViewModel(order));
            if (view != null)
                view.Show();
        }
    }
}
