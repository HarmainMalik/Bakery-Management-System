using System;
using System.Drawing;
using System.Windows.Forms;
using SweetBakery.BusinessLogic;

namespace UI
{
    public partial class OwnerLoginForm : Form
    {
        private readonly AdminService _adminService;
        private readonly SalesService _salesService;
        private readonly ProductService _productService;
        private readonly CustomerService _customerService;
        private readonly AuthService _authService;

        private TextBox txtId;
        private TextBox txtPassword;
        private Label lblError;

        public OwnerLoginForm(AdminService adminSvc, SalesService salesSvc,
            ProductService prodSvc, CustomerService custSvc, AuthService authSvc)
        {
            InitializeComponent();
            _adminService = adminSvc;
            _salesService = salesSvc;
            _productService = prodSvc;
            _customerService = custSvc;
            _authService = authSvc;

            this.Text = "Owner Login";
            this.Size = new Size(450, 340);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // title
            var lblTitle = new Label();
            lblTitle.Text = "Owner Login";
            lblTitle.Font = new Font("Georgia", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(90, 50, 20);
            lblTitle.Location = new Point(120, 20);
            lblTitle.Size = new Size(250, 40);
            this.Controls.Add(lblTitle);

            // divider
            var line = new Label();
            line.BorderStyle = BorderStyle.Fixed3D;
            line.Location = new Point(20, 65);
            line.Size = new Size(390, 2);
            this.Controls.Add(line);

            // ID label
            var lblId = new Label();
            lblId.Text = "Owner ID:";
            lblId.Font = new Font("Arial", 10);
            lblId.Location = new Point(50, 85);
            lblId.Size = new Size(100, 25);
            this.Controls.Add(lblId);

            // ID textbox
            txtId = new TextBox();
            txtId.Font = new Font("Arial", 10);
            txtId.Location = new Point(180, 82);
            txtId.Size = new Size(180, 26);
            this.Controls.Add(txtId);

            // Password label
            var lblPass = new Label();
            lblPass.Text = "Password:";
            lblPass.Font = new Font("Arial", 10);
            lblPass.Location = new Point(50, 130);
            lblPass.Size = new Size(100, 25);
            this.Controls.Add(lblPass);

            // Password textbox
            txtPassword = new TextBox();
            txtPassword.Font = new Font("Arial", 10);
            txtPassword.Location = new Point(180, 127);
            txtPassword.Size = new Size(180, 26);
            txtPassword.PasswordChar = '*';
            this.Controls.Add(txtPassword);

            // error label
            lblError = new Label();
            lblError.Text = "";
            lblError.ForeColor = Color.Red;
            lblError.Font = new Font("Arial", 9);
            lblError.Location = new Point(50, 170);
            lblError.Size = new Size(350, 25);
            this.Controls.Add(lblError);

            // Login button
            var btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Font = new Font("Arial", 10, FontStyle.Bold);
            btnLogin.Location = new Point(80, 210);
            btnLogin.Size = new Size(130, 42);
            btnLogin.BackColor = Color.FromArgb(70, 130, 180);
            btnLogin.ForeColor = Color.White;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.Click += btnLogin_Click;
            this.Controls.Add(btnLogin);

            // Cancel button
            var btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Arial", 10, FontStyle.Bold);
            btnCancel.Location = new Point(230, 210);
            btnCancel.Size = new Size(130, 42);
            btnCancel.BackColor = Color.FromArgb(180, 70, 70);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += btnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (!int.TryParse(txtId.Text.Trim(), out int id))
            {
                lblError.Text = "ID must be a number.";
                return;
            }

            bool ok = _authService.ValidateOwner(id, txtPassword.Text.Trim());

            if (ok)
            {
                var f = new OwnerPanel(_adminService, _salesService,
                                       _productService, _customerService);
                f.ShowDialog();
                this.Close();
            }
            else
            {
                lblError.Text = "Wrong ID or password.";
                txtPassword.Clear();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OwnerLoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}