using Bogus;
using RandomUsers.Models;
using System.Collections.Generic;

namespace RandomUsers.Data
{
    public class DataSeeder : IDataSeeder
    {
        // Dictionary mapping regions to their corresponding locales.
        private static readonly Dictionary<string, string> Locales = new()
        {
            { "USA", "en_US" },
            { "France", "fr" },
            { "Germany", "de" },
            { "Italy", "it" },
            { "Brazil", "pt_BR" },
            { "Korea", "ko" },
            { "China", "zh_CN" }
        };


        // Generates a list of users based on the specified region, count, seed, and error rate.
        public List<User> GenerateUsers(string region, int count, int seed, int errorsPerRecord)
        {
            

            var faker2 = new Faker<User>(locale : "ko")
                .RuleFor(u => u.Identifier, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.Name, f => f.Name.FullName())  // Generate a full name based on the locale.
                .RuleFor(u => u.Address, f => f.Address.FullAddress())  // Generate a full address.
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());  // Generate a phone number.

            var suers = faker2.Generate(100);
            foreach(var u in suers)
            {
                Console.WriteLine($" {u.Address}");
            } 
           
            // Determine the appropriate locale based on the region.
            var locale = Locales.ContainsKey(region) ? Locales[region] : "en_US"; // Default to "en_US"
            Randomizer.Seed = new Random(seed);

            // Create a Faker instance for generating user data based on the specified locale.
            var faker = new Faker<User>(locale:  locale)
                .RuleFor(u => u.Identifier, f => Guid.NewGuid().ToString())
                .RuleFor(u => u.Name, f => f.Name.FullName())  // Generate a full name based on the locale.
                .RuleFor(u => u.Address, f => f.Address.FullAddress())  // Generate a full address.
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber());  // Generate a phone number.

            // Generate the specified number of users.
            var users = faker.Generate(count);
            ApplyErrors(users, errorsPerRecord);
            return users;
        }

        // Applies random errors to the generated user data.
        private void ApplyErrors(List<User> users, int errorsPerRecord)
        {
            var random = new Random();
            foreach (var user in users)
            {
                var errorCount = (int)Math.Floor((double)errorsPerRecord);
                if (random.NextDouble() < (errorsPerRecord % 1))
                {
                    errorCount++;
                }
                for (int i = 0; i < errorCount; i++)
                {
                    ApplyRandomError(user, random);
                }
            }
        }

        // Applies a random error to a specific user field.
        private void ApplyRandomError(User user, Random random)
        {
            var fields = new[] { "Name", "Address", "PhoneNumber" };
            var field = fields[random.Next(fields.Length)];

            switch (field)
            {
                case "Name":
                    user.Name = ApplyError(user.Name, random);
                    break;
                case "Address":
                    user.Address = ApplyError(user.Address, random);
                    break;
                case "PhoneNumber":
                    user.PhoneNumber = ApplyError(user.PhoneNumber, random);
                    break;
            }
        }

        // Applies a random error to a string value.
        private string ApplyError(string value, Random random)
        {
            var errorTypes = new Func<string, Random, string>[]
            {
                DeleteRandomCharacter,
                AddRandomCharacter,
                SwapAdjacentCharacters
            };

            var error = errorTypes[random.Next(errorTypes.Length)];
            return error(value, random);
        }

        // Deletes a random character from a string.
        private string DeleteRandomCharacter(string value, Random random)
        {
            if (value.Length == 0) return value;
            var index = random.Next(value.Length);
            return value.Remove(index, 1);
        }

        // Adds a random character to a string.
        private string AddRandomCharacter(string value, Random random)
        {
            var index = random.Next(value.Length + 1);
            var character = (char)random.Next('a', 'z' + 1);
            return value.Insert(index, character.ToString());
        }

        // Swaps two adjacent characters in a string.
        private string SwapAdjacentCharacters(string value, Random random)
        {
            if (value.Length < 2) return value;
            var index = random.Next(value.Length - 1);
            var chars = value.ToCharArray();
            (chars[index], chars[index + 1]) = (chars[index + 1], chars[index]);
            return new string(chars);
        }
    }
}
