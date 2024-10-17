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
        private void ChatForm_Shown(object sender, EventArgs e)
        {
            FormManager.Instance.Client?.SendLoadCompleted();
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
            if (FormManager.Instance.GetForm(FormType.Login) is LoginForm loginForm)
            {
                loginForm.ShowEventHandler.Invoke(this, new EventArgs());
            }
        }

        private void ShowEvent(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    ShowDialog();
                }));
            }
            else
            {
                ShowDialog();
            }
        }

        private void ChatBoxWriteEvent(object? sender, BroadcastChatBoxData e)
        {
            DateTime now = DateTime.Now;
            string value = string.Empty;
            if (string.IsNullOrEmpty(e.Sender) == false &&
                string.IsNullOrWhiteSpace(e.Sender) == false)
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
    }
}
