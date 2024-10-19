using SuperSocketClient.Manager;
using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public struct BroadcastChatBoxData
    {
        public string Sender;
        public string Message;
    }

    public partial class ChatForm : Form
    {
        private List<string> __loginUserNameList = new List<string>();

        public EventHandler LogoutEventHandler;
        public EventHandler ShowEventHandler;
        public EventHandler<BroadcastChatBoxData> ChatBoxWriteEventHandler;
        public EventHandler<List<string>> InitChatInfoEventHandler;
        public EventHandler<string> OtherClientLoginEventHandler;
        public EventHandler<string> OtherClientLogoutEventHandler;

        public ChatForm()
        {
            InitializeComponent();

            LogoutEventHandler = new EventHandler(LogoutEvent);
            ShowEventHandler = new EventHandler(ShowEvent);
            ChatBoxWriteEventHandler = new EventHandler<BroadcastChatBoxData>(ChatBoxWriteEvent);
            InitChatInfoEventHandler = new EventHandler<List<string>>(InitChatInfoEvent);
            OtherClientLoginEventHandler = new EventHandler<string>(OtherClientLoginEvent);
            OtherClientLogoutEventHandler = new EventHandler<string>(OtherClientLogoutEvent);
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
            if (string.IsNullOrEmpty(e.Sender) ||
                string.IsNullOrWhiteSpace(e.Sender))
            {
                return;
            }
                
            DateTime now = DateTime.Now;
            string value = $"[{now.ToString("HH:mm:ss")}] {e.Sender} : {e.Message}{Environment.NewLine}";

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    if (ChatBoradTextBox.Lines.Length > 200) // 텍스트가 일정량 쌓였으면 비워주기
                    {
                        int index = ChatBoradTextBox.Text.IndexOf(Environment.NewLine);
                        ChatBoradTextBox.Text = ChatBoradTextBox.Text.Remove(0, index + 2);
                    }

                    ChatBoradTextBox.AppendText(value);
                }));
            }
            else
            {
                if (ChatBoradTextBox.Lines.Length > 200) // 텍스트가 일정량 쌓였으면 비워주기
                {
                    int index = ChatBoradTextBox.Text.IndexOf(Environment.NewLine);
                    ChatBoradTextBox.Text = ChatBoradTextBox.Text.Remove(0, index + 2);
                }

                ChatBoradTextBox.AppendText(value);
            }
        }

        private void InitChatInfoEvent(object? sender, List<string> e)
        {
            if(e.Count <= 0)
            {
                return;
            }

            __loginUserNameList.Clear();
            __loginUserNameList = e.ToList();

            string userListString = string.Empty;
            foreach (string userName in __loginUserNameList)
            {
                userListString += $"{userName}{Environment.NewLine}";
            }

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    ClientList.Text = userListString;
                }));
            }
            else
            {
                ClientList.Text = userListString;
            }
        }

        private void OtherClientLoginEvent(object? sender, string e)
        {
            if (string.IsNullOrEmpty(e) ||
               string.IsNullOrWhiteSpace(e))
            {
                return;
            }

            if(__loginUserNameList.Contains(e) == false)
            {
                __loginUserNameList.Add(e);
            }

            string userListString = string.Empty;
            foreach (string userName in __loginUserNameList)
            {
                userListString += $"{userName}{Environment.NewLine}";
            }

            DateTime now = DateTime.Now;
            string noticeMessage = $"[{now.ToString("HH:mm:ss")}] {e}님이 로그인했습니다.{Environment.NewLine}";

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    ClientList.Text = userListString;

                    ChatBoradTextBox.AppendText(noticeMessage);
                }));
            }
            else
            {
                ClientList.Text = userListString;

                ChatBoradTextBox.AppendText(noticeMessage);
            }
        }

        private void OtherClientLogoutEvent(object? sender, string e)
        {
            if (string.IsNullOrEmpty(e) ||
               string.IsNullOrWhiteSpace(e))
            {
                return;
            }

            if (__loginUserNameList.Contains(e))
            {
                __loginUserNameList.Remove(e);
            }

            string userListString = string.Empty;
            foreach (string userName in __loginUserNameList)
            {
                userListString += $"{userName}{Environment.NewLine}";
            }

            DateTime now = DateTime.Now;
            string noticeMessage = $"[{now.ToString("HH:mm:ss")}] {e}님이 로그아웃했습니다.{Environment.NewLine}";

            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    ClientList.Text = userListString;

                    ChatBoradTextBox.AppendText(noticeMessage);
                }));
            }
            else
            {
                ClientList.Text = userListString;

                ChatBoradTextBox.AppendText(noticeMessage);
            }
        }
    }
}
