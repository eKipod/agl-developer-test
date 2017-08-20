using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GetPeople
{
    public class CatListHelper
    {
        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        private class Pet
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local (used by JsonConvert)
            public string Name { get; set; }

            // ReSharper disable once UnusedAutoPropertyAccessor.Local (used by JsonConvert)
            public string Type { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        private class Owner
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local (used by JsonConvert)
            public string Gender { get; set; }
            // ReSharper disable once UnusedAutoPropertyAccessor.Local (used by JsonConvert)
            public IEnumerable<Pet> Pets { get; set; }
        }

        public static IEnumerable<NamedEnumerable<string>> GetCatsByOwnerGender(string sourceContent)
        {
            if (string.IsNullOrWhiteSpace(sourceContent))
                return Enumerable.Empty<NamedEnumerable<string>>();

            var owners = JsonConvert.DeserializeObject<IEnumerable<Owner>>(sourceContent);

            return owners
                .SelectMany(owner => (owner.Pets ?? Enumerable.Empty<Pet>())
                    .Where(pet => pet.Type == "Cat")
                    .Select(pet => (ownerGender: owner.Gender, catName: pet.Name)))
                .GroupBy(pair => pair.ownerGender, pair => pair.catName)
                .Select(group => new NamedEnumerable<string>
                {
                    Name = group.Key,
                    Items = group.OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                });
        }
    }
}
