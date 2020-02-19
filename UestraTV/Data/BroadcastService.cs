using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UestraTV.Data
{
    public class BroadcastService
    {
        private string fileEnding = ".jpg";
        private string town = "hannover";
        private string pattern = "/0/(?<id>(.*?)).jpg&";
        private string baseUrl = "https://www.publicbroadcast.de/programm.php?stadt={0}&tag={1}";
        private string thumbnail = "https://www.publicbroadcast.de/art/thumbnails/{0}/{1}/{2}{3}";
        private string fullsize = "https://www.publicbroadcast.de/art/fullsize_akt/{0}/{1}/{2}{3}";

        public async Task<List<Broadcast>> GetBroadcastAsync(ProgramWeekday day = ProgramWeekday.Today)
        {
            int dateOffset = (int)day;
            using var client = new HttpClient();

            var url = new Uri(string.Format(baseUrl, town, dateOffset));
            var result = await client.GetAsync(url);
            var content = await result.Content.ReadAsStringAsync();
            var matches = Regex.Matches(content, pattern, RegexOptions.IgnoreCase);
            var list = new List<Broadcast>();
            foreach (Match match in matches)
            {
                var id = match.Groups["id"].Value;
                var bd = new Broadcast
                {
                    Id = id,
                    Date = DateTime.Now.Date.AddDays(-dateOffset),
                    Url = new Uri(string.Format(fullsize, town, dateOffset, id, fileEnding)),
                    Thumbnail = new Uri(string.Format(thumbnail, town, dateOffset, id, fileEnding)),
                };
                list.Add(bd);
            }
            return list;
        }
    }
}
