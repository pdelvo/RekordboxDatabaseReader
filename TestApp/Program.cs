using RekordboxDatabaseReader;
using System;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var device = new RekordboxDevice(@"D:\\RB"))
            {
                var library = device.Library;

                foreach (var item in library.Tracks)
                {
                    Console.WriteLine($"Name: {item.Name}, Path: {item.Path}, Artist Id: {item.ArtistId}");
                }

                foreach (var item in library.Artists)
                {
                    Console.WriteLine($"Name: {item.Name}, Artist Id: {item.ArtistId}");
                }
            }
        }
    }
}
