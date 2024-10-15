using SuperSocketClient.Manager;
using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();

            //ChatBoradTextBox.Select(ChatBoradTextBox.Text.Length, 0);
            //ChatBoradTextBox.ScrollToCaret();
        }

        private void LogoutReq_Click(object sender, EventArgs e)
        {
            FormClose();
            Close();
        }

        private void ChatInputByPressKeyEnter(object sender, KeyEventArgs e)
        {
            e.Handled = true;

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                Client? client = FormManager.Instance.Client;
                if (client != null)
                {
                    client.SendChat(ChatInputTextBox.Text);
                }
                else
                {
                    FormClose();
                    Close();
                }

                ChatInputTextBox.Clear();
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClose();
        }

        public void BoradCastChatBox(string sender, string message)
        {
            DateTime now = DateTime.Now;

            if (ChatBoradTextBox.InvokeRequired)
            {
                ChatBoradTextBox.Invoke(new MethodInvoker(delegate ()
                {
                    ChatBoradTextBox.AppendText($"[{now.Hour}:{now.Minute}:{now.Second}] {sender} : {message}\r\n");
                }));
            }
            else
            {
                ChatBoradTextBox.AppendText($"[{now.Hour}:{now.Minute}:{now.Second}] {sender} : {message}\r\n");
            }
        }

        private void FormClose()
        {
            FormManager.Instance.Client?.Logout();

            // 채팅폼을 폼 매니저에서 삭제
            FormManager.Instance.RemoveForm(FormType.Chat);

            LoginForm? loginForm = FormManager.Instance.GetForm(FormType.Login) as LoginForm;
            loginForm?.Show();
        }
    }
}
