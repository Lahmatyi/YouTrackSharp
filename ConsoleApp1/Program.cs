using System;
using System.Linq;
using YouTrackSharp;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new BearerTokenConnection("https://ytsharp.myjetbrains.com/youtrack/",
                "perm:ZGVtbzE=.WW91VHJhY2tTaGFycA==.AX3uf8RYk3y2bupWA1xyd9BhAHoAxc");
            var service = connection.CreateIssuesService();
            var changes = service.GetChanges("DP1-1").Result;
            var first = changes.ChangesCollection.First();
            Console.WriteLine("Hello World!");
        }
    }
}
