using System;

namespace SuperSocketShared.Packet
{
    public enum PacketID : ushort
    {
        LoginReq = 1,
        LoginAck,

        LoadCompletedReq,
        ChatInfoAck,

        ChatReq,

        BroadcastLoginAck,
        BroadcastLogoutAck,
        BroadcastChatAck,
    }

    public enum ErrorEvent : Int64
    {
        None = 0,
        DuplicateLogin,
    }
}