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
    }
}
