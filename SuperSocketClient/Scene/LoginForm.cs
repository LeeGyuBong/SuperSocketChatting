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
        public EventHandler<string> ErrorLabelTextUpdateEventHandler;

        public LoginForm()
        {
            InitializeComponent();

            ChangeFormEventHandler = new EventHandler(ChangeFormEvent);
            ShowEventHandler = new EventHandler(ShowEvent);
            ErrorLabelTextUpdateEventHandler = new EventHandler<string>(ErrorLabelTextUpdateEvent);
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
            string inputUserID = InputIdTextBox.Text;
            if (string.IsNullOrEmpty(inputUserID) ||
                string.IsNullOrWhiteSpace(inputUserID))
            {
                ErrorNoticeLabel.Text = "���̵� �Է����ּ���.";
                return;
            }


            Client? client = FormManager.Instance.Client;
            if (client == null)
            {
                return;
            }

            if (client.IsInit == false)
            {
                // �÷��̾� �ʱ�ȭ
                client.Init(inputUserID);
            }

            if (client.SessionConnect() == false)
            {
                ErrorLabelTextUpdateEventHandler.Invoke(this, "���� ���� ����");
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

            // ä�� �� ���� �� ��ȯ
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

            // ä������ �� �Ŵ������� ����
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

        private async void ErrorNoticeLabel_TextChanged(object sender, EventArgs e)
        {
            if(ErrorNoticeLabel.Text.Length > 0)
            {
                await Task.Delay(2500);
                ErrorNoticeLabel.Text = string.Empty;
            }
        }

        private void ErrorLabelTextUpdateEvent(object? sender, string e)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    ErrorNoticeLabel.Text = e;
                }));
            }
            else
            {
                ErrorNoticeLabel.Text = e;
            }
        }
    }
}