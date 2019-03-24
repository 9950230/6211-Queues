using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Queues
{

    class Program
    {
        static void Main(string[] args)
        {
            
            string json = GetJSONString("https://uinames.com/api/?region=new%20zealand&amount=10");
            List<Person> persons = DeserializeJSONString<Person>(json);

            // Queue the people
            Queue queue = new Queue();
            foreach (var person in persons)
            {
                queue.Enqueue(person.Name);
            }

            bool exit = false;
            while(!exit)
            {
                Console.WriteLine("1: Display Queued Names");
                Console.WriteLine("2: Search");
                Console.WriteLine("3: Display names in alphabetical order");
                Console.WriteLine("4: exit");
                Console.Write("Choose your option: ");
                int option = Int32.Parse( Console.ReadLine() );

                if (option == 1)
                {
                    // Dequeue the people
                    Console.WriteLine("\n>>>> Dequeue: ");
                    while(queue.Count > 0)
                    {
                        Console.WriteLine("Name: {0}", queue.Dequeue());
                    }
                    foreach (var person in persons)
                    {
                        queue.Enqueue(person.Name);
                    }
                    Console.WriteLine("\n");
                }
                else if (option == 2)
                {
                    Console.Write("\nSearch: ");
                    string search = Console.ReadLine();
                    bool contains = queue.Contains(search);
                    if(contains)
                    {
                        Console.WriteLine("\n{0} is in the queue.\n", search);

                    }
                    else
                    {
                        Console.WriteLine("\n{0} is NOT in the queue.\n", search);
                    }

                }
                else if (option == 3)
                {

                    List<string> list = new List<string>();
                    while(queue.Count > 0)
                    {
                        list.Add(queue.Dequeue().ToString());
                    }

                    foreach (var person in persons)
                    {
                        queue.Enqueue(person.Name);
                    }
                    
                    list.Sort();
                    Console.WriteLine("\n>>>> Alphabetical Order: ");
                    foreach (var name in list)
                    {
                        Console.WriteLine("Name: {0}", name);
                        
                    }
                    Console.WriteLine("\n");


                }
                else if (option == 4)
                {
                    exit = true;
                }
            }

        }

        static List<T> DeserializeJSONString<T>(string json)
        {
            try
            {
                return new JavaScriptSerializer().Deserialize<List<T>>(json);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n************************************************************\n");
                Console.WriteLine("Error: {0}", e.Message);
                Console.WriteLine("JSON String:\n{0}", json);
                Console.WriteLine("\n************************************************************\n");
                return new List<T>();
            }
        }

        static string GetJSONString(string path)
        {
            try
            {
                WebClient client = new WebClient
                {
                    Encoding = System.Text.Encoding.UTF8
                };
                return client.DownloadString(path);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n************************************************************\n");
                Console.WriteLine("Error: {0}", e.Message);
                Console.WriteLine("Path: '{0}'", path);
                Console.WriteLine("\n************************************************************\n");
                return "";
            }
        }

        class Person
        {
            public string Name { get; set; }
        }
    }
}
