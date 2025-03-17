using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace SwaggerPetstore
{
    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Tag
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Pet
    {
        public long id { get; set; }
        public Category category { get; set; }
        public string name { get; set; }
        public List<string> photoUrls { get; set; }
        public List<Tag> tags { get; set; }
        public string status { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        static async Task GetPetsByStatusAsync(string status)
        {
            var apiUrl = "https://petstore.swagger.io/v2/pet/findByStatus?status="+status;

            using (var client = new HttpClient())
                try
                {
                    var response = await client.GetAsync(apiUrl);

                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    List<Pet> pets = JsonConvert.DeserializeObject<List<Pet>>(content);

                    FixPetListData(pets);

                    pets = pets.OrderBy(a => a.category.name).ThenByDescending(a=>a.name).ToList<Pet>();

                    foreach (Pet p in pets)
                    {
                        DisplayPet(p);
                    }                  

                    Console.WriteLine("\n");                   
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }

            Menu();
        }

        public static void FixPetListData(List<Pet> petsList)
        {
            foreach(Pet p in petsList)
            {
                FixPetData(p);
            }
        }

        public static void FixPetData(Pet p)
        {
            if (p.name == null)
                p.name = "Name Missing";

            if (p.category == null)
            {
                Category c = new Category();
                c.id = -1;
                c.name = "Category Missing";
                p.category = c;
            }
        }

        private static void DisplayPet(Pet p)
        {
            Console.Write("Status: "+p.status);
            Console.Write("\t\tCategory: "+ p.category.name);
            Console.Write("\t\tPet Name: "+ p.name);
            Console.Write("\n");
        }

        static public void Menu()
        {
            Console.WriteLine("Press 1 for list of Available pets, Press 2 for list of Pending pets, Press 3 for list of Sold pets, press C to close...");
            string readLine = Console.ReadLine();
            if (readLine == "1")
                GetPetsByStatusAsync("available").Wait();
            else if (readLine == "2")
                GetPetsByStatusAsync("pending").Wait();
            else if (readLine == "3")
                GetPetsByStatusAsync("sold").Wait();
            else if (readLine == "C")
            {
                Console.WriteLine("Closing...");
                Thread.Sleep(2000);
            }
            else
                Menu();
        }
    }
}
