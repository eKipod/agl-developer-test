using System;
using System.Linq;

namespace GetPeople
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var sourceUrl = args.FirstOrDefault() ?? "http://agl-developer-test.azurewebsites.net/people.json";
                var sourceContent = HttpHelper.GetContentFromUrl(sourceUrl);
                var catList = CatListHelper.GetCatsByOwnerGender(sourceContent);
                RenderingHelper.RenderList(catList, Console.Out);
            }
            catch (Exception e)
            {
                var exceptions = (e as AggregateException)?.InnerExceptions ?? new [] {e}.AsEnumerable();
                Console.Error.WriteLine("Execution failed due to the following errors:");
                foreach (var ex in exceptions)
                {
                    Console.Error.WriteLine($"{ex.GetType().FullName}: {ex.Message}");
                }
            }
        }
    }
}
