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
        public ListProduct(User user)
        {
            InitializeComponent();
            lvListProducts.ItemsSource = BaseClass.BD.Product.ToList();
            cbSort.SelectedIndex = 0;
            cbFilt.SelectedIndex = 0;
            tbCountProduct.Text = BaseClass.BD.Product.ToList().Count().ToString() + " из " + BaseClass.BD.Product.ToList().Count().ToString();
            tbFIO.Text = user.UserSurname.Replace(" ", "") + " " + user.UserName + " " + user.UserPatronymic;
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

        public void Filter()
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
                MessageBox.Show("Данные не найдены");
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
    }
}
