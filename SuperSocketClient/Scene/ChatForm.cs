using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public partial class ChatForm : Form
    {
        private LoginForm? __loginForm = null;
        private Player? __player = null;

        public ChatForm(Player player)
        {
            InitializeComponent();

            __player = player;

            //ChatBoradTextBox.Select(ChatBoradTextBox.Text.Length, 0);
            //ChatBoradTextBox.ScrollToCaret();
        }

        private void LogoutReq_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = (LoginForm)Tag;
            if (loginForm == null)
            {
                return;
            }

            // 플레이어 로그아웃
            if (__player != null)
            {
                __player.Logout();
            }

            // 로그인 폼 출력
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

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (__player != null)
            {
                __player.Logout();
            }
        }
    }
}
