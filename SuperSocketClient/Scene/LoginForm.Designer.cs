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
            SocketTypeComboBox = new ComboBox();
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
            // SocketTypeComboBox
            // 
            SocketTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SocketTypeComboBox.FormattingEnabled = true;
            SocketTypeComboBox.Location = new Point(12, 12);
            SocketTypeComboBox.Name = "SocketTypeComboBox";
            SocketTypeComboBox.Size = new Size(121, 23);
            SocketTypeComboBox.TabIndex = 1;
            SocketTypeComboBox.SelectedIndexChanged += SocketTypeComboBox_SelectedIndexChanged;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(SocketTypeComboBox);
            Controls.Add(LoginReq);
            Name = "LoginForm";
            Text = "SuperSocketClient_Login";
            Load += LoginForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button LoginReq;
        private ComboBox SocketTypeComboBox;
    }
}