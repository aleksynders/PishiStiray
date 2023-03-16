using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
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
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        double sum; // Общая сумма заказа
        double sumDiscount; // Общая сумма скидки заказа
        User user;
        List<ProductBasket> bascet;
        public Basket(List<ProductBasket> bascet, User user)
        {
            InitializeComponent();
            this.bascet = bascet;
            this.user = user;
            lvProducts.ItemsSource = bascet;
            SumAndDiscount();
            PickupPoint.ItemsSource = BaseClass.BD.PickupPoint.ToList();
            PickupPoint.SelectedValuePath = "OrderPickupPointID";
            PickupPoint.DisplayMemberPath = "OrderPickupPoint";
            PickupPoint.SelectedIndex = 0;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextBox tb = (TextBox)sender;
                int index = Convert.ToInt32(tb.Uid);
                ProductBasket productBasket = bascet.FirstOrDefault(x => x.product.ProductID == index);
                if (tb.Text.Replace(" ", "") == "") // Если пустое значение в количестве
                    productBasket.count = 0;
                else
                    productBasket.count = Convert.ToInt32(tb.Text);
                if (productBasket.count == 0) // Если 0-вое значение в количестве
                    bascet.Remove(productBasket);
                if (bascet.Count == 0) // Если корзина пуста, закрываем окно
                    this.Close();
                lvProducts.Items.Refresh();
                SumAndDiscount();
            }
            catch {
                MessageBox.Show("Произошла ошибка: Возможно Вы ввели в неверном формате количество товара");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnBasket_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order order = new Order();
                int countDay = 0; 
                List<Order> orderLast = BaseClass.BD.Order.OrderBy(x => x.OrderID).ToList();
                order.OrderStatusID = BaseClass.BD.OrderStatus.FirstOrDefault(x => x.OrderStatus1 == "Новый").OrderStatusID;
                order.OrderDate = DateTime.Now;
                if (getDeliveryTime())
                {
                    countDay = 6;
                }
                else
                {
                    countDay = 3;
                }
                order.OrderDeliveryDate = order.OrderDate.AddDays(countDay);
                order.OrderPickupPointID = (int)((PishiStiray.PickupPoint)PickupPoint.SelectedItem).OrderPickupPointID;
                if (user != null)
                {
                    order.UserID = user.UserID;
                }
                Random rand = new Random();
                string textCode = "";
                for (int i = 0; i < 3; i++)
                {
                    textCode = textCode + rand.Next(10).ToString();
                }
                order.OrderCode = Convert.ToInt32(textCode);
                BaseClass.BD.Order.Add(order);
                BaseClass.BD.SaveChanges();
                foreach (ProductBasket productBasket in bascet)
                {
                    OrderProduct orderProduct = new OrderProduct();
                    orderProduct.OrderID = order.OrderID;
                    orderProduct.ProductID = productBasket.product.ProductID;
                    orderProduct.Count = productBasket.count;
                    BaseClass.BD.OrderProduct.Add(orderProduct);
                }
                BaseClass.BD.SaveChanges();
                MessageBox.Show("Заказ успешно создан");
            }
            catch
            {
                MessageBox.Show("При создание заказа возникла ошибка!");
            }
        }

        private bool getDeliveryTime()
        {
            foreach (ProductBasket productBasket in bascet)
            {
                if (productBasket.product.ProductQuantityInStock < 3 || productBasket.product.ProductQuantityInStock < productBasket.count)
                {
                    return true;
                }
            }
            return false;
        }

        private void SumAndDiscount()
        {
            sum = 0;
            sumDiscount = 0;
            foreach (ProductBasket productBasket in bascet)
            {
                sum += productBasket.count * productBasket.product.costWithDiscount;
                sumDiscount += productBasket.count * ((double)productBasket.product.ProductCost - productBasket.product.costWithDiscount);
            }
            tbSum.Text = "Сумма заказа: " + sum.ToString("0.00") + " руб.";
            tbSumDiscount.Text = "Сумма скидки: " + sumDiscount.ToString("0.00") + " руб.";
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            ProductBasket productBasket = bascet.FirstOrDefault(x => x.product.ProductID == index);
            bascet.Remove(productBasket);
            if (bascet.Count == 0)
                this.Close();
            lvProducts.Items.Refresh();
            SumAndDiscount();
        }
    }
}
