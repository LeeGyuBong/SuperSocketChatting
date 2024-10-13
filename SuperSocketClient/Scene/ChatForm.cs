using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public partial class ChatForm : Form
    {
        private Client? __client = null;

        public ChatForm(Client client)
        {
            InitializeComponent();

            __client = client;

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
            if (__client != null)
            {
                __client.Logout();
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

                if (__client != null)
                {
                    __client.SendChat(ChatInputTextBox.Text);
                }
                else
                {
                    LogoutReq_Click(sender, e);
                }

                //DateTime now = DateTime.Now;
                //ChatBoradTextBox.AppendText($"[{now.Hour}:{now.Minute}:{now.Second}] {ChatInputTextBox.Text}\r\n");

                ChatInputTextBox.Clear();
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (__client != null)
            {
                __client.Logout();
            }
        }
    }
}
