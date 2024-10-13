using SuperSocketClient.Main;
using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public partial class LoginForm : Form
    {
        //private readonly SceneManager __sceneManager;

        private ChatForm? __chatForm = null;
        private Client? __client = null;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginReq_Click(object sender, EventArgs e)
        {
            if (__client == null)
            {
                // �÷��̾� ����
                __client = new Client("Tester");
            }
            
            // ���� ���� ����
            if (__client.Login() == false)
            {
                return; // ���� ����
            }

            // TODO : �α��� ��Ŷ ����

            // ä�� �� ���� �� ��ȯ
            if (__chatForm == null)
            {
                __chatForm = new ChatForm(__client);
                __chatForm.Tag = this;
            }

            __chatForm.Show();

            // �α��� �� ���߱�
            Hide();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            __client?.Logout();
        }
    }
}