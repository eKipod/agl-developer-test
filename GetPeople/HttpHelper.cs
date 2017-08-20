using System;
using System.Net.Http;

namespace GetPeople
{
    public class HttpHelper
    {
        public static string GetContentFromUrl(string sourceUrl)
        {
            using (var client = new HttpClient())
            {
                return client
                    .GetStringAsync(sourceUrl)
                    .Result;
            }
        }
    }
}
