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
    /// Логика взаимодействия для ListProduct.xaml
    /// </summary>
    public partial class ListProduct : Page
    {
        User user;
        List<ProductBasket> basket = new List<ProductBasket>();
        public ListProduct(User user) // Авторизованный пользователь
        {
            InitializeComponent();
            lvListProducts.ItemsSource = BaseClass.BD.Product.ToList();
            cbSort.SelectedIndex = 0;
            cbFilt.SelectedIndex = 0;
            tbCountProduct.Text = BaseClass.BD.Product.ToList().Count().ToString() + " из " + BaseClass.BD.Product.ToList().Count().ToString();
            tbFIO.Text = user.UserSurname.Replace(" ", "") + " " + user.UserName + " " + user.UserPatronymic;
            // В TextBlock добавляется наименование роли пользователя
            if (user.Role.RoleName == "Менеджер")
                tbFIO.Text = "Менеджер" + " " + tbFIO.Text;
            if (user.Role.RoleName == "Администратор")
                tbFIO.Text = "Администратор" + " " + tbFIO.Text;
            tbFIO.Visibility = Visibility.Visible;
            this.user = user;
        }
        public ListProduct()
        {
            InitializeComponent();
            lvListProducts.ItemsSource = BaseClass.BD.Product.ToList();
            cbSort.SelectedIndex = 0;
            cbFilt.SelectedIndex = 0;
            tbCountProduct.Text = BaseClass.BD.Product.ToList().Count().ToString() + " из " + BaseClass.BD.Product.ToList().Count().ToString();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new Autorization());
        }

        public void Filter() // Фильтрация, сортировка списка продуктов
        {
            List<Product> product = BaseClass.BD.Product.ToList();
            if (tbSearch.Text.Length > 0)
                product = product.Where(x => x.ProductName.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
            if (cbFilt.SelectedIndex > 0)
            {
                switch (cbFilt.SelectedIndex)
                {
                    case 1:
                        product = product.Where(x => x.ProductDiscountAmount > 0 && x.ProductDiscountAmount < 9.99).ToList();
                        break;
                    case 2:
                        product = product.Where(x => x.ProductDiscountAmount > 10 && x.ProductDiscountAmount < 14.99).ToList();
                        break;
                    case 3:
                        product = product.Where(x => x.ProductDiscountAmount > 15).ToList();
                        break;
                }
            }
            if (cbSort.SelectedIndex > 0)
            {
                switch (cbSort.SelectedIndex)
                {
                    case 1:
                        product = product.OrderBy(x => x.costWithDiscount).ToList();
                        break;
                    case 2:
                        product = product.OrderByDescending(x => x.costWithDiscount).ToList();
                        break;
                }
            }
            lvListProducts.ItemsSource = product;
            if (product.Count == 0)
                MessageBox.Show("Данные не найдены!");
            tbCountProduct.Text = "" + product.Count() + " из " + BaseClass.BD.Product.ToList().Count();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void cbFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void AddBasket_Click(object sender, RoutedEventArgs e)
        {
            Product x = (Product)lvListProducts.SelectedItem;
            bool stock = false;
            foreach (ProductBasket productBasket in basket)
            {
                if (productBasket.product == x) // Увеличение колличества товара в корзине на +1
                {
                    productBasket.count = productBasket.count += 1;
                    stock = true;
                }
            }
            if (!stock) // Добавление нового товара в корзину
            {
                ProductBasket product = new ProductBasket();
                product.product = x;
                product.count = 1;
                basket.Add(product);
            }
            BasketBtn.Visibility = Visibility.Visible;
        }

        private void BasketBtn_Click(object sender, RoutedEventArgs e)
        {
            Basket basketWin = new Basket(basket, user);
            basketWin.ShowDialog();
            if (basket.Count == 0)
                BasketBtn.Visibility = Visibility.Hidden;
        }

        private void DeleteBtn_Loaded(object sender, RoutedEventArgs e)
        {
            if (user == null)
                return;
            Button btnDelete = sender as Button;
            if (user.Role.RoleName == "Менеджер" || user.Role.RoleName == "Администратор")
                btnDelete.Visibility = Visibility.Visible;
            else
                btnDelete.Visibility = Visibility.Collapsed;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                int index = Convert.ToInt32(btn.Uid);
                Product product = BaseClass.BD.Product.FirstOrDefault(x => x.ProductID == index);
                List<OrderProduct> orderProducts = BaseClass.BD.OrderProduct.Where(x => x.ProductID == index).ToList();
                if (orderProducts.Count == 0)
                {
                    BaseClass.BD.Product.Remove(product);
                    BaseClass.BD.SaveChanges();
                }
                else
                    MessageBox.Show("Товар нельзя удалить так как он указан в заказе!");
            }
            catch
            {
                MessageBox.Show("При удаление товара возникла ошибка");
            }
        }

        private void OrdersBtn_Loaded(object sender, RoutedEventArgs e)
        {
            if (user == null)
                return;
            Button btn = sender as Button;
            if (user.Role.RoleName == "Менеджер" || user.Role.RoleName == "Администратор")
                btn.Visibility = Visibility.Visible;
            else
                btn.Visibility = Visibility.Collapsed;
        }

        private void OrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ListOrders(user));
        }
    }
}
