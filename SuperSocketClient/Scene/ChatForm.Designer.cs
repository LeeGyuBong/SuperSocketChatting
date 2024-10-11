namespace SuperSocketClient.Scene
{
    partial class ChatForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LogoutReq = new Button();
            ChatBoradTextBox = new TextBox();
            ChatInputTextBox = new TextBox();
            SuspendLayout();
            // 
            // LogoutReq
            // 
            LogoutReq.Location = new Point(704, 12);
            LogoutReq.Name = "LogoutReq";
            LogoutReq.Size = new Size(84, 32);
            LogoutReq.TabIndex = 0;
            LogoutReq.Text = "Logout";
            LogoutReq.UseVisualStyleBackColor = true;
            LogoutReq.Click += LogoutReq_Click;
            // 
            // ChatBoradTextBox
            // 
            ChatBoradTextBox.Enabled = false;
            ChatBoradTextBox.Location = new Point(12, 12);
            ChatBoradTextBox.Multiline = true;
            ChatBoradTextBox.Name = "ChatBoradTextBox";
            ChatBoradTextBox.Size = new Size(373, 426);
            ChatBoradTextBox.TabIndex = 1;
            // 
            // ChatInputTextBox
            // 
            ChatInputTextBox.Location = new Point(530, 415);
            ChatInputTextBox.Name = "ChatInputTextBox";
            ChatInputTextBox.Size = new Size(258, 23);
            ChatInputTextBox.TabIndex = 2;
            ChatInputTextBox.KeyDown += ChatInputByPressKeyEnter;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ChatInputTextBox);
            Controls.Add(ChatBoradTextBox);
            Controls.Add(LogoutReq);
            Name = "ChatForm";
            Text = "SuperSocketClient_Chat";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LogoutReq;
        private TextBox ChatBoradTextBox;
        private TextBox ChatInputTextBox;
    }
}