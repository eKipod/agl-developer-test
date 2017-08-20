using System;
using System.Collections.Generic;
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
            var catList = CatListHelper.GetCatsByOwnerGender(sourceContent);
            RenderCatList(catList, Console.Out);
        }

        private static void RenderCatList(IEnumerable<(string gender, IEnumerable<string> catNames)> catList, TextWriter @out)
        {
            throw new NotImplementedException();
        }
    }
}
