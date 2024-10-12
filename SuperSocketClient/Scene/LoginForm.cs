using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public partial class LoginForm : Form
    {
        private ChatForm? __chatForm = null;
        private Player? __player = null;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginReq_Click(object sender, EventArgs e)
        {
            if (__player == null)
            {
                // �÷��̾� ����
                __player = new Player("Tester");
            }
            
            // ���� ���� ����
            if (__player.Login("127.0.0.1", 11021) == false)
            {
                return; // ���� ����
            }

            // TODO : �α��� ��Ŷ ����

            // ä�� �� ���� �� ��ȯ
            if (__chatForm == null)
            {
                __chatForm = new ChatForm(__player);
                __chatForm.Tag = this;
            }

            __chatForm.Show();

            // �α��� �� ���߱�
            Hide();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            __player?.Logout();
        }
    }
}