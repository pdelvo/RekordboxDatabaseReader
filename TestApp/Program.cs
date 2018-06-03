using RekordboxDatabaseReader;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var device = new RekordboxDevice(@"I:"))
            {
                var library = device.Library;

                foreach (var item in library.ListTracks())
                {
                    Console.WriteLine($"Name: {item.Name}, Path: {item.Path}");
                }
            }
        }
    }
}
