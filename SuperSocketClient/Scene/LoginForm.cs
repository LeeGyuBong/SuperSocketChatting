using SuperSocketClient.Manager;
using SuperSocketClient.Object;

namespace SuperSocketClient.Scene
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // TODO : 폼에서 Net.Socket, 슈퍼소켓 선택해서 접속하도록 변경
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
                // 플레이어 초기화
                client.Init("Tester");
            }

            // 소켓 연결 시작
            if (client.Login() == false)
            {
                return; // 연결 실패
            }

            // TODO : 로그인 패킷 전송

            // 채팅 폼 생성 및 전환
            ChatForm? chatForm = FormManager.Instance.GetForm(FormType.Chat) as ChatForm;
            if (chatForm == null)
            {
                return;
            }

            chatForm.Show();

            // 로그인 폼 감추기
            Hide();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //__client?.Logout();
        }
    }
}