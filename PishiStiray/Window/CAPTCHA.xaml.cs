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
    /// Логика взаимодействия для CAPTCHA.xaml
    /// </summary>
    public partial class CAPTCHA : Window
    {
        public string text;
        public CAPTCHA()
        {
            InitializeComponent();
            Random rand = new Random();
            int kolText = 4;
            text = "";
            for (int i = 0; i < kolText; i++)
            {
                int j = rand.Next(2);
                if (j == 0)
                    text = text + rand.Next(9).ToString();
                else
                {
                    int l = rand.Next(2);
                    if (l == 0)
                    {
                        text = text + (char)rand.Next('A', 'Z' + 1);
                    }
                    else
                    {
                        text = text + (char)rand.Next('a', 'z' + 1);
                    }
                }
            }
            int widthBegin = 0;
            int widthEnd = 0;
            int h = (int)CvField.Width / text.Length;
            for (int i = 0; i < text.Length; i++)
            {
                if (i == 0) 
                    widthEnd += h;
                else
                {
                    widthBegin = widthEnd;
                    widthEnd += h;
                }
                int height = rand.Next((int)CvField.Height);
                int width = rand.Next(widthBegin, widthEnd);
                if (height > 170)
                    height -= 30;
                if (width > 590)
                    widthEnd -= 10;
                int fontSize = rand.Next(16, 33);
                TextBlock txt = new TextBlock()
                {
                    Text = text[i].ToString(),
                    TextDecorations = TextDecorations.Strikethrough,
                    Padding = new Thickness(width, height, 0, 0),
                    FontSize = fontSize,
                };
                CvField.Children.Add(txt);
            }
            int kolLine = rand.Next(5, 16);
            for (int i = 0; i < kolLine; i++)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
                Line l = new Line()
                {
                    X1 = rand.Next((int)CvField.Width),
                    Y1 = rand.Next((int)CvField.Height),
                    X2 = rand.Next((int)CvField.Width),
                    Y2 = rand.Next((int)CvField.Height),
                    Stroke = brush,
                };
                CvField.Children.Add(l);
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (tbInputField.Text == text)
            {
                Autorization.correct = true;
                this.Close();
            }
            else
            {
                Autorization.correct = false;
                this.Close();
            }
        }
    }
}
