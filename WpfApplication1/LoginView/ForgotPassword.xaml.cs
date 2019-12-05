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
using Outlook = Microsoft.Office.Interop.Outlook;

namespace WpfApplication1.LoginView
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        MyContext myContext = new MyContext();
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void btnSendEmail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtEmail.Text == "")
                {
                    MessageBox.Show("Email is required", "Caution", MessageBoxButton.OK);
                    txtEmail.Focus();
                }
                else
                {
                    var checkemail = myContext.Users.FirstOrDefault(v => v.Email == txtEmail.Text);
                    if (checkemail != null)
                    {
                        var email = checkemail.Email;
                        if (txtEmail.Text == email)
                        {
                            string newuserpass = Guid.NewGuid().ToString();
                            var emailcheck = myContext.Users.Where(s => s.Email == txtEmail.Text).FirstOrDefault();
                            emailcheck.Password = newuserpass;
                            myContext.SaveChanges();
                            MessageBox.Show("Password has been updated");
                            Outlook._Application _app = new Outlook.Application();
                            Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                            mail.To = txtEmail.Text;
                            mail.Subject ="[Forgot Password] " + DateTime.Now.ToString("ddMMyyyyhhmmss");
                            mail.Body = "Hi " + txtEmail.Text + "\nThis Is Your New Password : " + newuserpass;
                            mail.Importance = Outlook.OlImportance.olImportanceNormal;
                            ((Outlook._MailItem)mail).Send();
                            MessageBox.Show("Check Your Email for Your New Password", "Message", MessageBoxButton.OK);
                        }
                        //else
                        //{
                        //    MessageBox.Show("That Email Not Registered Yet!", "Caution", MessageBoxButton.OK);
                        //}
                    }
                    else
                    {
                        MessageBox.Show("That Email Not Registered Yet!", "Caution", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnLoginBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            login dashboard = new login();
            dashboard.Show();
            this.Close();
        }
    }
}
