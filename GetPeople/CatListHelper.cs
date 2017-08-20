using System;
using System.CodeDom;
using System.Collections;
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
            public string Name { get; set; }
            public string Type { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        private class Owner
        {
            public string Gender { get; set; }
            public IEnumerable<Pet> Pets { get; set; }
        }

        public static IEnumerable<(string gender, IEnumerable<string> catNames)> GetCatsByOwnerGender(string sourceContent)
        {
            if (string.IsNullOrWhiteSpace(sourceContent))
                return Enumerable.Empty<(string gender, IEnumerable<string> catNames)>();

            var owners = JsonConvert.DeserializeObject<IEnumerable<Owner>>(sourceContent);

            return owners
                .SelectMany(owner => (owner.Pets ?? Enumerable.Empty<Pet>())
                    .Where(pet => pet.Type == "Cat") 
                    .Select(pet => (ownerGender: owner.Gender, catName: pet.Name)))
                .GroupBy(pair => pair.ownerGender, pair => pair.catName)
                .Select(group => (
                    gender: group.Key, 
                    catNames: group
                        .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                        .AsEnumerable()));
        }
    }
}
