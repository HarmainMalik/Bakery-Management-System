namespace UI
{
    partial class OwnerLoginForm
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
            // 
            // OwnerLoginForm
            // 
            this.ClientSize = new System.Drawing.Size(450, 320);
            this.Name = "OwnerLoginForm";
            this.Load += new System.EventHandler(this.OwnerLoginForm_Load);
            this.ResumeLayout(false);

        }
    }
}