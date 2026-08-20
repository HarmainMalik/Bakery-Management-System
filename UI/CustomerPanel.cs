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
using SweetBakery.Models;

namespace UI
{
    public partial class CustomerPanel : Form
    {
        private readonly Customer _customer;
        private readonly ProductService _productService;
        private readonly SalesService _salesService;
        private readonly CartService _cartService;
        private DataGridView dgvProducts;
        private Label lblWelcome, lblCartInfo, lblMsg;
        private TextBox txtQuantity;

        public CustomerPanel(Customer customer, ProductService prodSvc, SalesService salesSvc)
        {
            InitializeComponent();
            _customer = customer;
            _productService = prodSvc;
            _salesService = salesSvc;
            _cartService = new CartService();
            this.Text = "Customer Panel";
            this.Size = new Size(900, 640);
            this.StartPosition = FormStartPosition.CenterScreen;

           
            lblWelcome = new Label();
            lblWelcome.Text = "Welcome, " + _customer.Id;
            lblWelcome.Font = new Font("Arial", 12, FontStyle.Bold);
            lblWelcome.Location = new Point(10, 10);
            lblWelcome.Size = new Size(300, 30);
            this.Controls.Add(lblWelcome);

           
            lblCartInfo = new Label();
            lblCartInfo.Text = "Cart: 0 items  |  Total: 0";
            lblCartInfo.Location = new Point(10, 45);
            lblCartInfo.Size = new Size(400, 25);
            this.Controls.Add(lblCartInfo);

           
            lblMsg = new Label();
            lblMsg.Text = "";
            lblMsg.Location = new Point(10, 72);
            lblMsg.Size = new Size(500, 25);
            lblMsg.ForeColor = Color.Red;
            this.Controls.Add(lblMsg);

            
            dgvProducts = new DataGridView();
            dgvProducts.Location = new Point(10, 105);
            dgvProducts.Size = new Size(860, 430);
            dgvProducts.ReadOnly = true;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.BackgroundColor = Color.White;
            this.Controls.Add(dgvProducts);

            
            var lblQty = new Label();
            lblQty.Text = "Qty:";
            lblQty.Location = new Point(10, 548);
            lblQty.Size = new Size(35, 25);
            this.Controls.Add(lblQty);

          
            txtQuantity = new TextBox();
            txtQuantity.Text = "1";
            txtQuantity.Location = new Point(48, 545);
            txtQuantity.Size = new Size(50, 26);
            this.Controls.Add(txtQuantity);

           
            var btnAdd = new Button();
            btnAdd.Text = "Add to Cart";
            btnAdd.Location = new Point(110, 541);
            btnAdd.Size = new Size(110, 35);
            btnAdd.BackColor = Color.SteelBlue;
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Click += btnAddToCart_Click;
            this.Controls.Add(btnAdd);

            
            var btnCart = new Button();
            btnCart.Text = "View Cart / Checkout";
            btnCart.Location = new Point(235, 541);
            btnCart.Size = new Size(175, 35);
            btnCart.BackColor = Color.SeaGreen;
            btnCart.ForeColor = Color.White;
            btnCart.FlatStyle = FlatStyle.Flat;
            btnCart.Click += btnViewCart_Click;
            this.Controls.Add(btnCart);

           
            var btnOut = new Button();
            btnOut.Text = "Logout";
            btnOut.Location = new Point(425, 541);
            btnOut.Size = new Size(90, 35);
            btnOut.BackColor = Color.IndianRed;
            btnOut.ForeColor = Color.White;
            btnOut.FlatStyle = FlatStyle.Flat;
            btnOut.Click += btnLogout_Click;
            this.Controls.Add(btnOut);

            LoadProducts();
        }
        private void LoadProducts()
        {
            dgvProducts.Rows.Clear();
            dgvProducts.Columns.Clear();
            dgvProducts.Columns.Add("No", "#");
            dgvProducts.Columns.Add("Name", "Product");
            dgvProducts.Columns.Add("Price", "Price");
            dgvProducts.Columns.Add("Stock", "In Stock");
            dgvProducts.Columns.Add("Exp", "Expiry");
            var products = _productService.GetAll();
            for (int i = 0; i < products.Count; i++)
            {
                var p = products[i];
                dgvProducts.Rows.Add(i + 1, p.Name, p.Price, p.Stock,
                    p.ExpDay + "/" + p.ExpMonth + "/" + p.ExpYear);
            }
        }

        private void RefreshCartLabel()
        {
            lblCartInfo.Text = "Cart: " + _cartService.ItemCount +
                               " item(s)  |  Total: " + _cartService.TotalPrice;
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (dgvProducts.CurrentRow == null)
            {
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Select a product first.";
                return;
            }
            if (!int.TryParse(txtQuantity.Text.Trim(), out int qty) || qty <= 0)
            {
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = "Enter a valid quantity.";
                return;
            }
            int idx = dgvProducts.CurrentRow.Index;
            var allProducts = _productService.GetAll().ToList();
            if (idx >= allProducts.Count) return;
            var product = allProducts[idx];

            var (ok, err) = _cartService.AddItem(product, qty);
            if (ok)
            {
                lblMsg.ForeColor = Color.Green;
                lblMsg.Text = "Added to cart!";
                RefreshCartLabel();
            }
            else
            {
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = err;
            }
        }

        private void btnViewCart_Click(object sender, EventArgs e)
        {
            var f = new CartForm(_cartService, _productService, _salesService);
            f.ShowDialog();
            LoadProducts();
            RefreshCartLabel();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    

        private void CustomerPanel_Load(object sender, EventArgs e)
        {

        }
    }
}
