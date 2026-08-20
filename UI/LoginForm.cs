using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DataAccess;
using SweetBakery.BusinessLogic;
using SweetBakery.DataAccess;

namespace UI
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;
        private readonly ProductService _productService;
        private readonly AdminService _adminService;
        private readonly CustomerService _customerService;
        private readonly SalesService _salesService;

        public LoginForm(AuthService auth, ProductService prod,
            AdminService admin, CustomerService cust, SalesService sales)
        {
            InitializeComponent();

            
            var productRepo = new ProductRepository();
            var salesRepo = new SalesRepository();
            var adminRepo = new AdminRepository();
            var custRepo = new CustomerRepository();

            _productService = new ProductService(productRepo);
            _salesService = new SalesService(salesRepo);
            _adminService = new AdminService(adminRepo);
            _customerService = new CustomerService(custRepo);
            _authService = new AuthService();

            this.Text = "Sweet Bakery";
            this.Size = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

          
            var lblTitle = new Label();
            lblTitle.Text = "Sweet Bakery";
            lblTitle.Font = new Font("Georgia", 28, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(90, 50, 20);
            lblTitle.Location = new Point(80, 40);
            lblTitle.Size = new Size(350, 55);
            this.Controls.Add(lblTitle);

          
            var lblSub = new Label();
            lblSub.Text = "Management System";
            lblSub.Font = new Font("Arial", 11, FontStyle.Italic);
            lblSub.ForeColor = Color.Gray;
            lblSub.Location = new Point(145, 98);
            lblSub.Size = new Size(220, 25);
            this.Controls.Add(lblSub);

            
            var line = new Label();
            line.BorderStyle = BorderStyle.Fixed3D;
            line.Location = new Point(50, 135);
            line.Size = new Size(390, 2);
            this.Controls.Add(line);

            
            var lblRole = new Label();
            lblRole.Text = "Select Your Role";
            lblRole.Font = new Font("Arial", 10, FontStyle.Regular);
            lblRole.ForeColor = Color.DimGray;
            lblRole.Location = new Point(175, 148);
            lblRole.Size = new Size(160, 22);
            this.Controls.Add(lblRole);

           
            string[] texts = { "Owner Login", "Admin Login", "Customer Login", "Exit" };
            EventHandler[] events = { btnOwner_Click, btnAdmin_Click, btnCustomer_Click, btnExit_Click };
            Color[] colors = {
        Color.FromArgb(70, 130, 180),
        Color.FromArgb(60, 120, 160),
        Color.FromArgb(50, 110, 150),
        Color.FromArgb(180, 70, 70)
    };

            for (int i = 0; i < texts.Length; i++)
            {
                var btn = new Button();
                btn.Text = texts[i];
                btn.Font = new Font("Arial", 11, FontStyle.Bold);
                btn.Location = new Point(110, 180 + i * 62);
                btn.Size = new Size(270, 48);
                btn.BackColor = colors[i];
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;
                btn.Click += events[i];
                this.Controls.Add(btn);
            }
        }

        private void btnOwner_Click(object sender, EventArgs e)
        {
            var f = new OwnerLoginForm(_adminService, _salesService,
                       _productService, _customerService, _authService);
            f.ShowDialog();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            var f = new AdminLoginForm(
                _adminService, _productService,
                _salesService, _authService);
            f.ShowDialog();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            var f = new CustomerLoginForm(
                _customerService, _productService,
                _salesService, _authService);
            f.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnOwner_Click_1(object sender, EventArgs e)
        {

        }

        private void btnAdmin_Click_1(object sender, EventArgs e)
        {
            var f = new AdminLoginForm(
                _adminService, _productService,
                _salesService, _authService);
            f.ShowDialog();
        }

        private void btnCustomer_Click_1(object sender, EventArgs e)
        {
            var f = new CustomerLoginForm(
                _customerService, _productService,
                _salesService, _authService);
            f.ShowDialog();
        }

        private void LoginForm_Load_1(object sender, EventArgs e)
        {

        }
    }
}