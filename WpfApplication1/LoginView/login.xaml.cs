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
using WpfApplication1.Context;

namespace WpfApplication1.LoginView
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        MyContext myContext = new MyContext();
        public login()
        {
            InitializeComponent();
        }
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((txtEmail.Text == "") || (txtPassword.Password == ""))
                {
                    if(txtEmail.Text == "")
                    {
                        MessageBox.Show("Email is required", "Caution", MessageBoxButton.OK);
                        txtEmail.Focus();
                    }
                    else if(txtPassword.Password == "")
                    {
                        MessageBox.Show("Password is required", "Caution", MessageBoxButton.OK);
                        txtPassword.Focus();
                    }
                }
                else
                {
                    var verifieduser = myContext.Users.FirstOrDefault(v => v.Email == txtEmail.Text);
                    if (verifieduser != null)
                    {
                        var pswd = verifieduser.Password;
                        pswd = txtPassword.Password;
                        if (txtPassword.Password == pswd)
                        {
                            MainWindow dashboard = new MainWindow();
                            dashboard.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Your email and password didn't match!", "Caution", MessageBoxButton.OK);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Your email and password didn't match!", "Caution", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnforgetpassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword dashboard = new ForgotPassword();
            dashboard.Show();
            this.Close();
        }

        private void btnLogin_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void btnforgetpassword_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
