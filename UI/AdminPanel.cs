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
    public partial class AdminPanel : Form
    {
        private readonly ProductService _productService;
        private readonly SalesService _salesService;
       

        public AdminPanel(ProductService prodSvc, SalesService salesSvc)
        {
            InitializeComponent();
            _productService = prodSvc;
            _salesService = salesSvc;
            this.Text = "Admin Panel";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // ── Status label ──
            lblStatus = new Label();
            lblStatus.Location = new Point(200, 15);
            lblStatus.Size = new Size(650, 25);
            lblStatus.Font = new Font("Arial", 10, FontStyle.Bold);
            lblStatus.Text = "Select an option";
            this.Controls.Add(lblStatus);

            // ── DataGridView ──
            dataGrid = new DataGridView();
            dataGrid.Location = new Point(200, 45);
            dataGrid.Size = new Size(670, 500);
            dataGrid.ReadOnly = true;
            dataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGrid.AllowUserToAddRows = false;
            dataGrid.BackgroundColor = Color.White;
            this.Controls.Add(dataGrid);

            // ── Left side buttons ──
            string[] btnTexts = {
                "Refresh Products",
                "Add Product",
                "Delete Product",
                "Update Stock",
                "Update Price",
                "Sales Report",
                "Low Stock",
                "Expiring Tomorrow",
                "Logout"
            };

            EventHandler[] btnEvents = {
                btnRefresh_Click,
                btnAdd_Click,
                btnDelete_Click,
                btnUpdStock_Click,
                btnUpdPrice_Click,
                btnSales_Click,
                btnLowStock_Click,
                btnExpiring_Click,
                btnLogout_Click
            };

            for (int i = 0; i < btnTexts.Length; i++)
            {
                var btn = new Button();
                btn.Text = btnTexts[i];
                btn.Location = new Point(10, 10 + i * 55);
                btn.Size = new Size(175, 45);
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.BackColor = btnTexts[i] == "Logout" ? Color.IndianRed : Color.SteelBlue;
                btn.Click += btnEvents[i];
                this.Controls.Add(btn);
            }

            LoadProducts();
        }
        private void LoadProducts()
        {
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
            lblStatus.Text = "All Products";
            dataGrid.Columns.Add("Name", "Product Name");
            dataGrid.Columns.Add("Stock", "Stock");
            dataGrid.Columns.Add("Price", "Price");
            dataGrid.Columns.Add("Exp", "Expiry");
            foreach (var p in _productService.GetAll())
                dataGrid.Rows.Add(p.Name, p.Stock, p.Price,
                    p.ExpDay + "/" + p.ExpMonth + "/" + p.ExpYear);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("Product name:", "Add Product");
            if (string.IsNullOrWhiteSpace(name)) return;

            string sStock = Microsoft.VisualBasic.Interaction.InputBox("Stock:", "Add Product");
            string sPrice = Microsoft.VisualBasic.Interaction.InputBox("Price:", "Add Product");
            string sDay = Microsoft.VisualBasic.Interaction.InputBox("Expiry Day (1-31):", "Add Product");
            string sMon = Microsoft.VisualBasic.Interaction.InputBox("Expiry Month (1-12):", "Add Product");
            string sYear = Microsoft.VisualBasic.Interaction.InputBox("Expiry Year e.g. 2026:", "Add Product");

            if (!int.TryParse(sStock.Trim(), out int stock) ||
                !int.TryParse(sPrice.Trim(), out int price) ||
                !int.TryParse(sDay.Trim(), out int day) ||
                !int.TryParse(sMon.Trim(), out int mon) ||
                !int.TryParse(sYear.Trim(), out int year))
            {
                MessageBox.Show("Please enter numbers only in all fields.");
                return;
            }

            var (ok, err) = _productService.AddProduct(name.Trim(), stock, price, day, mon, year);
            MessageBox.Show(ok ? "Product added!" : err);
            LoadProducts();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentRow == null)
            {
                MessageBox.Show("Select a product first.");
                return;
            }
            if (MessageBox.Show("Delete this product?", "Confirm",
                MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            var (ok, err) = _productService.DeleteProduct(dataGrid.CurrentRow.Index);
            MessageBox.Show(ok ? "Deleted!" : err);
            LoadProducts();
        }

        private void btnUpdStock_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentRow == null)
            {
                MessageBox.Show("Select a product first.");
                return;
            }
            string input = Microsoft.VisualBasic.Interaction.InputBox("Quantity to add:", "Update Stock");
            if (!int.TryParse(input, out int qty)) return;
            var (ok, err) = _productService.UpdateStock(dataGrid.CurrentRow.Index, qty);
            MessageBox.Show(ok ? "Stock updated!" : err);
            LoadProducts();
        }

        private void btnUpdPrice_Click(object sender, EventArgs e)
        {
            if (dataGrid.CurrentRow == null)
            {
                MessageBox.Show("Select a product first.");
                return;
            }
            string input = Microsoft.VisualBasic.Interaction.InputBox("New price:", "Update Price");
            if (!int.TryParse(input, out int price)) return;
            var (ok, err) = _productService.UpdatePrice(dataGrid.CurrentRow.Index, price);
            MessageBox.Show(ok ? "Price updated!" : err);
            LoadProducts();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
            lblStatus.Text = "Sales Report  |  Revenue: " + _salesService.TotalRevenue();
            dataGrid.Columns.Add("Product", "Product");
            dataGrid.Columns.Add("Qty", "Quantity");
            dataGrid.Columns.Add("Total", "Total Price");
            foreach (var s in _salesService.GetAll())
                dataGrid.Rows.Add(s.ProductName, s.Quantity, s.TotalPrice);
        }

        private void btnLowStock_Click(object sender, EventArgs e)
        {
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
            lblStatus.Text = "Low Stock (under 5 units)";
            dataGrid.Columns.Add("Name", "Product");
            dataGrid.Columns.Add("Stock", "Stock");
            foreach (var p in _productService.GetLowStock())
                dataGrid.Rows.Add(p.Name, p.Stock);
        }

        private void btnExpiring_Click(object sender, EventArgs e)
        {
            dataGrid.DataSource = null;
            dataGrid.Rows.Clear();
            dataGrid.Columns.Clear();
            lblStatus.Text = "Expiring Tomorrow";
            dataGrid.Columns.Add("Name", "Product");
            dataGrid.Columns.Add("Stock", "Stock");
            foreach (var p in _productService.GetExpiringTomorrow())
                dataGrid.Rows.Add(p.Name, p.Stock);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    

        private void AdminPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
