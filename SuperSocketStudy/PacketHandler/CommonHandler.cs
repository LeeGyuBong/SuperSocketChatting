using Newtonsoft.Json;
using SuperSocketStudy.Network;
using SuperSocketStudy.Packet;
using System.Text;

namespace SuperSocketStudy.PacketHandler
{
    public enum PacketID : int
    {
        PacketID_DummyChatReq = 1,
        PacketID_LoginReq = 11,
    }

    public class CommonHandler
    {
        public void RequestDummyChat(MySession session, MyBinaryRequestInfo reqInfo)
        {
            string jsonString = Encoding.GetEncoding("UTF-8").GetString(reqInfo.Body);
            if (jsonString.Length <= 0)
                return;

            var packet = JsonConvert.DeserializeObject<PK_CHAT>(jsonString);
            session.Send(packet.Sender + " : " + packet.Message);
        }

        public void RequestLogin(MySession session, MyBinaryRequestInfo reqInfo)
        {

        }
    }
}
