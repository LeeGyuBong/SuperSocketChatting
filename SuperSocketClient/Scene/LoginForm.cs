using SuperSocketClient.Manager;
using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // TODO : ������ Net.Socket, ���ۼ��� �����ؼ� �����ϵ��� ����
#if LOCAL_SOCKET
            NetworkTypeLabel.Text = "Net.Socket";
#else
            NetworkTypeLabel.Text = "SuperSocket";
#endif
        }

        private void LoginReq_Click(object sender, EventArgs e)
        {
            Client? client = FormManager.Instance.Client;
            if(client == null)
            {
                return;
            }

            if (client.IsInit == false)
            {
                // �÷��̾� �ʱ�ȭ
                client.Init("Tester");
            }

            // ���� ���� ����
            if (client.Login() == false)
            {
                return; // ���� ����
            }

            // TODO : �α��� ��Ŷ ����

            // ä�� �� ���� �� ��ȯ
            ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
            if (chatForm == null)
            {
                return;
            }

            chatForm.Show();

            // �α��� �� ���߱�
            Hide();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //__client?.Logout();
        }
    }
}