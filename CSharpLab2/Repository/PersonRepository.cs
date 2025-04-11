using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using KMA.Krachylo.Lab2.Models;

namespace KMA.Krachylo.Lab2.Repository
{
    public class PersonRepository
    {
        private const string FilePath = "users.json";

        public async Task<List<Person>> LoadUsersAsync()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = await File.ReadAllTextAsync(FilePath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        return JsonSerializer.Deserialize<List<Person>>(json) ?? new List<Person>();
                    }
                }
                // Генеруємо 50 користувачів при першому запуску
                var users = await GenerateDefaultUsers();
                await SaveUsersAsync(users);
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading users: " + ex.Message);
            }
        }

        public async Task SaveUsersAsync(List<Person> users)
        {
            try
            {
                string json = JsonSerializer.Serialize(users);
                await File.WriteAllTextAsync(FilePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving users: " + ex.Message);
            }
        }

        private async Task<List<Person>> GenerateDefaultUsers()
        {
            return await Task.Run(() =>
            {
                var users = new List<Person>();
                var random = new Random();
                var domains = new[] { "gmail.com", "ukr.net", "outlook.com", "ukma.edu.ua" };
                var names = new[] { "Illia", "Ivan", "Petro", "Olexandr", "Viktor", "Anton", "Oleksii", "Olha", "Anna", "Julia" };
                var surnames = new[] { "Ivanenko", "Petrenko", "Poroshenko", "Symonenko", "Shevchenko", "Horbenko", "Kravchenko", "Shvets", "Antoniuk", "Hryhorenko" };

                for (int i = 0; i < 50; i++)
                {
                    var name = names[random.Next(names.Length)];
                    var surname = surnames[random.Next(surnames.Length)];
                    var email = $"{name.ToLower().First()}.{surname.ToLower()}{random.Next(1000)}@{domains[random.Next(domains.Length)]}";

                    var now = DateTime.Now;
                    var years = random.Next(1, 90);
                    var days = random.Next(1, 365);
                    var birthDate = now.AddYears(-years).AddDays(-days);

                    var person = new Person(name, surname, email, birthDate);
                    users.Add(person);
                }
                return users;
            });
        }
    }
}
