﻿namespace Server.Model
{
    public class Receive_msg
    {
        public byte MsgId { get; set; }
        public User User { get; set; } = new();
        public Record Record { get; set; } = new();
    }
}