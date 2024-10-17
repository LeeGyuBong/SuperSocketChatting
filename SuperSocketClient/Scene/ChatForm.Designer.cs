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
            ClientList = new TextBox();
            label1 = new Label();
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
            ChatBoradTextBox.Location = new Point(12, 12);
            ChatBoradTextBox.Multiline = true;
            ChatBoradTextBox.Name = "ChatBoradTextBox";
            ChatBoradTextBox.ReadOnly = true;
            ChatBoradTextBox.ScrollBars = ScrollBars.Vertical;
            ChatBoradTextBox.Size = new Size(373, 425);
            ChatBoradTextBox.TabIndex = 1;
            ChatBoradTextBox.TabStop = false;
            // 
            // ChatInputTextBox
            // 
            ChatInputTextBox.Location = new Point(12, 414);
            ChatInputTextBox.Name = "ChatInputTextBox";
            ChatInputTextBox.Size = new Size(373, 23);
            ChatInputTextBox.TabIndex = 2;
            ChatInputTextBox.KeyDown += ChatInputByPressKeyEnter;
            // 
            // ClientList
            // 
            ClientList.Location = new Point(391, 32);
            ClientList.Multiline = true;
            ClientList.Name = "ClientList";
            ClientList.ReadOnly = true;
            ClientList.Size = new Size(100, 382);
            ClientList.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(411, 15);
            label1.Name = "label1";
            label1.Size = new Size(59, 15);
            label1.TabIndex = 4;
            label1.Text = "유저 목록";
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(799, 451);
            Controls.Add(label1);
            Controls.Add(ClientList);
            Controls.Add(ChatInputTextBox);
            Controls.Add(ChatBoradTextBox);
            Controls.Add(LogoutReq);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ChatForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chat";
            FormClosing += ChatForm_FormClosing;
            Shown += ChatForm_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button LogoutReq;
        private TextBox ChatBoradTextBox;
        private TextBox ChatInputTextBox;
        private TextBox ClientList;
        private Label label1;
    }
}