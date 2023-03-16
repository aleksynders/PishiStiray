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
using System.Windows.Shapes;

namespace PishiStiray
{
    /// <summary>
    /// Логика взаимодействия для StatusOrder.xaml
    /// </summary>
    public partial class StatusOrder : Window
    {
        Order order;
        public StatusOrder(Order order)
        {
            InitializeComponent();
            this.order = order;
            tbHeader.Text = tbHeader.Text + order.OrderID;
            cbStatus.ItemsSource = BaseClass.BD.OrderStatus.ToList();
            cbStatus.SelectedValuePath = "OrderStatusID";
            cbStatus.DisplayMemberPath = "OrderStatus1";
            cbStatus.SelectedValue = order.OrderStatusID;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                order.OrderStatusID = cbStatus.SelectedIndex + 1;
                BaseClass.BD.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("При изменение даты доставки возникла ошибка!\nВозможно не выбран статус...");
            }
        }
    }
}
