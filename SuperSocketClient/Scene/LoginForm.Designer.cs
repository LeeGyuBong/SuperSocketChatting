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
            InputIdTextBox = new TextBox();
            LoginTextLabel = new Label();
            SuspendLayout();
            // 
            // LoginReq
            // 
            LoginReq.Location = new Point(60, 84);
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
            // InputIdTextBox
            // 
            InputIdTextBox.Location = new Point(114, 53);
            InputIdTextBox.MaxLength = 8;
            InputIdTextBox.Name = "InputIdTextBox";
            InputIdTextBox.Size = new Size(100, 23);
            InputIdTextBox.TabIndex = 2;
            // 
            // LoginTextLabel
            // 
            LoginTextLabel.AutoSize = true;
            LoginTextLabel.Location = new Point(65, 56);
            LoginTextLabel.Name = "LoginTextLabel";
            LoginTextLabel.Size = new Size(43, 15);
            LoginTextLabel.TabIndex = 3;
            LoginTextLabel.Text = "아이디";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(269, 161);
            ControlBox = false;
            Controls.Add(LoginTextLabel);
            Controls.Add(InputIdTextBox);
            Controls.Add(SocketTypeComboBox);
            Controls.Add(LoginReq);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += LoginForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LoginReq;
        private ComboBox SocketTypeComboBox;
        private TextBox InputIdTextBox;
        private Label LoginTextLabel;
    }
}