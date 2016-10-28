using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RMS.DATAACCESS;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
namespace RMS.GUI
{
    public partial class RMS : MetroFramework.Forms.MetroForm
    {
       private double orderTotal, vatTotal, netTotal, discount;

        public RMS()
        {
            InitializeComponent();
            itemCategoryComboboxFill();
            itemListCategoryComboboxDefaultSelect();
            autoCompleteItemNameTbox();
            autoCompleteItemCategoryTbox();
            
        }

        void itemCategoryComboboxFill()
        {
            try
            {
                string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                OracleConnection conn = new OracleConnection(connStr);

                string sql = "select distinct category from item";
                OracleCommand cmd = new OracleCommand(sql, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                itemListCategoryCombobox.Items.Clear();
                placeOrderItemCategoryCbox.Items.Clear();
                itemListCategoryCombobox.Items.Add("ALL");

                while (dr.Read())
                {
                    string category = dr.GetString(0);

                    itemListCategoryCombobox.Items.Add(category);
                    placeOrderItemCategoryCbox.Items.Add(category);
                }



                conn.Close();
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }



        }

        void itemNameComboboxFill(string category)
        {
            try
            {
                string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                OracleConnection conn = new OracleConnection(connStr);

                string sql = "select name from item where category='" + category + "'";
                OracleCommand cmd = new OracleCommand(sql, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                placeOrderItemNameCbox.Items.Clear();             

                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    placeOrderItemNameCbox.Items.Add(name);
                }

                conn.Close();
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }

        }
        private void addItemBtn_Click(object sender, EventArgs e)
        {
            string name, category;
            double unitPrice;

            try
            {
                ItemDataAccess ob = new ItemDataAccess();
                name = addItemName.Text.ToString().ToUpper();
                category = addCategoryName.Text.ToString().ToUpper();
                if (name.CompareTo("") != 0 && category.CompareTo("") != 0 && addPrice.Text.ToString().CompareTo("") != 0)
                {
                    unitPrice = Convert.ToDouble(addPrice.Text);
                    int rowCount = ob.addItem(name, category, unitPrice);
                    if (rowCount <= 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Item Added");
                        itemCategoryComboboxFill();
                        itemListCategoryComboboxDefaultSelect();
                        autoCompleteItemNameTbox();
                        autoCompleteItemCategoryTbox();

                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Error: An Item Is Already Registered With This Name");
                    }

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                addItemName.Text = "";
                addCategoryName.Text = "";
                addPrice.Text = "";

            }

            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }


        }

        private void editItemNameBtn_Click(object sender, EventArgs e)
        {
            string curName, newName, query;

            try
            {
                ItemDataAccess ob = new ItemDataAccess();
                curName = editItemNameCin.Text.ToString().ToUpper();
                newName = editItemNameNin.Text.ToString().ToUpper();
                if (curName.CompareTo("") != 0 && newName.CompareTo("") != 0)
                {
                    query = "UPDATE ITEM Set Name='" + newName + "' WHERE Name='" + curName + "'";
                    int rowCount = ob.editItem(query);
                    if (rowCount > 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Item Name Edited Successfully");
                        itemCategoryComboboxFill();
                        itemListCategoryComboboxDefaultSelect();
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Current Item Name Not Found ");
                    }

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                editItemNameCin.Text = "";
                editItemNameNin.Text = "";
                autoCompleteItemNameTbox();

            }

            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
        }

        private void editCategoryNameBtn_Click(object sender, EventArgs e)
        {
            string curName, newCategory, query;

            try
            {
                ItemDataAccess ob = new ItemDataAccess();
                curName = editCategoryNameCin.Text.ToString().ToUpper();
                newCategory = editCategoryNameNcn.Text.ToString().ToUpper();
                if (curName.CompareTo("") != 0 && newCategory.CompareTo("") != 0)
                {
                    query = "UPDATE ITEM Set Category='" + newCategory + "' WHERE Name='" + curName + "'";
                    int rowCount = ob.editItem(query);
                    if (rowCount > 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Item Category Edited Successfully");
                        itemCategoryComboboxFill();
                        itemListCategoryComboboxDefaultSelect();
                        autoCompleteItemCategoryTbox();

                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Current Item Name Not Found ");
                    }

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                editCategoryNameCin.Text = "";
                editCategoryNameNcn.Text = "";

            }

            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
        }

        private void editItemPriceBtn_Click(object sender, EventArgs e)
        {
            string curName, query;
            double newPrice;
            try
            {
                ItemDataAccess ob = new ItemDataAccess();
                curName = editItemPriceCin.Text.ToString().ToUpper();

                if (curName.CompareTo("") != 0 && editItemPriceNip.Text.ToString().CompareTo("") != 0)
                {
                    newPrice = Convert.ToDouble(editItemPriceNip.Text.ToString());
                    query = "UPDATE ITEM Set Price=" + newPrice + " WHERE Name='" + curName + "'";
                    int rowCount = ob.editItem(query);
                    if (rowCount > 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Item Price Edited Successfully");
                        itemCategoryComboboxFill();
                        itemListCategoryComboboxDefaultSelect();
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Current Item Name Not Found ");
                    }

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                editItemPriceCin.Text = "";
                editItemPriceNip.Text = "";

            }

            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
        }

        private void itemListCategoryCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sql;
                string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                OracleConnection conn = new OracleConnection(connStr);
                if (itemListCategoryCombobox.SelectedItem.ToString().CompareTo("ALL") == 0)
                {
                    sql = "select name, category, price from item";
                }
                else
                {
                    sql = "select name, category, price from item where category = '" + itemListCategoryCombobox.SelectedItem.ToString() + "'";
                }

                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                itemListGrid.DataSource = dt;
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }

        }
        void itemListCategoryComboboxDefaultSelect()
        {
            try
            {
                string sql;
                string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                OracleConnection conn = new OracleConnection(connStr);
                itemListCategoryCombobox.SelectedItem = "ALL";
                if (itemListCategoryCombobox.SelectedItem.ToString().CompareTo("ALL") == 0)
                {
                    sql = "select name, category, price from item";
                }
                else
                {
                    sql = "select name, category, price from item where category = '" + itemListCategoryCombobox.SelectedItem.ToString() + "'";
                }

                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                itemListGrid.DataSource = dt;
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
        }



        

        private void editItemPasswordBtn_Click(object sender, EventArgs e)
        {

            string password;

            try
            {
                AdminDataAccess ob = new AdminDataAccess();
                password = editItemPasswordTextBox.Text.ToString();
                if (password.CompareTo("") != 0)
                {
                    if (password.CompareTo(ob.getAdminPassword()) != 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Password");
                    }
                    else
                    {
                        editItemPasswordPanel.Hide();
                        
                    }

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                editItemPasswordTextBox.Text = "";


            }


            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }

        }

        private void changeAdminPasswordBtn_Click(object sender, EventArgs e)
        {
            string curAdminPassword, newAdminPassword, re_NewAdminPassword;

            try
            {
                AdminDataAccess ob = new AdminDataAccess();
                curAdminPassword = changeAdminPasswordCp.Text.ToString();
                newAdminPassword = changeAdminPasswordNp.Text.ToString();
                re_NewAdminPassword = changeAdminPasswordCnp.Text.ToString();

                if (curAdminPassword.CompareTo("") != 0 && newAdminPassword.CompareTo("") != 0 && re_NewAdminPassword.CompareTo("") != 0)
                {
                    if (newAdminPassword.CompareTo(re_NewAdminPassword) == 0)
                    {
                        string query = "UPDATE ADMINPASSWORD Set Password='" + newAdminPassword + "' WHERE Password='" + curAdminPassword + "'";
                        int rowCount = ob.updateAdminPassword(query);
                        if (rowCount > 0)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Password Changed Successfully");
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Wrong Current Password");
                        }
                    }

                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Password does not match the confirm password.\nType both passwords again.");
                    }


                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                changeAdminPasswordCp.Text = "";
                changeAdminPasswordNp.Text = "";
                changeAdminPasswordCnp.Text = "";

            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
        }

        private void placeOrderItemCategoryCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = placeOrderItemCategoryCbox.Text.ToString();
            itemNameComboboxFill(category);
        }

        private void placeOrderAddItemBtn_Click(object sender, EventArgs e)
        {
            if (placeOrderItemNameCbox.Text.ToString().CompareTo("") != 0)
            {
                try
                {

                    ItemDataAccess itemOb = new ItemDataAccess();
                    OrderInfoDataAccess orderOb = new OrderInfoDataAccess();
                    string itemName = placeOrderItemNameCbox.Text.ToString();
                    double itemUnitPrice = itemOb.getItemUnitPrice(itemName);
                    int qty = Convert.ToInt32(placeOrderQtyUpDown.Value);
                    int orderId;

                    if (placeOrderIdLabel.Text.ToString().CompareTo("") == 0)
                    {
                        string sql = "Insert into ORDERINFO (ID,STATUS) VALUES(order_orderid_seq.NEXTVAL,'FALSE')";
                        orderOb.addOrderDetails(sql);
                        orderId = orderOb.getCurrentOrderId();
                        placeOrderIdLabel.Text = Convert.ToString(orderId);
                        placeOrder_orderTotal.Text = "0.0";
                        placeOrder_vatTotal.Text = "0.0";
                        placeOrder_netTotal.Text = "0.0";
                        placeOrder_discountTbox.Text = "0.0";
                    }
                    orderId = Convert.ToInt32(placeOrderIdLabel.Text.ToString());
                    string addOrderdItemSql = "Insert into ORDEREDITEM VALUES(" + orderId + "," + qty + "," + itemUnitPrice + ",'" + itemName + "')";
                    orderOb.addOrderDetails(addOrderdItemSql);


                    string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                    OracleConnection conn = new OracleConnection(connStr);

                    string orderedItemListSql = "select ITEMNAME, ITEMUNITPRICE, ITEMQTY from ORDEREDITEM where ORDERID =" + orderId + "";


                    OracleDataAdapter adapter = new OracleDataAdapter(orderedItemListSql, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    placeOrderItemList.DataSource = dt;
                  
                    
                    orderTotal = orderTotal + (itemUnitPrice * qty);
                    vatTotal = orderTotal * 0.15;
                    netTotal = orderTotal + vatTotal;
                    placeOrder_orderTotal.Text = Convert.ToString(orderTotal);
                    placeOrder_vatTotal.Text = Convert.ToString(vatTotal);
                    placeOrder_netTotal.Text = Convert.ToString(netTotal);

                    placeOrderItemNameCbox.Items.Clear();
                    placeOrderItemCategoryCbox.Items.Clear();
                    itemCategoryComboboxFill();
                    itemListCategoryComboboxDefaultSelect();
                    placeOrderQtyUpDown.Value=placeOrderQtyUpDown.Minimum;
                }

                catch (Exception exc)
                {
                    MetroFramework.MetroMessageBox.Show(this, exc.ToString());
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "No Item Selected");
            }



        }

       

        private void placeOrderPaymentMethodCbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (placeOrderPaymentMethodCbox.Text.ToString().CompareTo("CASH") == 0)
            {
                placeOrderPaymentMethodCardPanel.Hide();
                payemnetMethodCashPanel.Show();
                
            }
            else if (placeOrderPaymentMethodCbox.Text.ToString().CompareTo("CARD") == 0)
            {
                payemnetMethodCashPanel.Hide();
                placeOrderPaymentMethodCardPanel.Show();
            }

            else
            {
                payemnetMethodCashPanel.Hide();
                placeOrderPaymentMethodCardPanel.Hide();
            }
        }

        private void placeOrderPaymentMethodCashFinishBtn_Click(object sender, EventArgs e)
        {
            if (placeOrderIdLabel.Text.ToString().CompareTo("") != 0)
            {
                try
                {
                    

                    if (placeOrderCashPaidTbox.Text.ToString().CompareTo("") != 0)
                    {
                        if (Convert.ToDouble(placeOrderCashPaidTbox.Text.ToString()) < 0)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Wrong Input: Negative Values Not Allowed");
                        }
                        else
                        {

                            if (Convert.ToDouble(placeOrderCashPaidTbox.Text.ToString()) >= netTotal)
                            {
                                double cashPaid = Convert.ToDouble(placeOrderCashPaidTbox.Text.ToString());
                                double changeDue = cashPaid - netTotal;
                                DateTime dtValue = DateTime.Now;
                                string orderDate = dtValue.ToString("yyyy-MM-dd");
                                string orderTime = dtValue.ToString("HH:mm:ss");
                                OrderInfoDataAccess orderOb = new OrderInfoDataAccess();
                                double discountOrderTotal = orderTotal - discount;
                                string query = "update orderinfo set orderdate='" + orderDate + "', ordertime='" + orderTime + "', ordertotal=" + discountOrderTotal + ", discount=" + discount + ", vattotal=" + vatTotal + ", nettotal=" + netTotal + ", paymentmethod='" + placeOrderPaymentMethodCbox.Text.ToString() + "', cashpaid='" + cashPaid + "', change=" + changeDue + ",status='TRUE' where id=" + Convert.ToInt32(placeOrderIdLabel.Text.ToString());
                                orderOb.addOrderDetails(query);
                                placeOrderChangeDueLbl.Text = "Change Due : " + changeDue;
                                MetroFramework.MetroMessageBox.Show(this, "Order Details Updated To Database Successfully");
                                placeOrderPaymentMethodCashFinishBtn.Hide();
                                placeOrderPaymentMethodCardFinishBtn.Hide();
                                placeOrderAddItemBtn.Hide();
                                placeOrderCancelResetBtn.Text = "New Order";
                                placeOrder_discountTbox.ReadOnly = true;
                                placeOrderCashPaidTbox.ReadOnly = true;
                                placeOrderCardNo.ReadOnly = true;
                                placeOrderCardType.Enabled = false;
                                placeOrderPaymentMethodCbox.Enabled = false;
                            }
                            else
                            {
                                MetroFramework.MetroMessageBox.Show(this, "Wrong Input: Insufficient Amount Paid");
                            }
                        }
                    }



                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                    }
                }

                catch (FormatException)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
                }

                catch (Exception exc)
                {
                    MetroFramework.MetroMessageBox.Show(this, exc.ToString());
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: No Item Is Ordered Yet");
            }
        }


        private void placeOrder_discountTbox_TextChanged(object sender, EventArgs e)
        {
            try
            {

                if (placeOrder_discountTbox.Text.ToString().CompareTo("") != 0)
                {

                    if (placeOrder_discountTbox.Text.ToString().CompareTo("-") != 0)
                    {
                        if (Convert.ToDouble(placeOrder_discountTbox.Text.ToString()) <= orderTotal)
                        {
                            double discount_orderTotal;
                            discount = Convert.ToDouble(placeOrder_discountTbox.Text.ToString());
                            discount_orderTotal = orderTotal - discount;
                            vatTotal = discount_orderTotal * 0.15;
                            netTotal = discount_orderTotal + vatTotal;
                            placeOrder_orderTotal.Text = Convert.ToString(discount_orderTotal);
                            placeOrder_vatTotal.Text = Convert.ToString(vatTotal);
                            placeOrder_netTotal.Text = Convert.ToString(netTotal);
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Wrong Input: Discount Amount Can Not Be More Than Order Total");
                            placeOrder_discountTbox.Text = "";
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Wrong Input: Negative Values Not Allowed");
                        placeOrder_discountTbox.Text = "";
                    }
                }
                else
                {
                    double discount_orderTotal;
                    discount =0;
                    discount_orderTotal = orderTotal - discount;
                    vatTotal = discount_orderTotal * 0.15;
                    netTotal = discount_orderTotal + vatTotal;
                    placeOrder_orderTotal.Text = Convert.ToString(discount_orderTotal);
                    placeOrder_vatTotal.Text = Convert.ToString(vatTotal);
                    placeOrder_netTotal.Text = Convert.ToString(netTotal);
                }
            }
            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
                placeOrder_discountTbox.Text = "";
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
                placeOrder_discountTbox.Text = "";
            }
            

        }

        private void placeOrder_discountTbox_Leave(object sender, EventArgs e)
        {
            if (placeOrder_discountTbox.Text.ToString().CompareTo("") == 0)
            {
                placeOrder_discountTbox.Text = "0.0";
            }
        }

        private void placeOrder_discountTbox_Enter(object sender, EventArgs e)
        {
            if (placeOrder_discountTbox.Text.ToString().CompareTo("0.0") == 0)
            {
                placeOrder_discountTbox.Text = "";
            }
        }

        private void placeOrderPaymentMethodCardFinishBtn_Click(object sender, EventArgs e)
        {

            billprint();
            if (placeOrderIdLabel.Text.ToString().CompareTo("") != 0)
            {
                try
                {

                    if (placeOrderCardType.Text.ToString().CompareTo("") != 0 && placeOrderCardNo.Text.ToString().CompareTo("") != 0)
                    {
                        if (Convert.ToDouble(placeOrderCardNo.Text.ToString()) < 0)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Wrong Input: Negative Values Not Allowed");
                        }
                        else
                        {

                                double cardNo = Convert.ToDouble(placeOrderCardNo.Text.ToString());
                                String cardType = placeOrderCardType.Text.ToString();
                                DateTime dtValue = DateTime.Now;
                                string orderDate = dtValue.ToString("yyyy-MM-dd");
                                string orderTime = dtValue.ToString("HH:mm:ss");
                                OrderInfoDataAccess orderOb = new OrderInfoDataAccess();
                                double discountOrderTotal = orderTotal - discount;
                                string query = "update orderinfo set orderdate='" + orderDate + "', ordertime='" + orderTime + "', ordertotal=" + discountOrderTotal + ", discount=" + discount + ", vattotal=" + vatTotal + ", nettotal=" + netTotal + ", paymentmethod='" + placeOrderPaymentMethodCbox.Text.ToString() + "', cardtype='" + cardType + "', cardnum=" + cardNo + ",status='TRUE' where id=" + Convert.ToInt32(placeOrderIdLabel.Text.ToString());
                                orderOb.addOrderDetails(query);
                                MetroFramework.MetroMessageBox.Show(this, "Order Details Updated To Database Successfully");
                                placeOrderPaymentMethodCashFinishBtn.Hide();
                                placeOrderPaymentMethodCardFinishBtn.Hide();
                                placeOrderAddItemBtn.Hide();
                                placeOrderCancelResetBtn.Text = "New Order";
                                placeOrder_discountTbox.ReadOnly = true;
                                placeOrderCashPaidTbox.ReadOnly = true;
                                placeOrderCardNo.ReadOnly = true;
                                placeOrderCardType.Enabled = false;
                                placeOrderPaymentMethodCbox.Enabled = false;

                        }
                    }



                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                    }
                }

                catch (FormatException)
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
                }

                catch (Exception exc)
                {
                    MetroFramework.MetroMessageBox.Show(this, exc.ToString());
                }
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: No Item Is Ordered Yet");
            }

        }

        private void placeOrderCancelResetBtn_Click(object sender, EventArgs e)
        {     
            orderTotal=0;
            vatTotal=0;
            netTotal=0;
            discount = 0;
            placeOrderChangeDueLbl.Text = "";
            placeOrderIdLabel.Text = "";
            placeOrder_orderTotal.Text = "0.0";
            placeOrder_vatTotal.Text = "0.0";
            placeOrder_netTotal.Text = "0.0";
            placeOrderItemList.DataSource = null;
            placeOrderPaymentMethodCashFinishBtn.Show();
            placeOrderPaymentMethodCardFinishBtn.Show();
            placeOrderAddItemBtn.Show();
            placeOrderCancelResetBtn.Text = "Cancel Order";
            placeOrder_discountTbox.ReadOnly = false;
            placeOrder_discountTbox.Text = "0.0";
            placeOrderCashPaidTbox.ReadOnly = false;
            placeOrderCashPaidTbox.Text = "";
            placeOrderCardNo.ReadOnly = false;
            placeOrderCardNo.Text = "";
            placeOrderCardType.Enabled = true;
            placeOrderCardType.Items.Clear();
            placeOrderCardType.Items.Add("MASTER CARD");
            placeOrderCardType.Items.Add("VISA CARD");
            placeOrderCardType.Items.Add("AMERICAN EXPRESS");
            placeOrderPaymentMethodCbox.Enabled = true;
            placeOrderPaymentMethodCbox.Items.Clear();
            placeOrderPaymentMethodCbox.Items.Add("CASH");
            placeOrderPaymentMethodCbox.Items.Add("CARD");
            placeOrderPaymentMethodCardPanel.Hide();
            payemnetMethodCashPanel.Hide();
        }

        private void orderStatementBtn_Click(object sender, EventArgs e)
        {
            string dtFrom = orderStatementDtFrom.Value.ToString("yyyy-MM-dd");
            string dtTo = orderStatementDtTo.Value.ToString("yyyy-MM-dd");
       
            
            try
            {
                
                string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                OracleConnection conn = new OracleConnection(connStr);
                string query = "SELECT id,orderdate,ordertime,ordertotal,discount,vattotal,nettotal,paymentmethod,cashpaid,change,cardtype,cardnum FROM orderinfo WHERE orderdate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' and status='TRUE' order by id desc";
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                orderStatementDataGrid.DataSource = dt;
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
            
        }

        void autoCompleteItemNameTbox()
        {
            editCategoryNameCin.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            editCategoryNameCin.AutoCompleteSource=AutoCompleteSource.CustomSource;
            editItemNameCin.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            editItemNameCin.AutoCompleteSource = AutoCompleteSource.CustomSource;
            editItemPriceCin.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            editItemPriceCin.AutoCompleteSource = AutoCompleteSource.CustomSource;
            deleteItemCinTbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            deleteItemCinTbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            deleteItemRcinTbox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            deleteItemRcinTbox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

            try
            {
                string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                OracleConnection conn = new OracleConnection(connStr);

                string sql = "select name from item";
                OracleCommand cmd = new OracleCommand(sql, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string name = dr.GetString(0);
                    coll.Add(name);
                }

                conn.Close();
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
            editCategoryNameCin.AutoCompleteCustomSource = coll;
            editItemNameCin.AutoCompleteCustomSource = coll;
            editItemPriceCin.AutoCompleteCustomSource = coll;
            deleteItemCinTbox.AutoCompleteCustomSource = coll;
            deleteItemRcinTbox.AutoCompleteCustomSource = coll;
        }

        void autoCompleteItemCategoryTbox()
        {
            editCategoryNameNcn.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            editCategoryNameNcn.AutoCompleteSource = AutoCompleteSource.CustomSource;
            addCategoryName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            addCategoryName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

            try
            {
                string connStr = "Data Source=localhost; User Id= Tokee; Password=12345";
                OracleConnection conn = new OracleConnection(connStr);

                string sql = "select distinct category from item";
                OracleCommand cmd = new OracleCommand(sql, conn);
                conn.Open();
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    string category = dr.GetString(0);
                    coll.Add(category);
                }

                conn.Close();
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }

            editCategoryNameNcn.AutoCompleteCustomSource = coll;
            addCategoryName.AutoCompleteCustomSource = coll;
        }

          
        private void logOutEditItemCategory_Click(object sender, EventArgs e)
        {
            editItemPasswordPanel.Show();
        }

        private void logOutEditItemPrice_Click(object sender, EventArgs e)
        {
            editItemPasswordPanel.Show();
        }

        private void logOutEditItemName_Click(object sender, EventArgs e)
        {
            editItemPasswordPanel.Show();
        }

        private void logOutAddItem_Click(object sender, EventArgs e)
        {
            addItemPasswordPanel.Show();
        }

       
        private void deleteItemBtn_Click(object sender, EventArgs e)
        {
            string curName, reCurName, query;

            try
            {
                ItemDataAccess ob = new ItemDataAccess();
                curName = deleteItemCinTbox.Text.ToString().ToUpper();
                reCurName = deleteItemRcinTbox.Text.ToString().ToUpper();
                if (curName.CompareTo("") != 0 && reCurName.CompareTo("") != 0)
                {
                    if (curName.CompareTo(reCurName) == 0)
                    {
                        query = "Delete From Item Where Name='"+curName+"'";
                        int rowCount = ob.editItem(query);
                        if (rowCount > 0)
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Item Deleted Successfully");
                            itemCategoryComboboxFill();
                            itemListCategoryComboboxDefaultSelect();
                            autoCompleteItemCategoryTbox();
                            autoCompleteItemNameTbox();

                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "Current Item Name Not Found ");
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Wrong Input: Both Names Are Not Same.\nType Both Names Again.");                 
                    }
                   

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                deleteItemCinTbox.Text = "";
                deleteItemRcinTbox.Text = "";

            }

            catch (FormatException)
            {
                MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Input");
            }

            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
        }

        private void logOutDeleteItem_Click(object sender, EventArgs e)
        {
            editItemPasswordPanel.Show();
        }

        private void addItemPasswordBtn_Click(object sender, EventArgs e)
        {
            string password;

            try
            {
                AdminDataAccess ob = new AdminDataAccess();
                password = addItemPasswordTextBox.Text.ToString();
                if (password.CompareTo("") != 0)
                {
                    if (password.CompareTo(ob.getAdminPassword()) != 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "Error: Wrong Password");
                    }
                    else
                    {
                        addItemPasswordPanel.Hide();
                    }

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "Error: Input Can't be Blank");
                }
                addItemPasswordTextBox.Text = "";


            }


            catch (Exception exc)
            {
                MetroFramework.MetroMessageBox.Show(this, exc.ToString());
            }
        }

        private void metroTabPage2_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel35_Click(object sender, EventArgs e)
        {

        }

        private void metroLabel40_Click(object sender, EventArgs e)
        {

        }

        public void billprint()
        {
            Document bill = new Document(iTextSharp.text.PageSize.LETTER, 20, 20, 42, 35);
            PdfWriter write = PdfWriter.GetInstance(bill, new FileStream("bill.pdf", FileMode.Create));
            bill.Open();
            Paragraph p = new Paragraph("   ");
            bill.Add(p);
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("logo.png");
            logo.ScalePercent(50f);
            logo.SetAbsolutePosition(120,730);
            bill.Add(logo);
            /*This does smething*/
            
            string[,] billinfo=new string[placeOrderItemList.RowCount,4];

            for (int i = 0; i < placeOrderItemList.RowCount; i++)
            {
                for (int j = 0; j < placeOrderItemList.ColumnCount + 1; j++)
                {
                    if (j < 3)
                    {
                        billinfo[i, j] = placeOrderItemList[j, i].Value.ToString();
                    }
                    else
                    {
                        double a = Convert.ToDouble(placeOrderItemList[1,i].Value);
                        double b = Convert.ToDouble(placeOrderItemList[2,i].Value);
                        double c = a * b;
                        billinfo[i,j]=c.ToString();
                    }
                }
            }



            PdfPTable table = new PdfPTable(4);
            string[] header=new string[4];
            for (int i = 0; i < placeOrderItemList.ColumnCount; i++)
            {
              header[i]=placeOrderItemList.Columns[i].HeaderText;
            }
            header[3] = "TOTALPRICE";
            Paragraph p1 = new Paragraph("     "+header[0]+"       "+header[1]+"    "+header[2]+"    "+header[3]);
            p1.IndentationLeft = 200;
            bill.Add(p1);
            
            List l = new List(List.ORDERED);
            l.IndentationLeft = 200;
            for (int row = 0; row < placeOrderItemList.RowCount; row++)
            {
                for (int column = 0; column < placeOrderItemList.ColumnCount+1; column+=4)
                {
                    

                    if (row == 0)
                        l.Add(new ListItem(billinfo[row, column] + "            " + billinfo[row, column + 1] +"                      " + billinfo[row, column + 2] + "                    " + billinfo[row, column + 3]));
                    else
                        l.Add(billinfo[row, column] + "            " + billinfo[row, column + 1] + "                      " + billinfo[row, column + 2] + "                    " + billinfo[row, column + 3]);
                }
                
            }
            bill.Add(l);
            Paragraph l2 = new Paragraph("Order total          "+placeOrder_orderTotal.Text);
            l2.IndentationLeft = 400;
            bill.Add(l2);
            Paragraph l3 = new Paragraph("Vat total               " + placeOrder_vatTotal.Text);
            l3.IndentationLeft = 400;
            bill.Add(l3);
            Paragraph l4 = new Paragraph("Discount              " + placeOrder_discountTbox.Text);
            l4.IndentationLeft = 400;
            bill.Add(l4);
            Paragraph l5 = new Paragraph("Net total              " + placeOrder_netTotal.Text);
            l5.IndentationLeft = 400;
            bill.Add(l5);
           
                
              bill.Close();
              Process.Start("bill.pdf");
            
        }

        private void placeOrder_netTotal_Click(object sender, EventArgs e)
        {

        }
    }
}
