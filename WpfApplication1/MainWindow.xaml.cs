using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WpfApplication1.Context;
using WpfApplication1.Model;
using Outlook = Microsoft.Office.Interop.Outlook;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using System.ComponentModel;
using System.Drawing;
using System.Collections.ObjectModel;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyContext myContext = new MyContext();
        int cb_sup;
        int cb_role;
        int cb_itemCode;
        int updStock;
        int grandTotal;
        int pay;
        //User userlogin = new User();
        string laporan = "Id\t" + "Name\t" + "Price\t" + "Quantity\n";
        List<ListTransaction> TransList = new List<ListTransaction>();

        public MainWindow()
        {
            InitializeComponent();
            dataGrid.ItemsSource = myContext.Suppliers.ToList();
            dataGridItem.ItemsSource = myContext.Items.ToList();
            datagridlistuser.ItemsSource = myContext.Users.ToList();
            datagridlistrole.ItemsSource = myContext.Roles.ToList();
            cbSupplier.ItemsSource = myContext.Suppliers.ToList();
            cbItemCode.ItemsSource = myContext.Items.ToList();
            cbUserRole.ItemsSource = myContext.Roles.ToList();
            btnDeleteItem.IsEnabled = false;
            btnUpdateItem.IsEnabled = false;
            btnClearItem.IsEnabled = false;
            btnDelete.IsEnabled = false;
            btnUpdate.IsEnabled = false;
            btnClear.IsEnabled = false;
            txtTransactionDate.Text = DateTimeOffset.Now.DateTime.ToString();
            //this.userlogin = verifieduser;

            //if(verifieduser.Role.Id == 1)
            //{
            //    tabAddUser.Visibility = Visibility.Hidden;
            //    datagridlistuser.Visibility = Visibility.Hidden;
            //}
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((txtName.Text == "") || (txtEmail.Text == ""))
                {
                    if (txtName.Text == "")
                    {
                        MessageBox.Show("Name is required", "Caution", MessageBoxButton.OK);
                        txtName.Focus();
                    }
                    else if (txtEmail.Text == "")
                    {
                        MessageBox.Show("Email is required", "Caution", MessageBoxButton.OK);
                        txtEmail.Focus();
                    }
                }
                else
                {
                    var checkEmail = myContext.Suppliers.FirstOrDefault(s => s.Email == txtEmail.Text);
                    //var checkEmail = myContext.Suppliers.Where(s => s.Email == txtEmail.Text);
                    if (checkEmail == null)
                    {
                        var push = new Supplier(txtName.Text, txtEmail.Text);
                        myContext.Suppliers.Add(push);
                        var result = myContext.SaveChanges();
                        if (result > 0)
                        {
                            MessageBox.Show(result + " row has been inserted");
                        }

                        dataGrid.ItemsSource = myContext.Suppliers.ToList();

                        //Outlook._Application _app = new Outlook.Application();
                        //Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                        //mail.To = txtEmail.Text;
                        //mail.Body = "hai " + txtName.Text + " email ini dikirim menggunakan wpf";
                        //mail.Importance = Outlook.OlImportance.olImportanceNormal;
                        //((Outlook._MailItem)mail).Send();
                        //MessageBox.Show("Your email has been sent!", "Message", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Email has been used");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
            }
            txtName.Text = "";
            txtEmail.Text = "";
            cbSupplier.ItemsSource = myContext.Suppliers.ToList();
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var data = dataGrid.SelectedItem;
            string id = (dataGrid.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            txtId.Text = id;
            string name = (dataGrid.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
            txtName.Text = name;
            string email = (dataGrid.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
            txtEmail.Text = email;
            btnSubmit.IsEnabled = false;
            btnDelete.IsEnabled = true;
            btnUpdate.IsEnabled = true;
            btnClear.IsEnabled = true;
        }

        private void txtEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z0-9.@]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(txtId.Text);
                var dRow = myContext.Suppliers.Where(s => s.Id == num).FirstOrDefault();
                dRow.Name = txtName.Text;
                dRow.Email = txtEmail.Text;
                var result = myContext.SaveChanges();
                MessageBox.Show(result + " row has been updated");
                txtId.Text = "";
                txtName.Text = "";
                txtEmail.Text = "";
                dataGrid.ItemsSource = myContext.Suppliers.ToList();
                btnSubmit.IsEnabled = true;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                btnClear.IsEnabled = false;
            }
            catch (Exception)
            {

            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(txtId.Text);
                var dRow = myContext.Suppliers.Where(s => s.Id == num).FirstOrDefault();
                MessageBoxResult messageboxresult = System.Windows.MessageBox.Show("Are you sure want to delete this supplier?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageboxresult == MessageBoxResult.Yes)
                {
                    btnSubmit.IsEnabled = false;
                    btnDelete.IsEnabled = true;
                    btnUpdate.IsEnabled = true;
                    btnClear.IsEnabled = true;
                    myContext.Suppliers.Remove(dRow);
                    myContext.SaveChanges();
                    MessageBox.Show(" row has been deleted");
                    txtId.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    dataGrid.ItemsSource = myContext.Suppliers.ToList();
                }
                else
                {
                    btnSubmit.IsEnabled = false;
                    btnDelete.IsEnabled = true;
                    btnUpdate.IsEnabled = true;
                    btnClear.IsEnabled = true;
                    txtId.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    dataGrid.ItemsSource = myContext.Suppliers.ToList();
                }
            }
            catch (Exception)
            {
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = myContext.Suppliers.ToList();
        }

        private void cbSupplier_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_sup = Convert.ToInt32(cbSupplier.SelectedValue.ToString());
            //MessageBox.Show(cb_sup.ToString());
        }

        private void btnSubmitItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((cbSupplier.Text == "") || (txtItemName.Text == "") || (txtItemStock.Text == "") || (txtItemPrice.Text == ""))
                {

                    if (cbSupplier.Text == "")
                    {
                        MessageBox.Show("Supplier Name is required", "Caution", MessageBoxButton.OK);
                        cbSupplier.Focus();
                    }
                    else if (txtItemName.Text == "")
                    {
                        MessageBox.Show("Item Name is required", "Caution", MessageBoxButton.OK);
                        txtItemName.Focus();
                    }
                    else if (txtItemStock.Text == "")
                    {
                        MessageBox.Show("Stock is required", "Caution", MessageBoxButton.OK);
                        txtItemStock.Focus();
                    }
                    else if (txtItemPrice.Text == "")
                    {
                        MessageBox.Show("Price is required", "Caution", MessageBoxButton.OK);
                        txtItemPrice.Focus();
                    }
                }
                else
                {
                    if (txtItemName.Text != null)
                    {
                        int Stock = Convert.ToInt32(txtItemStock.Text);
                        int Price = Convert.ToInt32(txtItemPrice.Text);

                        var supplier = myContext.Suppliers.Where(x => x.Id == cb_sup).FirstOrDefault();
                        var itemname = myContext.Items.Where(y => y.Name == txtItemName.Text && y.Supplier.Name == cbSupplier.Text).FirstOrDefault();


                        if (itemname != null) //same item name
                        {
                            var stockrecent = itemname.Stock;
                            int pricerecent = itemname.Price;
                            updStock = Stock + stockrecent;

                            if (txtItemPrice.Text == pricerecent.ToString() && supplier.Name == itemname.Supplier.Name && itemname.Name == txtItemName.Text)
                            //if (txtItemPrice.Text == pricerecent.ToString()) //same item name, same price
                            {
                                itemname.Stock = Convert.ToInt32(updStock);
                                var result2 = myContext.SaveChanges();

                                if (result2 > 0)
                                {
                                    MessageBox.Show("Stock Updated");
                                    txtItemName.Text = "";
                                    txtItemPrice.Text = "";
                                    txtItemStock.Text = "";
                                }
                                else
                                {
                                    MessageBox.Show("Stock not Updated");
                                    txtItemName.Text = "";
                                    txtItemPrice.Text = "";
                                    txtItemStock.Text = "";
                                }
                            }
                            else //different price
                            {
                                int Stock2 = Convert.ToInt32(txtItemStock.Text);
                                int Price2 = Convert.ToInt32(txtItemPrice.Text);

                                var supplier2 = myContext.Suppliers.Where(w => w.Id == cb_sup).FirstOrDefault();
                                var pushStock = new Item(txtItemName.Text, Stock2, Price2, supplier2);
                                myContext.Items.Add(pushStock);
                                var result = myContext.SaveChanges();
                                if (result > 0)
                                {
                                    MessageBox.Show("Item Inserted");
                                    txtItemName.Text = "";
                                    txtItemPrice.Text = "";
                                    txtItemStock.Text = "";
                                }
                                else
                                {
                                    MessageBox.Show("Item can't Inserted");
                                    txtItemName.Text = "";
                                    txtItemPrice.Text = "";
                                    txtItemStock.Text = "";
                                }
                            }
                            dataGridItem.ItemsSource = myContext.Items.ToList();
                        }
                        else //different item name
                        {
                            int Stock2 = Convert.ToInt32(txtItemStock.Text);
                            int Price2 = Convert.ToInt32(txtItemPrice.Text);

                            var supplier2 = myContext.Suppliers.Where(w => w.Id == cb_sup).FirstOrDefault();
                            var pushStock = new Item(txtItemName.Text, Stock2, Price2, supplier2);
                            myContext.Items.Add(pushStock);
                            var result = myContext.SaveChanges();
                            if (result > 0)
                            {
                                MessageBox.Show("Item Inserted");
                                txtItemName.Text = "";
                                txtItemPrice.Text = "";
                                txtItemStock.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Item can't Inserted");
                                txtItemName.Text = "";
                                txtItemPrice.Text = "";
                                txtItemStock.Text = "";
                            }
                            dataGridItem.ItemsSource = myContext.Items.ToList();
                        }
                    }
                }
                cbItemCode.ItemsSource = myContext.Items.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
            }

        }

        private void txtItemName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtItemStock_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtItemPrice_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtItemId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void dataGridItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {

                var data = dataGridItem.SelectedItem;
                string id = (dataGridItem.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
                txtItemId.Text = id;
                string suppliername = (dataGridItem.SelectedCells[1].Column.GetCellContent(data) as TextBlock).Text;
                cbSupplier.Text = suppliername;
                string name = (dataGridItem.SelectedCells[2].Column.GetCellContent(data) as TextBlock).Text;
                txtItemName.Text = name;
                string stock = (dataGridItem.SelectedCells[3].Column.GetCellContent(data) as TextBlock).Text;
                txtItemStock.Text = stock;
                string price = (dataGridItem.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
                txtItemPrice.Text = price;
                btnSubmitItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = true;
                btnUpdateItem.IsEnabled = true;
                btnClearItem.IsEnabled = true;
            }
            catch (Exception)
            {

            }
        }

        private void btnDeleteItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int num = Convert.ToInt32(txtItemId.Text);
                var itemRow = myContext.Items.Where(i => i.Id == num).FirstOrDefault();
                MessageBoxResult messageboxresult = System.Windows.MessageBox.Show("Are you sure want to delete this item?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                if (messageboxresult == MessageBoxResult.Yes)
                {
                    btnSubmitItem.IsEnabled = true;
                    btnDeleteItem.IsEnabled = false;
                    btnUpdateItem.IsEnabled = false;
                    btnClearItem.IsEnabled = false;
                    myContext.Items.Remove(itemRow);
                    myContext.SaveChanges();
                    MessageBox.Show("row has been deleted");
                    txtItemId.Text = "";
                    txtItemName.Text = "";
                    txtItemStock.Text = "";
                    txtItemPrice.Text = "";
                    cbSupplier.Text = "";
                    dataGridItem.ItemsSource = myContext.Items.ToList();
                }
                else
                {
                    btnSubmitItem.IsEnabled = true;
                    btnDeleteItem.IsEnabled = false;
                    btnUpdateItem.IsEnabled = false;
                    btnClearItem.IsEnabled = false;
                    txtItemId.Text = "";
                    txtItemName.Text = "";
                    txtItemStock.Text = "";
                    txtItemPrice.Text = "";
                    cbSupplier.Text = "";
                    dataGridItem.ItemsSource = myContext.Items.ToList();
                }
            }
            catch (Exception)
            {
                dataGridItem.ItemsSource = myContext.Items.ToList();
            }
        }

        private void btnClearItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSubmitItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = false;
                btnUpdateItem.IsEnabled = false;
                btnClearItem.IsEnabled = false;
                txtItemId.Text = "";
                txtItemName.Text = "";
                txtItemStock.Text = "";
                txtItemPrice.Text = "";
                cbSupplier.Text = "";
                dataGridItem.ItemsSource = myContext.Items.ToList();
            }
            catch (Exception)
            {

            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSubmit.IsEnabled = true;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
                btnClear.IsEnabled = false;
                txtId.Text = "";
                txtName.Text = "";
                txtEmail.Text = "";
            }
            catch (Exception)
            {

            }
        }

        private void btnUpdateItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var supplier = myContext.Suppliers.Where(i => i.Id == cb_sup).FirstOrDefault();
                int num = Convert.ToInt32(txtItemId.Text);
                var dRow = myContext.Items.Where(i => i.Id == num).FirstOrDefault();
                dRow.Supplier = supplier;
                dRow.Name = txtItemName.Text;
                dRow.Stock = Convert.ToInt32(txtItemStock.Text);
                dRow.Price = Convert.ToInt32(txtItemPrice.Text);
                myContext.SaveChanges();
                dataGridItem.ItemsSource = myContext.Items.ToList();
                MessageBox.Show(" row has been updated");
                btnSubmitItem.IsEnabled = true;
                btnDeleteItem.IsEnabled = false;
                btnUpdateItem.IsEnabled = false;
                btnClearItem.IsEnabled = false;
                cbSupplier.Text = "";
                txtItemName.Text = "";
                txtItemStock.Text = "";
                txtItemPrice.Text = "";
            }
            catch (Exception)
            {

            }
        }

        private void cbSupplier_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtItemName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z!]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtItemStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void txtItemPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void txtName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^A-Za-z]+$");
            e.Handled = regex.IsMatch(e.Text);
        }


        private void dataGridItemTransaction_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var data = dataGridItemTransaction.SelectedItem;
            //string id = (dataGridItemTransaction.SelectedCells[0].Column.GetCellContent(data) as TextBlock).Text;
            //txtTransactionId.Text = id;
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtTransactionId.Text == "")
                {
                    MessageBox.Show("Add Transaction First");
                    btnAddTrans.Focus();
                }
                else
                {
                    if (txtQuantity.Text == "")
                    {
                        txtQuantity.Focus();
                        MessageBox.Show("Quantity is required", "Caution", MessageBoxButton.OK);
                    }
                    else
                    {
                        int transid = Convert.ToInt32(txtTransactionId.Text);
                        int vPrice = Convert.ToInt32(txtPrice.Text);
                        int vQuantity = Convert.ToInt32(txtQuantity.Text);
                        var itemname = myContext.Items.Where(y => y.Id == cb_itemCode).FirstOrDefault();
                        var transitem = myContext.Transaction.Where(m => m.Id == transid).FirstOrDefault();
                        int total = vPrice * vQuantity;
                        grandTotal += total;
                        TransList.Add(new ListTransaction { Quantity = vQuantity, Transaction = transitem, Item = itemname });
                        dataGridItemTransaction.Items.Add(new
                        {
                            Id = txtItemIdTransaction.Text,
                            Name = cbItemCode.Text,
                            Price = txtPrice.Text,
                            Quantity = txtQuantity.Text,
                            Total = total.ToString()
                        });
                        txtGrandTotal.Text = grandTotal.ToString();
                        txtItemIdTransaction.Text = "";
                        txtPrice.Text = "";
                        txtQuantity.Text = "";
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            if (txtPay.Text == "")
            {
                MessageBox.Show("Input Pay First");
                txtPay.Focus();
            }
            else
            {
                if (grandTotal <= pay)
                {
                    pay = Convert.ToInt32(txtPay.Text);
                    int change = pay - grandTotal;
                    var trans = myContext.Transaction.FirstOrDefault(t => t.Id.ToString() == txtTransactionId.Text);
                    int totPrice = Convert.ToInt32(txtGrandTotal.Text);
                    trans.PriceTotal = totPrice;
                    foreach (var s in TransList)
                    {
                        myContext.ListTransaction.Add(s);
                        myContext.SaveChanges();
                        laporan += s.Item.Id.ToString() + "\t" + s.Item.Name.ToString() + "\t" + s.Item.Price.ToString() + "\t" + s.Quantity + "\n";
                    }
                    MessageBox.Show("The changes = " + change.ToString("n0"));
                    txtChanges.Text = change.ToString("n0");
                    using (PdfDocument document = new PdfDocument())
                    {
                        //Add a page to the document
                        PdfPage page = document.Pages.Add();

                        //Create PDF graphics for the page
                        PdfGraphics graphics = page.Graphics;

                        //Set the standard font
                        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                        //Draw the text
                        graphics.DrawString(laporan, font, PdfBrushes.Black, new PointF(0, 0));

                        //Save the document
                        document.Save("Output.pdf");

                        #region View the Workbook
                        //Message box confirmation to view the created document.
                        if (MessageBox.Show("Do you want to view the PDF?", "PDF has been created",
                            MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                        {
                            try
                            {
                                //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
                                System.Diagnostics.Process.Start("Output.pdf");

                                //Exit
                                Close();
                            }
                            catch (Win32Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                        else
                            Close();
                        #endregion
                    }
                }
                else
                {
                    MessageBox.Show("Payment Invalid");
                    txtPay.Focus();
                }
            }

            //using (PdfDocument document = new PdfDocument())
            //{
            //    //Add a page to the document
            //    PdfPage page = document.Pages.Add();

            //    //Create PDF graphics for the page
            //    PdfGraphics graphics = page.Graphics;

            //    //Set the standard font
            //    PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //    //Draw the text
            //    graphics.DrawString("", font, PdfBrushes.Black, new PointF(0, 0));

            //    //Save the document
            //    document.Save("Output.pdf");

            //    #region View the Workbook
            //    //Message box confirmation to view the created document.
            //    if (MessageBox.Show("Do you want to view the PDF?", "PDF has been created",
            //        MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            //    {
            //        try
            //        {
            //            //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
            //            System.Diagnostics.Process.Start("Output.pdf");

            //            //Exit
            //            Close();
            //        }
            //        catch (Win32Exception ex)
            //        {
            //            Console.WriteLine(ex.ToString());
            //        }
            //    }
            //    else
            //        Close();
            //    #endregion
            //}
        }

        private void btnClearTransaction_Click(object sender, RoutedEventArgs e)
        {
            dataGridItemTransaction.Items.Clear();
            txtGrandTotal.Text = null;
        }

        private void cbItemCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_itemCode = Convert.ToInt32(cbItemCode.SelectedValue.ToString());
            var itemcode = myContext.Items.Where(i => i.Id == cb_itemCode).FirstOrDefault();
            //txtItemId.Text = itemcode.Id.ToString();
            txtItemIdTransaction.Text = itemcode.Id.ToString();
            txtPrice.Text = itemcode.Price.ToString();
        }

        private void txtTransactionDate_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtItemIdTransaction_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+$");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteOneItem_Click(object sender, RoutedEventArgs e)
        {
            var data = dataGridItemTransaction.SelectedItem;
            string subtotal = (dataGridItemTransaction.SelectedCells[4].Column.GetCellContent(data) as TextBlock).Text;
            int subtotalint = Convert.ToInt32(subtotal);
            int totalgrand = Convert.ToInt32(txtGrandTotal.Text);
            if (dataGridItemTransaction.SelectedItem != null)
            {
                int totalafter = totalgrand - subtotalint;
                grandTotal -= subtotalint;
                txtGrandTotal.Text = totalafter.ToString();
                dataGridItemTransaction.Items.RemoveAt(dataGridItemTransaction.SelectedIndex);
            }
            else
            {
                txtGrandTotal.Clear();
            }
        }

        private void txtGrandTotal_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtPay_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                grandTotal = Convert.ToInt32(txtGrandTotal.Text);
                pay = Convert.ToInt32(txtPay.Text);
                txtChanges.Text = (pay - grandTotal).ToString("n0");
            }
            catch (Exception)
            {

            }
        }

        private void txtPay_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnAddTrans_Click(object sender, RoutedEventArgs e)
        {
            var addTrans = new Transaction();
            myContext.Transaction.Add(addTrans);
            myContext.SaveChanges();
            txtTransactionId.Text = Convert.ToString(addTrans.Id);
        }

        private void cbUserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_role = Convert.ToInt32(cbUserRole.SelectedValue.ToString());
        }

        private void btnUserPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((txtUsername.Text == "") || (txtUserEmail.Text == "") || (cbUserRole.Text == ""))
                {
                    if (txtUsername.Text == "")
                    {
                        MessageBox.Show("Name is required", "Caution", MessageBoxButton.OK);
                        txtName.Focus();
                    }
                    else if (txtUserEmail.Text == "")
                    {
                        MessageBox.Show("Email is required", "Caution", MessageBoxButton.OK);
                        txtEmail.Focus();
                    }
                    else if (cbUserRole.Text == "")
                    {
                        MessageBox.Show("Role is required", "Caution", MessageBoxButton.OK);
                        cbUserRole.Focus();
                    }
                }
                else
                {
                    string userpass = Guid.NewGuid().ToString();
                    var checkuseremail = myContext.Users.FirstOrDefault(u => u.Email == txtUserEmail.Text);
                    var userrole = myContext.Roles.FirstOrDefault(r => r.Id == cb_role);
                    if (checkuseremail == null)
                    {
                        var push = new User(txtUsername.Text, txtUserEmail.Text, userpass, userrole);
                        myContext.Users.Add(push);
                        var result = myContext.SaveChanges();
                        if (result > 0)
                        {
                            MessageBox.Show("row has been inserted");
                        }
                        datagridlistuser.ItemsSource = myContext.Users.ToList();
                        Outlook._Application _app = new Outlook.Application();
                        Outlook.MailItem mail = (Outlook.MailItem)_app.CreateItem(Outlook.OlItemType.olMailItem);
                        mail.To = txtUserEmail.Text;
                        mail.Body = "Hi " + txtUsername.Text + "\nThis Is Your Password : " + userpass;
                        mail.Importance = Outlook.OlImportance.olImportanceNormal;
                        ((Outlook._MailItem)mail).Send();
                        MessageBox.Show("Your email has been sent!", "Message", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Email has been used");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
            }
            txtUsername.Text = "";
            txtUserEmail.Text = "";
        }

        private void datagridlistuser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void txtUserEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void txtUserEmail_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void gridTitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnLogout_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void txtRoleName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtRoleName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void btnSubmitRole_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtRoleName.Text == "")
                {
                        MessageBox.Show("Rule Name is required", "Caution", MessageBoxButton.OK);
                        txtName.Focus();
                }
                else
                {
                    var checkrole = myContext.Roles.FirstOrDefault(r => r.Name == txtRoleName.Text);
                    if (checkrole == null)
                    {
                        var push = new Role(txtRoleName.Text);
                        myContext.Roles.Add(push);
                        var result = myContext.SaveChanges();
                        if (result > 0)
                        {
                            MessageBox.Show("Role has been inserted");
                        }
                        datagridlistrole.ItemsSource = myContext.Roles.ToList();
                    }
                    else
                    {
                        MessageBox.Show("Role has been used");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK);
            }
            txtRoleName.Text = "";
            cbUserRole.ItemsSource = myContext.Roles.ToList();
        }

        private void datagridlistrole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        //private void btnPDF_Click(object sender, RoutedEventArgs e)
        //{
        //    using (PdfDocument document = new PdfDocument())
        //    {
        //        //Add a page to the document
        //        PdfPage page = document.Pages.Add();

        //        //Create PDF graphics for the page
        //        PdfGraphics graphics = page.Graphics;

        //        //Set the standard font
        //        PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

        //        //Draw the text
        //        graphics.DrawString("you've been inputted new supplier\n" + "the name is : " + txtName.Text + "\n" + "the email is : " + txtEmail.Text, font, PdfBrushes.Black, new PointF(0, 0));

        //        //Save the document
        //        document.Save("Output.pdf");

        //        #region View the Workbook
        //        //Message box confirmation to view the created document.
        //        if (MessageBox.Show("Do you want to view the PDF?", "PDF has been created",
        //            MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
        //        {
        //            try
        //            {
        //                //Launching the Excel file using the default Application.[MS Excel Or Free ExcelViewer]
        //                System.Diagnostics.Process.Start("Output.pdf");

        //                //Exit
        //                Close();
        //            }
        //            catch (Win32Exception ex)
        //            {
        //                Console.WriteLine(ex.ToString());
        //            }
        //        }
        //        else
        //            Close();
        //        #endregion
        //    }
        //}

        //private void btnpdf_click(object sender, routedeventargs e)
        //{
        //    using (pdfdocument document = new pdfdocument())
        //    {
        //        //add a page to the document
        //        pdfpage page = document.pages.add();

        //        //create pdf graphics for the page
        //        pdfgraphics graphics = page.graphics;

        //        //set the standard font
        //        pdffont font = new pdfstandardfont(pdffontfamily.helvetica, 20);

        //        //draw the text
        //        graphics.drawstring("you've been inputted new supplier\n" + "the name is : " + txtname.text + "\n" + "the email is : " + txtemail.text, font, pdfbrushes.black, new pointf(0, 0));

        //        //save the document
        //        document.save("output.pdf");

        //        #region view the workbook
        //        //message box confirmation to view the created document.
        //        if (messagebox.show("do you want to view the pdf?", "pdf has been created",
        //            messageboxbutton.yesno, messageboximage.information) == messageboxresult.yes)
        //        {
        //            try
        //            {
        //                //launching the excel file using the default application.[ms excel or free excelviewer]
        //                system.diagnostics.process.start("output.pdf");

        //                //exit
        //                close();
        //            }
        //            catch (win32exception ex)
        //            {
        //                console.writeline(ex.tostring());
        //            }
        //        }
        //        else
        //            close();
        //        #endregion
        //    }
        //}
    }
}
