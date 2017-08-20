using System;
using System.IO;
using System.Linq;

namespace GetPeople
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceUrl = args.FirstOrDefault() ?? "http://agl-developer-test.azurewebsites.net/people.json";
            var sourceContent = HttpHelper.GetContentFromUrl(sourceUrl);
            var catList = GetCatsByOwnerGender(sourceContent);
            RenderCatList(catList, Console.Out);
        }

        private static IOrderedEnumerable<(string gender, IOrderedEnumerable<string> catNames)> GetCatsByOwnerGender(string sourceContent)
        {
            throw new NotImplementedException();
        }

        private static void RenderCatList(IOrderedEnumerable<(string gender, IOrderedEnumerable<string> catNames)> catList, TextWriter @out)
        {
            throw new NotImplementedException();
        }
    }
}
