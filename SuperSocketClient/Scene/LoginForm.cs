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
            // 로그인 성공. 채팅 폼 생성 및 전환
            if(__chatForm == null)
            {
                __chatForm = new ChatForm();
                __chatForm.Tag = this;
            }

            __chatForm.Show();

            // 로그인 폼 감추기
            Hide();
        }
    }
}