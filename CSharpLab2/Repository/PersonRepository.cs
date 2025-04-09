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
                    return JsonSerializer.Deserialize<List<Person>>(json);
                }
                return new List<Person>();
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
    }
}
