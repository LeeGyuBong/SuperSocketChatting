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
                // 플레이어 생성
                __player = new Player("Tester");
            }
            
            // 소켓 연결 시작
            if (__player.Login("127.0.0.1", 11021) == false)
            {
                return; // 연결 실패
            }

            // TODO : 로그인 패킷 전송

            // 채팅 폼 생성 및 전환
            if (__chatForm == null)
            {
                __chatForm = new ChatForm(__player);
                __chatForm.Tag = this;
            }

            __chatForm.Show();

            // 로그인 폼 감추기
            Hide();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            __player?.Logout();
        }
    }
}