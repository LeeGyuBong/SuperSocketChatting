namespace SuperSocketClient.Scene
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LoginReq = new Button();
            NetworkTypeLabel = new Label();
            SuspendLayout();
            // 
            // LoginReq
            // 
            LoginReq.Location = new Point(315, 259);
            LoginReq.Name = "LoginReq";
            LoginReq.Size = new Size(154, 65);
            LoginReq.TabIndex = 0;
            LoginReq.Text = "Login";
            LoginReq.UseVisualStyleBackColor = true;
            LoginReq.Click += LoginReq_Click;
            // 
            // NetworkTypeLabel
            // 
            NetworkTypeLabel.AutoSize = true;
            NetworkTypeLabel.Location = new Point(12, 9);
            NetworkTypeLabel.Name = "NetworkTypeLabel";
            NetworkTypeLabel.Size = new Size(0, 15);
            NetworkTypeLabel.TabIndex = 1;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(NetworkTypeLabel);
            Controls.Add(LoginReq);
            Name = "LoginForm";
            Text = "SuperSocketClient_Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoginReq;
        private Label NetworkTypeLabel;
    }
}