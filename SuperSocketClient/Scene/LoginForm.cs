using SuperSocketClient.Manager;
using SuperSocketClient.Network;
using SuperSocketClient.Object;
using System.Windows.Forms;

namespace SuperSocketClient.Scene
{
    public partial class LoginForm : Form
    {
        public EventHandler ChangeFormEventHandler;
        public EventHandler ShowEventHandler;

        public LoginForm()
        {
            InitializeComponent();

            ChangeFormEventHandler = new EventHandler(ChangeFormEvent);
            ShowEventHandler = new EventHandler(ShowEvent);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            foreach (SocketSessionType type in Enum.GetValues(typeof(SocketSessionType)))
            {
                SocketTypeComboBox.Items.Add(type.ToString());
            }
            SocketTypeComboBox.SelectedIndex = 0;
        }

        private void LoginReq_Click(object sender, EventArgs e)
        {
            Client? client = FormManager.Instance.Client;
            if (client == null)
            {
                return;
            }

            if (client.IsInit == false)
            {
                // 플레이어 초기화
                client.Init("Tester");
            }

            if (client.SessionConnect() == false)
            {
                client.Logout();
                return;
            }

            client.SendLoginReq();
        }

        private void ChangeFormEvent(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(delegate ()
                {
                    Hide();
                }));
            }
            else
            {
                Hide();
            }

            // 채팅 폼 생성 및 전환
            ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
            chatForm?.ShowEventHandler?.Invoke(this, EventArgs.Empty);
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

            // 채팅폼을 폼 매니저에서 삭제
            FormManager.Instance.RemoveForm(FormType.Chat);

            FormManager.Instance.Client?.Logout();
        }

        private void SocketTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SocketSessionType selectType = (SocketSessionType)SocketTypeComboBox.SelectedIndex;

            Client? client = FormManager.Instance.Client;
            if (client != null)
            {
                client.SocketSessionType = selectType;
            }
        }
    }
}