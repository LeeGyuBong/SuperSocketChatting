using Newtonsoft.Json;
using SuperSocketServer.Network.TCP;
using SuperSocketServer.Packet;
using System.Text;

namespace SuperSocketServer.PacketHandler
{
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
