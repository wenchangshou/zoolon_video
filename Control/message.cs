using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    internal class replyMessage
    {
        public Boolean reply;
        public message content;
    }
    internal class message
    {
        public static int MsgIDLength = 16;
        public byte[] id;
        public string body;
        public UInt64 timestamp;
        public UInt16 attempts;
        public string topic;
        public message(byte[] message)
        {
            Parser(message);
        }
        public void Parser(byte[] message)
        {
            UInt16 topicSize;
            timestamp = BinaryPrimitives.ReadUInt64BigEndian(message[0..8]);
            attempts = BinaryPrimitives.ReadUInt16BigEndian(message[8..10]);
            topicSize = BinaryPrimitives.ReadUInt16BigEndian(message[10..12]);
            topicSize = (ushort)(12 + topicSize);
            topic = System.Text.Encoding.ASCII.GetString(message[12..topicSize]);
            id = message[topicSize..(topicSize + MsgIDLength)];
            body = System.Text.Encoding.UTF8.GetString(message[(topicSize + MsgIDLength)..]);
        }
        public byte[] Encode()
        {
            byte[] _topic = Encoding.UTF8.GetBytes(topic);
            byte[] _msgBody = Encoding.UTF8.GetBytes(body);
            int sizeLen = 12 + _topic.Length + MsgIDLength + _msgBody.Length;
            byte[] buffer = new byte[sizeLen];
            var span = new Span<byte>(buffer);

            BinaryPrimitives.WriteUInt64BigEndian(span.Slice(0, 8), timestamp);
            BinaryPrimitives.WriteUInt16BigEndian(span.Slice(8, 10), attempts);
            BinaryPrimitives.WriteUInt16BigEndian(span.Slice(10, 12), (ushort)_topic.Length);
            _topic.CopyTo(span.Slice(12, 12 + _topic.Length));


            id.CopyTo(span.Slice(12 + _topic.Length));
            _msgBody.CopyTo(span.Slice(12 + _topic.Length + MsgIDLength));
            return buffer;
        }
    }
}
