using System;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;
using static GetPeople.CatListHelper;

namespace GetPeople.Tests
{
    public class GetCatsByOwnerGenderTests
    {
        [Test]
        public void WhenNullOrWhitespaceSource()
        {
            var result = GetCatsByOwnerGender(string.Empty);
            result.ShouldBeEmpty();
        }

        [Test]
        public void WhenEmptyList()
        {
            const string sourceContent = "[]";
            var result = GetCatsByOwnerGender(sourceContent);
            result.ShouldBeEmpty();
        }

        [Test]
        public void WhenInvalidJson()
        {
            const string sourceContent = "this is not JSON";
            Should.Throw<JsonReaderException>(() => GetCatsByOwnerGender(sourceContent));
        }

        [Test]
        public void WhenOwnerHasNoPets()
        {
            var sourceContent = new[] { new { gender = "Male" } };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent));
            result.ShouldBeEmpty();
        }

        [Test]
        public void WhenOwnerHasNoCats()
        {
            var sourceContent = new[] { new
            {
                gender = "Female",
                pets = new[] { new { name = "Nemo", type = "Fish" } }
            } };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent));
            result.ShouldBeEmpty();
        }

        [Test]
        public void WhenOwnerHasOneCat()
        {
            var sourceContent = new[] { new
            {
                gender = "Male",
                pets = new[] { new { name = "Felix", type = "Cat" } }
            } };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent)).ToList();
            result.ShouldHaveSingleItem();
            result[0].Name.ShouldBe("Male");
            result[0].Items.ShouldHaveSingleItem("Felix");
        }

        [Test]
        public void WhenOwnerHasTwoCats()
        {
            var sourceContent = new[] { new
            {
                gender = "Female",
                pets = new[]
                {
                    new { name = "grumpy", type = "Cat" },
                    new { name = "Felix", type = "Cat" }
                }
            } };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent)).ToList();
            result.ShouldHaveSingleItem();
            result[0].Name.ShouldBe("Female");
            result[0].Items.ShouldBeInOrder(SortDirection.Ascending, StringComparer.OrdinalIgnoreCase);
            result[0].Items.SequenceEqual(new[] { "Felix", "grumpy" }).ShouldBeTrue();
        }

        [Test]
        public void WhenOwnerHasOneCatAndOneNonCat()
        {
            var sourceContent = new[] { new
            {
                gender = "Other",
                pets = new[]
                {
                    new { name = "Dodo", type = "Dodo" },
                    new { name = "Felix", type = "Cat" }
                }
            } };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent)).ToList();
            result.ShouldHaveSingleItem();
            result[0].Name.ShouldBe("Other");
            result[0].Items.SequenceEqual(new[] { "Felix" }).ShouldBeTrue();
        }

        [Test]
        public void WhenTwoOwnerWithSameGenderAndCats()
        {
            var sourceContent = new[] {
                new
                {
                    gender = "Alien",
                    pets = new[] { new { name = "Grumpy", type = "Cat" } }
                },
                new
                {
                    gender = "Alien",
                    pets = new[] { new { name = "Felix", type = "Cat" } }
                }
            };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent)).ToList();
            result.ShouldHaveSingleItem();
            result[0].Name.ShouldBe("Alien");
            result[0].Items.ShouldBeInOrder();
            result[0].Items.SequenceEqual(new[] { "Felix", "Grumpy" }).ShouldBeTrue();
        }

        [Test]
        public void WhenTwoOwnerWithDifferentGenderAndCats()
        {
            var sourceContent = new[] {
                new
                {
                    gender = "Alien",
                    pets = new[]
                    {
                        new { name = "Grumpy", type = "Cat" },
                        new { name = "Felix", type = "Cat" }
                    }
                },
                new
                {
                    gender = "Terrestrial",
                    pets = new[]
                    {
                        new { name = "Felix", type = "Cat" },
                        new { name = "Nyan", type = "Cat" }
                    }
                }
            };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent)).ToList();
            result.Count.ShouldBe(2);

            var result0 = result.FirstOrDefault(r => r.Name == "Alien");
            result0.ShouldNotBeNull();
            result0.Items.ShouldBeInOrder();
            result0.Items.SequenceEqual(new[] { "Felix", "Grumpy" }).ShouldBeTrue();

            var result1 = result.FirstOrDefault(r => r.Name == "Terrestrial");
            result1.ShouldNotBeNull();
            result1.Items.ShouldBeInOrder();
            result1.Items.SequenceEqual(new[] { "Felix", "Nyan" }).ShouldBeTrue();
        }

        [Test]
        public void WhenTwoOwnerWithDifferentGenderAndOneHasCatAndTheOtherDoesnt()
        {
            var sourceContent = new[] {
                new
                {
                    gender = "Alien",
                    pets = new[]
                    {
                        new { name = "Dingo", type = "Dog" }
                    }
                },
                new
                {
                    gender = "Terrestrial",
                    pets = new[]
                    {
                        new { name = "Felix", type = "Cat" },
                        new { name = "Nyan", type = "Cat" }
                    }
                }
            };
            var result = GetCatsByOwnerGender(JsonConvert.SerializeObject(sourceContent)).ToList();
            result.ShouldHaveSingleItem();

            result[0].Name.ShouldBe("Terrestrial");
            result[0].Items.ShouldBeInOrder();
            result[0].Items.SequenceEqual(new[] { "Felix", "Nyan" }).ShouldBeTrue();
        }
    }
}
