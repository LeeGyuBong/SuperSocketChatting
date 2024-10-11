namespace SuperSocketClient.Scene
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();

            ChatBoradTextBox.Select(ChatBoradTextBox.Text.Length, 0);
            ChatBoradTextBox.ScrollToCaret();
        }

        private void LogoutReq_Click(object sender, EventArgs e)
        {
            var loginForm = (LoginForm)Tag;
            if (loginForm == null)
            {
                // ErrorMessage
                return;
            }

            // 로그아웃 성공. 로그인 폼 출력
            loginForm.Show();

            // 채팅 폼 감추기
            Hide();
        }

        private void ChatInputByPressKeyEnter(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                DateTime now = DateTime.Now;

                ChatBoradTextBox.AppendText($"[{now.Hour}:{now.Minute}:{now.Second}] {ChatInputTextBox.Text}\r\n");
                ChatInputTextBox.Clear();
            }
        }
    }
}
