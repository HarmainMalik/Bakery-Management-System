using System;
using System.Drawing;
using System.Windows.Forms;
using SweetBakery.BusinessLogic;

namespace UI
{
    public partial class AdminLoginForm : Form
    {
        private readonly AdminService _adminService;
        private readonly ProductService _productService;
        private readonly SalesService _salesService;
        private readonly AuthService _authService;

        private CheckBox chkNew;
        private Label lblId, lblPass, lblNewPass, lblMsg;
        private TextBox txtId, txtPassword, txtNewPass;
        private Button btnAction;

        public AdminLoginForm(AdminService adminSvc, ProductService prodSvc,
            SalesService salesSvc, AuthService authSvc)
        {
            InitializeComponent();
            _adminService = adminSvc;
            _productService = prodSvc;
            _salesService = salesSvc;
            _authService = authSvc;

            this.Text = "Admin Login";
            this.Size = new Size(500, 420);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 245, 245);

            // title
            var lblTitle = new Label();
            lblTitle.Text = "Admin Login";
            lblTitle.Font = new Font("Georgia", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(90, 50, 20);
            lblTitle.Location = new Point(140, 20);
            lblTitle.Size = new Size(250, 40);
            this.Controls.Add(lblTitle);

            // divider
            var line = new Label();
            line.BorderStyle = BorderStyle.Fixed3D;
            line.Location = new Point(20, 65);
            line.Size = new Size(440, 2);
            this.Controls.Add(line);

            // checkbox
            chkNew = new CheckBox();
            chkNew.Text = "First time? Register here";
            chkNew.Font = new Font("Arial", 10);
            chkNew.Location = new Point(140, 80);
            chkNew.Size = new Size(230, 25);
            chkNew.CheckedChanged += chkNew_CheckedChanged;
            this.Controls.Add(chkNew);

            // ID label
            lblId = new Label();
            lblId.Text = "Admin ID:";
            lblId.Font = new Font("Arial", 10);
            lblId.Location = new Point(80, 125);
            lblId.Size = new Size(100, 25);
            this.Controls.Add(lblId);

            // ID textbox
            txtId = new TextBox();
            txtId.Font = new Font("Arial", 10);
            txtId.Location = new Point(210, 122);
            txtId.Size = new Size(180, 26);
            this.Controls.Add(txtId);

            // Password label
            lblPass = new Label();
            lblPass.Text = "Password:";
            lblPass.Font = new Font("Arial", 10);
            lblPass.Location = new Point(80, 168);
            lblPass.Size = new Size(100, 25);
            this.Controls.Add(lblPass);

            // Password textbox
            txtPassword = new TextBox();
            txtPassword.Font = new Font("Arial", 10);
            txtPassword.Location = new Point(210, 165);
            txtPassword.Size = new Size(180, 26);
            txtPassword.PasswordChar = '*';
            this.Controls.Add(txtPassword);

            // New password label (hidden)
            lblNewPass = new Label();
            lblNewPass.Text = "Set Password:";
            lblNewPass.Font = new Font("Arial", 10);
            lblNewPass.Location = new Point(80, 125);
            lblNewPass.Size = new Size(120, 25);
            lblNewPass.Visible = false;
            this.Controls.Add(lblNewPass);

            // New password textbox (hidden)
            txtNewPass = new TextBox();
            txtNewPass.Font = new Font("Arial", 10);
            txtNewPass.Location = new Point(210, 122);
            txtNewPass.Size = new Size(180, 26);
            txtNewPass.PasswordChar = '*';
            txtNewPass.Visible = false;
            this.Controls.Add(txtNewPass);

            // message label
            lblMsg = new Label();
            lblMsg.Text = "";
            lblMsg.Font = new Font("Arial", 9);
            lblMsg.ForeColor = Color.Red;
            lblMsg.Location = new Point(60, 210);
            lblMsg.Size = new Size(380, 40);
            this.Controls.Add(lblMsg);

            // Login/Register button
            btnAction = new Button();
            btnAction.Text = "Login";
            btnAction.Font = new Font("Arial", 10, FontStyle.Bold);
            btnAction.Location = new Point(100, 265);
            btnAction.Size = new Size(130, 42);
            btnAction.BackColor = Color.FromArgb(70, 130, 180);
            btnAction.ForeColor = Color.White;
            btnAction.FlatStyle = FlatStyle.Flat;
            btnAction.FlatAppearance.BorderSize = 0;
            btnAction.Cursor = Cursors.Hand;
            btnAction.Click += btnAction_Click;
            this.Controls.Add(btnAction);

            // Cancel button
            var btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font("Arial", 10, FontStyle.Bold);
            btnCancel.Location = new Point(255, 265);
            btnCancel.Size = new Size(130, 42);
            btnCancel.BackColor = Color.FromArgb(180, 70, 70);
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.Click += btnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        private void chkNew_CheckedChanged(object sender, EventArgs e)
        {
            bool isNew = chkNew.Checked;
            lblId.Visible = !isNew;
            txtId.Visible = !isNew;
            lblPass.Visible = !isNew;
            txtPassword.Visible = !isNew;
            lblNewPass.Visible = isNew;
            txtNewPass.Visible = isNew;
            btnAction.Text = isNew ? "Register" : "Login";
            lblMsg.Text = "";
        }

        private void btnAction_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (chkNew.Checked)
            {
                string pass = txtNewPass.Text.Trim();
                if (string.IsNullOrWhiteSpace(pass))
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Please type a password.";
                    return;
                }
                var (success, newId, error) = _adminService.RegisterRequest(pass);
                if (success)
                {
                    lblMsg.ForeColor = Color.Green;
                    lblMsg.Text = "Done! Your ID is: " + newId + ". Wait for owner approval.";
                }
                else
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = error;
                }
            }
            else
            {
                string id = txtId.Text.Trim();
                string pass = txtPassword.Text.Trim();
                if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(pass))
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Enter ID and password.";
                    return;
                }
                var admin = _authService.ValidateAdmin(id, pass, _adminService.GetAll());
                if (admin != null)
                {
                    var f = new AdminPanel(_productService, _salesService);
                    f.ShowDialog();
                    this.Close();
                }
                else
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Wrong ID/password or not approved yet.";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}