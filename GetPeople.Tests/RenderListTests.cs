using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace GetPeople.Tests
{
    public class RenderListTests
    {

        [Test, TestCaseSource(typeof(TestCases), nameof(TestCases.Data))]
        public string RenderListTest(IEnumerable<NamedEnumerable<string>> input)
        {
            var stringBuilder = new StringBuilder();
            using (var stringWriter = new StringWriter(stringBuilder))
            {
                RenderingHelper.RenderList(input, stringWriter);
                return stringBuilder.ToString();
            }
        }

        public class TestCases
        {
            public static IEnumerable Data
            {
                get
                {
                    yield return new TestCaseData(null)
                        .Returns(string.Empty)
                        .SetName("No input");

                    yield return new TestCaseData(new[] { new NamedEnumerable<string>{ Name = "Header1" } }.AsEnumerable())
                        .Returns($"Header1{Environment.NewLine}")
                        .SetName("Header with no items");

                    yield return new TestCaseData(new[] { new NamedEnumerable<string> { Name = "Header1", Items = new[] { "Item1" } } }.AsEnumerable())
                        .Returns($"Header1{Environment.NewLine}\tItem1{Environment.NewLine}")
                        .SetName("Header with one item");

                    yield return new TestCaseData(new[] { new NamedEnumerable<string> { Name = "Header1", Items = new[] { "Item1", "Item2" } } }.AsEnumerable())
                        .Returns($"Header1{Environment.NewLine}\tItem1{Environment.NewLine}\tItem2{Environment.NewLine}")
                        .SetName("Header with two items");

                    yield return new TestCaseData(new[]
                        {
                            new NamedEnumerable<string> { Name = "Header1", Items = new[] { "Item1", "Item2" } },
                            new NamedEnumerable<string> { Name = "Header2", Items = new[] { "Item3", "Item4" } }
                        }.AsEnumerable())
                        .Returns($"Header1{Environment.NewLine}\tItem1{Environment.NewLine}\tItem2{Environment.NewLine}" +
                                 $"Header2{Environment.NewLine}\tItem3{Environment.NewLine}\tItem4{Environment.NewLine}")
                        .SetName("Two header with two items");

                    yield return new TestCaseData(new[]
                        {
                            new NamedEnumerable<string> { Name = "Header1" },
                            new NamedEnumerable<string> { Name = "Header2", Items = new[] { "Item3", "Item4" } }
                        }.AsEnumerable())
                        .Returns($"Header1{Environment.NewLine}" +
                                 $"Header2{Environment.NewLine}\tItem3{Environment.NewLine}\tItem4{Environment.NewLine}")
                        .SetName("Header1 has no items, and Header2 has two items");
                }
            }
        }
    }
}
