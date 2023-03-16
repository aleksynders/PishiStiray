using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PishiStiray
{
    /// <summary>
    /// Логика взаимодействия для ListOrders.xaml
    /// </summary>
    public partial class ListOrders : Page
    {
        User user;
        public ListOrders(User user)
        {
            InitializeComponent();
            this.user = user;
            lvOrders.ItemsSource = BaseClass.BD.Order.ToList();
            cbSort.SelectedIndex = 0;
            cbFilt.SelectedIndex = 0;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ListProduct(user));
        }

        private void Filter()
        {
            List<Order> orders = BaseClass.BD.Order.ToList();
            if (cbFilt.SelectedIndex > 0)
            {
                switch (cbFilt.SelectedIndex)
                {
                    case 1:
                        orders = orders.Where(x => x.DiscountProcent > 0 && x.DiscountProcent < 10).ToList();
                        break;
                    case 2:
                        orders = orders.Where(x => x.DiscountProcent >= 10 && x.DiscountProcent < 15).ToList();
                        break;
                    case 3:
                        orders = orders.Where(x => x.DiscountProcent >= 15).ToList();
                        break;
                }
            }
            if (cbSort.SelectedIndex > 0)
            {
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        orders = orders.OrderBy(x => x.Summa).ToList();
                        break;
                    case 2:
                        orders = orders.OrderByDescending(x => x.Summa).ToList();
                        break;
                }
            }
            lvOrders.ItemsSource = orders;
            if (orders.Count == 0)
                MessageBox.Show("Данные не найдены");
        }

        private void Filter(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void btnChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = Convert.ToInt32(btn.Uid);
            Order order = BaseClass.BD.Order.FirstOrDefault(x => x.OrderID == index);
            StatusOrder changeStatus = new StatusOrder(order);
            changeStatus.ShowDialog();
        }

        private void btnChangeDeliveryDate_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            int index = Convert.ToInt32(btn.Uid);
            Order order = BaseClass.BD.Order.FirstOrDefault(x => x.OrderID == index);
            DateOrder dateOrder = new DateOrder(order);
            dateOrder.ShowDialog();
        }
    }
}

