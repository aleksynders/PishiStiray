using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;

namespace PishiStiray
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {

        int countError;
        public static bool correct;
        int countTime;
        DispatcherTimer disTimer = new DispatcherTimer();

        public Autorization()
        {
            InitializeComponent();
            countError = 0;
            correct = false;
            disTimer.Interval = new TimeSpan(0, 0, 1);
            disTimer.Tick += new EventHandler(DisTimer_Tick);
        }

        private void DisTimer_Tick(object sender, EventArgs e)
        {
            if (countTime == 0)
            {
                AutBtn.IsEnabled = true;
                GuestBtn.IsEnabled = true;
                disTimer.Stop();
                tbCode.Visibility = Visibility.Collapsed;
            }
            else
                tbCode.Text = "Попытаться авторизоваться можно будет через " + countTime + " секунд";
            countTime--;
        }

        private void AutBtn_Click(object sender, RoutedEventArgs e)
        {
            User user = BaseClass.BD.User.FirstOrDefault(x => x.UserLogin == tbLogin.Text && x.UserPassword == pbPassword.Password);
            if (user != null)
                FrameClass.frame.Navigate(new ListProduct(user));
            else
            {
                if (countError == 0)
                {
                    MessageBox.Show("Пользователь с таким логиным и паролем не найден!");
                    countError++;
                }
                else
                {
                    CAPTCHA captcha = new CAPTCHA();
                    captcha.ShowDialog();
                    countError++;
                    if (!correct)
                    {
                        AutBtn.IsEnabled = false;
                        GuestBtn.IsEnabled = false;
                        countTime = 10;
                        tbCode.Text = "Получить новый код можно будет через " + countTime + " секунд";
                        tbCode.Visibility = Visibility.Visible;
                        disTimer.Start();
                    }
                }
            }
        }

        private void GuestBtn_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.frame.Navigate(new ListProduct());
        }
    }
}
