using SuperSocketClient.Manager;
using SuperSocketClient.Object;
using System.Windows.Forms;

namespace SuperSocketClient.Scene
{
    public struct BroadcastChatBoxData
    {
        public string Sender;
        public string Message;
    }

    public partial class ChatForm : Form
    {
        public EventHandler LogoutEventHandler;
        public EventHandler ShowEventHandler;
        public EventHandler<BroadcastChatBoxData> ChatBoxWriteEventHandler;

        public ChatForm()
        {
            InitializeComponent();

            LogoutEventHandler = new EventHandler(LogoutEvent);
            ShowEventHandler = new EventHandler(ShowEvent);
            ChatBoxWriteEventHandler = new EventHandler<BroadcastChatBoxData>(ChatBoxWriteEvent);
        }

        private void LogoutReq_Click(object sender, EventArgs e)
        {
            Close();
            LogoutEventHandler.Invoke(this, EventArgs.Empty);
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
                    client.SendChatReq(ChatInputTextBox.Text);
                }
                else
                {
                    Close();
                    LogoutEventHandler.Invoke(this, EventArgs.Empty);
                }

                ChatInputTextBox.Clear();
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogoutEventHandler.Invoke(this, EventArgs.Empty);
        }

        private void LogoutEvent(object? sender, EventArgs e)
        {
            LoginForm? loginForm = FormManager.Instance.GetForm(FormType.Login) as LoginForm;
            if (loginForm != null)
            {
                loginForm?.ShowEventHandler.Invoke(this, new EventArgs());
            }
        }

        private void ShowEvent(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    Show();
                }));
            }
            else
            {
                Show();
            }

            Application.Run(); // TODO : 좋지 못한 해결법. 개선해야함
        }

        private void ChatBoxWriteEvent(object? sender, BroadcastChatBoxData e)
        {
            DateTime now = DateTime.Now;
            string value = string.Empty;
            if (e.Sender != "")
            {
                value = $"[{now.ToString("HH:mm:ss")}] {e.Sender} : {e.Message}\r\n";
            }
            else
            {
                value = $"[{now.ToString("HH:mm:ss")}] {e.Message}\r\n";
            }

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {

                    ChatBoradTextBox.AppendText(value);
                }));
            }
            else
            {
                ChatBoradTextBox.AppendText(value);
            }
        }

        private void ChatForm_Shown(object sender, EventArgs e)
        {
            FormManager.Instance.Client?.SendLoadCompleted();
        }
    }
}
