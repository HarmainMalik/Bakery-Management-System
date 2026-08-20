using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SweetBakery.BusinessLogic;

namespace UI
{
    public partial class OwnerPanel : Form
    {

        private readonly AdminService _adminService;
        private readonly SalesService _salesService;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        public OwnerPanel(AdminService adminSvc, SalesService salesSvc,
             ProductService prodSvc, CustomerService custSvc)
        {
            InitializeComponent();
            _adminService = adminSvc;
            _salesService = salesSvc;
            _productService = prodSvc;
            _customerService = custSvc;

            this.Text = "Owner Panel";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblStatus = new Label();
            lblStatus.Location = new Point(200, 15);
            lblStatus.Size = new Size(650, 25);
            lblStatus.Font = new Font("Arial", 10, FontStyle.Bold);
            lblStatus.Text = "Select an option from the left";
            this.Controls.Add(lblStatus);

            dataGrid = new DataGridView();
            dataGrid.Location = new Point(200, 45);
            dataGrid.Size = new Size(670, 480);
            dataGrid.ReadOnly = true;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.BackgroundColor = Color.White;
            this.Controls.Add(dataGrid);

            btnApprove = new Button();
            btnApprove.Text = "Approve";
            btnApprove.Location = new Point(200, 535);
            btnApprove.Size = new Size(90, 30);
            btnApprove.Visible = false;
            btnApprove.BackColor = Color.Green;
            btnApprove.ForeColor = Color.White;
            btnApprove.Click += btnApprove_Click;
            this.Controls.Add(btnApprove);

            btnDeny = new Button();
            btnDeny.Text = "Deny";
            btnDeny.Location = new Point(300, 535);
            btnDeny.Size = new Size(90, 30);
            btnDeny.Visible = false;
            btnDeny.BackColor = Color.Red;
            btnDeny.ForeColor = Color.White;
            btnDeny.Click += btnDeny_Click;
            this.Controls.Add(btnDeny);

            string[] btnTexts = {
                "Admin Requests",
                "All Sales",
                "Low Stock",
                "Expiring Tomorrow",
                "All Admins",
                "All Customers",
                "Logout"
            };
            string[] btnText = {
                "Admin Requests",
                "All Sales",
                "Low Stock",
                "Expiring Tomorrow",
                "All Admins",
                "All Customers",
                "Logout"
            };

            EventHandler[] btnEvents = {
                btnAdminReqs_Click,
                btnAllSales_Click,
                btnLowStock_Click,
                btnExpiring_Click,
                btnAllAdmins_Click,
                btnAllCust_Click,
                btnLogout_Click
            };

            for (int i = 0; i < btnTexts.Length; i++)
            {
                var btn = new Button();
                btn.Text = btnTexts[i];
                btn.Location = new Point(10, 10 + i * 55);
                btn.Size = new Size(175, 40);
                btn.BackColor = (btnTexts[i] == "Logout") ? Color.IndianRed : Color.SteelBlue;
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Click += btnEvents[i];
                this.Controls.Add(btn);
            }
        }

        
        private void ClearGrid()
        {
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
            btnApprove.Visible = false;
            btnDeny.Visible = false;
        }

        private void btnAdminReqs_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var pending = _adminService.GetPending();
            lblStatus.Text = "Pending Requests: " + pending.Count;
            dataGrid.Columns.Add("Id", "Admin ID");
            dataGrid.Columns.Add("Pass", "Password");
            foreach (var a in pending)
                dataGrid.Rows.Add(a.Id, a.Password);
            btnApprove.Visible = true;
            btnDeny.Visible = true;
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentRow == null) return;
            string id = dataGrid.CurrentRow.Cells["Id"].Value.ToString();
            var (ok, err) = _adminService.ApproveAdmin(id);
            MessageBox.Show(ok ? "Admin approved!" : err);
            btnAdminReqs_Click(sender, e);
        }

        private void BtnDeny_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentRow == null) return;
            string id = dataGrid.CurrentRow.Cells["Id"].Value.ToString();
            var (ok, err) = _adminService.DenyAdmin(id);
            MessageBox.Show(ok ? "Admin denied." : err);
            btnAdminReqs_Click(sender, e);
        }

        private void BtnAllSales_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var sales = _salesService.GetAll();
            lblStatus.Text = "Sales: " + sales.Count + "  |  Total Revenue: " + _salesService.TotalRevenue();
            dataGrid.Columns.Add("Product", "Product");
            dataGrid.Columns.Add("Qty", "Quantity");
            dataGrid.Columns.Add("Total", "Total Price");
            foreach (var s in sales)
                dataGrid.Rows.Add(s.ProductName, s.Quantity, s.TotalPrice);
        }

        private void BtnLowStock_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var items = _productService.GetLowStock();
            lblStatus.Text = "Low Stock Items: " + items.Count;
            dataGrid.Columns.Add("Name", "Product");
            dataGrid.Columns.Add("Stock", "Stock");
            dataGrid.Columns.Add("Price", "Price");
            foreach (var p in items)
                dataGrid.Rows.Add(p.Name, p.Stock, p.Price);
        }

        private void BtnExpiring_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var items = _productService.GetExpiringTomorrow();
            lblStatus.Text = "Expiring Tomorrow: " + items.Count;
            dataGrid.Columns.Add("Name", "Product");
            dataGrid.Columns.Add("Stock", "Stock");
            dataGrid.Columns.Add("Exp", "Expiry");
            foreach (var p in items)
                dataGrid.Rows.Add(p.Name, p.Stock, p.ExpDay + "/" + p.ExpMonth + "/" + p.ExpYear);
        }

        private void btnAllAdmins_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var admins = _adminService.GetAll();
            lblStatus.Text = "All Admins: " + admins.Count;
            dataGrid.Columns.Add("Id", "Admin ID");
            dataGrid.Columns.Add("Approved", "Approved");
            dataGrid.Columns.Add("Active", "Active");
            foreach (var a in admins)
                dataGrid.Rows.Add(a.Id, a.Approved ? "Yes" : "No", a.Active ? "Yes" : "No");
        }

        private void btnAllCust_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var custs = _customerService.GetAll();
            lblStatus.Text = "All Customers: " + custs.Count;
            dataGrid.Columns.Add("Id", "Customer ID");
            dataGrid.Columns.Add("Active", "Active");
            foreach (var c in custs)
                dataGrid.Rows.Add(c.Id, c.Active ? "Yes" : "No");
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    

        private void button5_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var admins = _adminService.GetAll();
            lblStatus.Text = "All Admins: " + admins.Count;
            dataGrid.Columns.Add("Id", "Admin ID");
            dataGrid.Columns.Add("Approved", "Approved");
            dataGrid.Columns.Add("Active", "Active");
            foreach (var a in admins)
                dataGrid.Rows.Add(a.Id,
                    a.Approved ? "Yes" : "No",
                    a.Active ? "Yes" : "No");
        }

        private void btnExpiring_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var items = _productService.GetExpiringTomorrow();
            lblStatus.Text = "Expiring Tomorrow: " + items.Count;
            dataGrid.Columns.Add("Name", "Product");
            dataGrid.Columns.Add("Stock", "Stock");
            dataGrid.Columns.Add("Exp", "Expiry");
            foreach (var p in items)
                dataGrid.Rows.Add(p.Name, p.Stock, p.ExpDay + "/" + p.ExpMonth + "/" + p.ExpYear);
        }

        private void btnDeny_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentRow == null) return;
            string id = dataGrid.CurrentRow.Cells["Id"].Value.ToString();
            var (ok, err) = _adminService.DenyAdmin(id);
            MessageBox.Show(ok ? "Admin denied." : err);
            btnAdminReqs_Click(sender, e);
        }

        private void btnLowStock_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var items = _productService.GetLowStock();
            lblStatus.Text = "Low Stock Items: " + items.Count;
            dataGrid.Columns.Add("Name", "Product");
            dataGrid.Columns.Add("Stock", "Stock");
            dataGrid.Columns.Add("Price", "Price");
            foreach (var p in items)
                dataGrid.Rows.Add(p.Name, p.Stock, p.Price);
        }

        private void btnAllSales_Click(object sender, EventArgs e)
        {
            ClearGrid();
            var sales = _salesService.GetAll();
            lblStatus.Text = "Sales: " + sales.Count + "  |  Total Revenue: " + _salesService.TotalRevenue();
            dataGrid.Columns.Add("Product", "Product");
            dataGrid.Columns.Add("Qty", "Quantity");
            dataGrid.Columns.Add("Total", "Total Price");
            foreach (var s in sales)
                dataGrid.Rows.Add(s.ProductName, s.Quantity, s.TotalPrice);
        }

        private void OwnerPanel_Load(object sender, EventArgs e)
        {

        }

        private void btnAdminReqs_Click_1(object sender, EventArgs e)
        {
            ClearGrid();
            var pending = _adminService.GetPending();
            lblStatus.Text = "Pending Admin Requests: " + pending.Count;
            dataGrid.Columns.Add("Id", "Admin ID");
            dataGrid.Columns.Add("Pass", "Password");
            foreach (var a in pending)
                dataGrid.Rows.Add(a.Id, a.Password);
            btnApprove.Visible = true;
            btnDeny.Visible = true;
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
