using System.Collections.Generic;
using System.IO;

namespace GetPeople
{
    public class RenderingHelper
    {
        public static void RenderList(IEnumerable<NamedEnumerable<string>> list, TextWriter @out)
        {
            if (list == null) return;
            foreach (var group in list)
            {
                @out.WriteLine(group.Name);
                foreach(var element in group)
                {
                    @out.WriteLine($"\t{element}");
                }
            }
        }
    }
}
