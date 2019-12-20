using System;

namespace UestraTV.Data
{
    public class Broadcast
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public Uri Url { get; set; }

        public Uri Thumbnail { get; set; }
    }
}
