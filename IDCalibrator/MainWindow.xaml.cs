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
using System.Windows.Media.Animation;

namespace IDCalibrator
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        DoubleAnimation fadein = new DoubleAnimation();//创建动画
        public MainWindow()
        {
            InitializeComponent();

            fadein.From = 0;
            fadein.To = 1;
            fadein.Duration = TimeSpan.FromMilliseconds(500);
        }

        public bool CheckChinaIDCardNumberFormat(string idCardNumber)
        {
            string idNumber = idCardNumber;
            bool result = true;
            try
            {
                if (idNumber.Length != 18)
                {
                    return false;
                }


                long n = 0;
                if (long.TryParse(idNumber.Remove(17), out n) == false
                    || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
                {
                    return false;//数字验证  
                }
                string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
                if (address.IndexOf(idNumber.Remove(2)) == -1)
                {
                    return false;//省份验证  
                }
                string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
                DateTime time = new DateTime();
                if (DateTime.TryParse(birth, out time) == false)
                {
                    return false;//生日验证  
                }
                string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
                string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
                char[] Ai = idNumber.Remove(17).ToCharArray();
                int sum = 0;
                for (int i = 0; i < 17; i++)
                {
                    sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
                }
                int y = -1;
                Math.DivRem(sum, 11, out y);
                if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
                {
                    return false;//校验码验证  
                }
                return true;//符合GB11643-1999标准 

            }
            catch (Exception ex)
            {
                MessageBox.Show("发生异常：" + ex.Message, "异常");
                result = false;
            }
            return result;
        }

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {


            if (txbCardNumber.Text != null && txbCardNumber.Text != "")
            {
                if (CheckChinaIDCardNumberFormat(txbCardNumber.Text))
                {
                    txbResult.Text = "身份证通过校验。";
                    txbResult.Foreground = new SolidColorBrush(Color.FromRgb(61, 145, 64));
                }
                else
                {
                    txbResult.Text = "身份证未通过校验。";
                    txbResult.Foreground = new SolidColorBrush(Color.FromRgb(255, 153, 18));
                }
            }
            else
            {
                txbResult.Text = "请输入身份证。";
                txbResult.Foreground = new SolidColorBrush(Color.FromRgb(178, 34, 34));
            }
            txbResult.BeginAnimation(OpacityProperty, fadein);
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            txbResult.Foreground = new SolidColorBrush(Colors.Gray);
            txbResult.Text = "身份证校验器 v1.0.0\n基于 .Net Framework 4.7.2 与 Windows Presentation Foundation\n使用 C# 编写\n魏天图、付宇轩开发";
            txbResult.BeginAnimation(OpacityProperty, fadein);
        }
    }
}
