namespace UI
{
    partial class CustomerPanel
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(900, 580);
            this.Name = "CustomerPanel";
            this.ResumeLayout(false);
        }
    }
    }