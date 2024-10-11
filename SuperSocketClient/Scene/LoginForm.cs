namespace SuperSocketClient.Scene
{
    public partial class LoginForm : Form
    {
        ChatForm? __chatForm = null;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginReq_Click(object sender, EventArgs e)
        {
            // �α��� ����. ä�� �� ���� �� ��ȯ
            if(__chatForm == null)
            {
                __chatForm = new ChatForm();
                __chatForm.Tag = this;
            }

            __chatForm.Show();

            // �α��� �� ���߱�
            Hide();
        }
    }
}