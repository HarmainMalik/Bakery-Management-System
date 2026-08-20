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
    public partial class CustomerLoginForm : Form
    {
        private readonly CustomerService _customerService;
        private readonly ProductService _productService;
        private readonly SalesService _salesService;
        private readonly AuthService _authService;
        private CheckBox chkNew;
        private Label lblId, lblPass, lblNewPass, lblMsg;
        private TextBox txtId, txtPassword, txtNewPass;
        private Button btnAction, btnCancel;
        public CustomerLoginForm(CustomerService custSvc, ProductService prodSvc,
             SalesService salesSvc, AuthService authSvc)
        {
            InitializeComponent();
            _customerService = custSvc;
            _productService = prodSvc;
            _salesService = salesSvc;
            _authService = authSvc;

            this.Text = "Customer Login";
            this.Size = new Size(500, 380);
            this.StartPosition = FormStartPosition.CenterScreen;

            // title
            var lblTitle = new Label();
            lblTitle.Text = "Customer Login";
            lblTitle.Font = new Font("Arial", 14, FontStyle.Bold);
            lblTitle.Location = new Point(150, 20);
            lblTitle.Size = new Size(250, 30);
            this.Controls.Add(lblTitle);

            // checkbox
            chkNew = new CheckBox();
            chkNew.Text = "New customer? Register here";
            chkNew.Location = new Point(130, 65);
            chkNew.Size = new Size(250, 25);
            chkNew.CheckedChanged += chkNew_CheckedChanged;
            this.Controls.Add(chkNew);

            // ID label + textbox
            lblId = new Label();
            lblId.Text = "Customer ID:";
            lblId.Location = new Point(100, 105);
            lblId.Size = new Size(110, 25);
            this.Controls.Add(lblId);

            txtId = new TextBox();
            txtId.Location = new Point(220, 102);
            txtId.Size = new Size(150, 26);
            this.Controls.Add(txtId);

            // Password label + textbox
            lblPass = new Label();
            lblPass.Text = "Password:";
            lblPass.Location = new Point(100, 145);
            lblPass.Size = new Size(110, 25);
            this.Controls.Add(lblPass);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(220, 142);
            txtPassword.Size = new Size(150, 26);
            txtPassword.PasswordChar = '*';
            this.Controls.Add(txtPassword);

            // New password label + textbox (hidden by default)
            lblNewPass = new Label();
            lblNewPass.Text = "Set Password:";
            lblNewPass.Location = new Point(100, 105);
            lblNewPass.Size = new Size(110, 25);
            lblNewPass.Visible = false;
            this.Controls.Add(lblNewPass);

            txtNewPass = new TextBox();
            txtNewPass.Location = new Point(220, 102);
            txtNewPass.Size = new Size(150, 26);
            txtNewPass.PasswordChar = '*';
            txtNewPass.Visible = false;
            this.Controls.Add(txtNewPass);

            // message label
            lblMsg = new Label();
            lblMsg.Text = "";
            lblMsg.Location = new Point(80, 190);
            lblMsg.Size = new Size(350, 40);
            lblMsg.ForeColor = Color.Red;
            this.Controls.Add(lblMsg);

            // Login/Register button
            btnAction = new Button();
            btnAction.Text = "Login";
            btnAction.Location = new Point(130, 240);
            btnAction.Size = new Size(100, 35);
            btnAction.BackColor = Color.SteelBlue;
            btnAction.ForeColor = Color.White;
            btnAction.FlatStyle = FlatStyle.Flat;
            btnAction.Click += btnAction_Click;
            this.Controls.Add(btnAction);

            // Cancel button
            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(260, 240);
            btnCancel.Size = new Size(100, 35);
            btnCancel.BackColor = Color.IndianRed;
            btnCancel.ForeColor = Color.White;
            btnCancel.FlatStyle = FlatStyle.Flat;
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
                var (success, customer, error) = _customerService.Register(pass);
                if (success)
                {
                    lblMsg.ForeColor = Color.Green;
                    lblMsg.Text = "Registered! Your ID: " + customer.Id;
                    var f = new CustomerPanel(customer, _productService, _salesService);
                    f.ShowDialog();
                    this.Close();
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
                var customer = _authService.ValidateCustomer(id, pass, _customerService.GetAll());
                if (customer != null)
                {
                    var f = new CustomerPanel(customer, _productService, _salesService);
                    f.ShowDialog();
                    this.Close();
                }
                else
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Wrong ID or password.";
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
        

       
