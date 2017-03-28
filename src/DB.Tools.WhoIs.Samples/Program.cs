using DB.Tools.WhoIs.Checkers;
using System;
using System.Linq;

namespace DB.Tools.WhoIs.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create all persons
            var myself = new Person("Myself");
            var melanie = new Person("Melanie");
            var davy = new Person("Davy");
            var eva = new Person("Eva");

            // Create and configure a home
            var home = new Home((conf) =>
            {
                // Add created persons to home
                conf.Persons.Add(myself);
                conf.Persons.Add(melanie);
                conf.Persons.Add(davy);
                conf.Persons.Add(eva);

                // Use the network checker
                conf.UseChecker(new NetworkChecker(conf, (checkConf) =>
                {
                    // Configure the ping timeout
                    checkConf.PingTimeout = 5000;

                    // Add and link devices to its owner
                    checkConf.LinkDevice(myself, new Device("My device", "127.0.0.1"));
                    checkConf.LinkDevice(melanie, new Device("Samsung Galaxy Ace", "192.168.0.13"));
                    checkConf.LinkDevice(davy, new Device("Samsung Galaxy A3 (2016)", "192.168.0.12"));
                    checkConf.LinkDevice(eva, new Device("iPhone 6S", "192.168.0.18"));
                }));
            });

            // Check each person presence
            var amIHome = home.IsAtHome(myself);
            var isDavyHome = home.IsAtHome(davy);
            var isMelanieHome = home.IsAtHome(melanie);
            var isEvaHome = home.IsAtHome(eva);

            // Write a fancy report
            Console.WriteLine($"i> {myself} {(amIHome ? "is" : "is not")} home");
            Console.WriteLine($"i> {davy} {(isDavyHome ? "is" : "is not")} home");
            Console.WriteLine($"i> {melanie} {(isMelanieHome ? "is" : "is not")} home");
            Console.WriteLine($"i> {eva} {(isEvaHome ? "is" : "is not")} home");
            
            // Configure and start thread
            var homeThread = new HomeThread(home);
            homeThread.SleepTime = 1000;
            homeThread.Tolerence = 4;
            homeThread.OnPersonPresenceChanges += (p, h) => Console.WriteLine($"{p} {(h ? "is" : "is not")} home");
            homeThread.Start();
            
            // Start on infinite loop, type 'quit' to ... quit !
            var input = string.Empty;
            while (input != "quit")
            {
                input = Console.ReadLine();

                if (input.ToLower() == "who is home?")
                {
                    var presentPeople = homeThread.WhoIsHome();
                    if (presentPeople.Any()) Console.WriteLine(string.Join(", ", presentPeople.Select(p => p.Name)));
                    else Console.WriteLine("No one");
                }
                else if (input.ToLower() == "am i home?")
                {
                    var isHome = home.IsAtHome(myself);
                    Console.WriteLine(isHome ? "yes" : "no");
                }
            }

            homeThread.Stop();
        }
    }
}