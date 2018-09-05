using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

 public class AbsoluteStateMessage : MessageBase {

    public static short AbsoluteMessageType = MsgType.Highest + 1;

    public string message;
    public int frameId;

    public AbsoluteStateMessage(string message, int frameId)
    {
        this.message = message;
        this.frameId = frameId;
    }

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(frameId); 
        writer.Write(message);
        //base.Serialize(writer);
    }

    public override void Deserialize(NetworkReader reader)
    {
        frameId = reader.ReadInt32();
        message = reader.ReadString();
        base.Deserialize(reader);
    }
}
