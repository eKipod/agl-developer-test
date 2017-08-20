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
            var catList = CatListHelper.GetCatsByOwnerGender(sourceContent);
            RenderingHelper.RenderList(catList, Console.Out);
        }

    }
}
