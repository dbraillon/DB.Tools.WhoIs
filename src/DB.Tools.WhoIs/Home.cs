using System;
using System.Collections.Generic;
using System.Linq;

namespace DB.Tools.WhoIs
{
    /// <summary>
    /// A home.
    /// </summary>
    public class Home
    {
        /// <summary>
        /// A home configuration.
        /// </summary>
        public HomeConfiguration Configuration { get; set; }
        
        /// <summary>
        /// Initialize and configure a home.
        /// </summary>
        /// <param name="configure">Configuration action.</param>
        public Home(Action<HomeConfiguration> configure)
        {
            Configuration = new HomeConfiguration();
            configure(Configuration);

            if (Configuration == null) throw new ArgumentNullException(nameof(Configuration));
            if (Configuration.Checkers == null || !Configuration.Checkers.Any()) throw new ArgumentException("Not configured, add at least one checker.");
            if (Configuration.Persons == null || !Configuration.Persons.Any()) throw new ArgumentException("Not configured, add at least one person.");
        }
        
        /// <summary>
        /// Check if a person is home.
        /// </summary>
        /// <param name="person">A person to check.</param>
        /// <returns>
        ///     True - if all checkers say the given person is home.
        ///     False - if at least one checker says the given person is not home.
        /// </returns>
        public bool IsAtHome(Person person)
        {
            return Configuration.Checkers.All(c => c.Check(person));
        }

        /// <summary>
        /// Give a list of all persons at home.
        /// </summary>
        /// <returns>A list of all persons at home.</returns>
        public IEnumerable<Person> WhoIsHome()
        {
            return Configuration.Persons.Where(p => IsAtHome(p));
        }
    }
}
