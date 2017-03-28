using DB.Tools.WhoIs.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DB.Tools.WhoIs
{
    /// <summary>
    /// A home thread, keep track of persons presence.
    /// </summary>
    public class HomeThread : SimpleThread
    {
        /// <summary>
        /// Time to wait between two presence checks en milliseconds.
        /// </summary>
        public override int SleepTime { get; set; } = 1000;

        /// <summary>
        /// Number of negative check result for a person to be considered not here.
        /// </summary>
        public int Tolerence { get; set; } = 3;

        /// <summary>
        /// An action called when a person presence changes.
        /// </summary>
        public event Action<Person, bool> OnPersonPresenceChanges;

        /// <summary>
        /// A reference to the home.
        /// </summary>
        protected Home Home { get; }

        /// <summary>
        /// Keep track of a person presence.
        /// </summary>
        protected Dictionary<Person, int> PersonCounter { get; } = new Dictionary<Person, int>();

        /// <summary>
        /// Initialize the home thread.
        /// </summary>
        /// <param name="home">A reference to the home.</param>
        public HomeThread(Home home)
        {
            Home = home;
        }
        
        /// <summary>
        /// Main loop.
        /// </summary>
        public override void Execute()
        {
            AddMissingPersons();
            UpdateCounter(GetPersonPresences());
        }

        /// <summary>
        /// Add missing persons to PersonPresences.
        /// </summary>
        public void AddMissingPersons()
        {
            var registeredPersons = Home.Configuration.Persons;
            
            foreach (var registeredPerson in registeredPersons)
            {
                if (!PersonCounter.ContainsKey(registeredPerson))
                {
                    PersonCounter.Add(registeredPerson, 0);
                }
            }
        }

        /// <summary>
        /// Retrieve a dictionary of person presences.
        /// </summary>
        /// <returns>A dictionary with Key representing a Person and Value its presence.</returns>
        public Dictionary<Person, bool> GetPersonPresences()
        {
            return Home.Configuration.Persons.ToDictionary(p => p, p => Home.IsAtHome(p));
        }

        /// <summary>
        /// Update counter in counter tracker dictionary.
        /// </summary>
        /// <param name="personPresences">A dictionary to give a person presence.</param>
        public void UpdateCounter(Dictionary<Person, bool> personPresences)
        {
            foreach (var person in personPresences)
            {
                if (personPresences.ContainsKey(person.Key))
                {
                    if (personPresences[person.Key])
                    {
                        var oldValue = PersonCounter[person.Key];
                        if (oldValue == Tolerence) OnPersonPresenceChanges?.Invoke(person.Key, true);

                        PersonCounter[person.Key] = 0;
                    }
                    else
                    {
                        if (PersonCounter[person.Key] < Tolerence)
                        {
                            PersonCounter[person.Key] += 1;

                            if (PersonCounter[person.Key] == Tolerence)
                            {
                                OnPersonPresenceChanges?.Invoke(person.Key, false);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The thread version of IsAtHome, does not use Checker but use its internal tracker instead.
        /// </summary>
        /// <param name="person">A person to check.</param>
        /// <returns>
        ///     True - if all checkers say the given person is home.
        ///     False - if at least one checker says the given person is not home.
        /// </returns>
        public bool IsAtHome(Person person)
        {
            return PersonCounter.ContainsKey(person) ?
                PersonCounter[person] < Tolerence :
                false;
        }


        /// <summary>
        /// The thread version of WhoIsHome, does not use Checker but use its internal tracker instead.
        /// </summary>
        /// <returns>A list of all persons at home.</returns>
        public IEnumerable<Person> WhoIsHome()
        {
            return PersonCounter.Where(p => p.Value < Tolerence).Select(p => p.Key);
        }
    }
}
