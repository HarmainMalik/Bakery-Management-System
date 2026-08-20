using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SweetBakery.BusinessLogic;

namespace UI
{
    public partial class CartForm : Form
    {
        private readonly CartService _cartService;
        private readonly ProductService _productService;
        private readonly SalesService _salesService;

        private DataGridView dgvCart;
        private Label lblTotal;
        private Label lblBill;
        private Button btnRemove;
        private Button btnCheckout;

        public CartForm(CartService cartSvc, ProductService prodSvc, SalesService salesSvc)
        {
            InitializeComponent();
            _cartService = cartSvc;
            _productService = prodSvc;
            _salesService = salesSvc;

            this.Text = "Your Cart";
            this.Size = new Size(780, 580);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            var lblTitle = new Label();
            lblTitle.Text = "Your Cart";
            lblTitle.Font = new Font("Georgia", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(90, 50, 20);
            lblTitle.Location = new Point(20, 15);
            lblTitle.Size = new Size(300, 40);
            this.Controls.Add(lblTitle);

            var line = new Label();
            line.BorderStyle = BorderStyle.Fixed3D;
            line.Location = new Point(20, 58);
            line.Size = new Size(720, 2);
            this.Controls.Add(line);

            dgvCart = new DataGridView();
            dgvCart.Location = new Point(20, 68);
            dgvCart.Size = new Size(720, 300);
            dgvCart.ReadOnly = true;
            dgvCart.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCart.AllowUserToAddRows = false;
            dgvCart.BackgroundColor = Color.White;
            dgvCart.BorderStyle = BorderStyle.None;
            dgvCart.RowHeadersVisible = false;
            dgvCart.Font = new Font("Arial", 10);
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCart.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 10, FontStyle.Bold);
            dgvCart.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(70, 130, 180);
            dgvCart.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvCart.EnableHeadersVisualStyles = false;
            dgvCart.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(235, 245, 255);
            this.Controls.Add(dgvCart);

            lblTotal = new Label();
            lblTotal.Text = "Total:  Rs. 0";
            lblTotal.Font = new Font("Arial", 13, FontStyle.Bold);
            lblTotal.ForeColor = Color.FromArgb(90, 50, 20);
            lblTotal.Location = new Point(20, 385);
            lblTotal.Size = new Size(300, 30);
            this.Controls.Add(lblTotal);

            lblBill = new Label();
            lblBill.Text = "";
            lblBill.Font = new Font("Courier New", 9);
            lblBill.ForeColor = Color.DarkGreen;
            lblBill.Location = new Point(350, 380);
            lblBill.Size = new Size(390, 100);
            this.Controls.Add(lblBill);

            btnRemove = new Button();
            btnRemove.Text = "Remove Selected";
            btnRemove.Font = new Font("Arial", 10, FontStyle.Bold);
            btnRemove.Location = new Point(20, 430);
            btnRemove.Size = new Size(160, 42);
            btnRemove.BackColor = Color.FromArgb(200, 80, 80);
            btnRemove.ForeColor = Color.White;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.Click += btnRemove_Click;
            this.Controls.Add(btnRemove);

            btnCheckout = new Button();
            btnCheckout.Text = "Checkout";
            btnCheckout.Font = new Font("Arial", 10, FontStyle.Bold);
            btnCheckout.Location = new Point(195, 430);
            btnCheckout.Size = new Size(160, 42);
            btnCheckout.BackColor = Color.FromArgb(46, 139, 87);
            btnCheckout.ForeColor = Color.White;
            btnCheckout.FlatStyle = FlatStyle.Flat;
            btnCheckout.FlatAppearance.BorderSize = 0;
            btnCheckout.Cursor = Cursors.Hand;
            btnCheckout.Click += btnCheckout_Click;
            this.Controls.Add(btnCheckout);

            var btnClose = new Button();
            btnClose.Text = "Back";
            btnClose.Font = new Font("Arial", 10, FontStyle.Bold);
            btnClose.Location = new Point(370, 430);
            btnClose.Size = new Size(160, 42);
            btnClose.BackColor = Color.FromArgb(100, 100, 100);
            btnClose.ForeColor = Color.White;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Click += btnClose_Click;
            this.Controls.Add(btnClose);

            LoadCart();
        }

        private void LoadCart()
        {
            dgvCart.Rows.Clear();
            dgvCart.Columns.Clear();
            dgvCart.Columns.Add("Product", "Product");
            dgvCart.Columns.Add("UnitPrice", "Unit Price");
            dgvCart.Columns.Add("Qty", "Quantity");
            dgvCart.Columns.Add("LineTotal", "Line Total");
            foreach (var item in _cartService.Items)
                dgvCart.Rows.Add(item.Product.Name, item.Product.Price,
                                 item.Quantity, item.LineTotal);
            lblTotal.Text = "Total:  Rs. " + _cartService.TotalPrice;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Remove clicked. Row: " + (dgvCart.CurrentRow == null ? "null" : dgvCart.CurrentRow.Index.ToString()));
            if (dgvCart.CurrentRow == null)
            {
                MessageBox.Show("Select an item to remove.");
                return;
            }
            var (ok, err) = _cartService.RemoveItem(dgvCart.CurrentRow.Index);
            if (!ok) { MessageBox.Show(err); return; }
            LoadCart();
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Checkout clicked. Items in cart: " + _cartService.ItemCount);
            if (_cartService.ItemCount == 0)
            {
                MessageBox.Show("Your cart is empty!");
                return;
            }
            var items = _cartService.Checkout();
            _salesService.RecordSales(items, _productService,
                                      _productService.GetAll().ToList());

            string bill = "====== RECEIPT ======\n";
            foreach (var item in items)
                bill += item.Product.Name + " x" + item.Quantity +
                        "  =  Rs. " + item.LineTotal + "\n";
            bill += "---------------------\n";
            bill += "TOTAL:  Rs. " + items.Sum(i => i.LineTotal) + "\n";
            bill += "Thank you!";

            lblBill.Text = bill;
            btnCheckout.Enabled = false;
            btnRemove.Enabled = false;
            LoadCart();

            MessageBox.Show(bill, "Checkout Complete",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}