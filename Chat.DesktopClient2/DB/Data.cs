using System;

namespace Chat.DesktopClient2.DB
{
    public class Data
    {
        public Guid id { get; set; }

        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Nick { get; set; }
    }
}
