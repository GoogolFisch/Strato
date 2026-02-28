using UnityEngine;

public enum PacketTypes
{
    PackNone = 0,
    KeyExchangePack = 0,
    PingPack,
    ChatPacket,
    BaseEntityPacket,
    MovingEntityPacket,

    SummonEntityPacket,

    PackUp,
}
