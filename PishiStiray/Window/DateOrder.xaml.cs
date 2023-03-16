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
    /// Логика взаимодействия для DateOrder.xaml
    /// </summary>
    public partial class DateOrder : Window
    {
        Order order;
        public DateOrder(Order order)
        {
            InitializeComponent();
            this.order = order;
            dpDeliveryDate.SelectedDate = order.OrderDeliveryDate;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Basket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                order.OrderDeliveryDate = (DateTime)dpDeliveryDate.SelectedDate;
                BaseClass.BD.SaveChanges();
                this.Close();
            }
            catch
            {
                MessageBox.Show("При изменение даты доставки возникла ошибка!");
            }
        }
    }
}
