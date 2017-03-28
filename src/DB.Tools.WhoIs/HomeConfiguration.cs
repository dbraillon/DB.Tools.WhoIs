using DB.Tools.WhoIs.Checkers;
using System.Collections.Generic;
using System.Linq;

namespace DB.Tools.WhoIs
{
    /// <summary>
    /// A home configuration.
    /// </summary>
    public class HomeConfiguration
    {
        /// <summary>
        /// List of persons at home.
        /// </summary>
        public List<Person> Persons { get; set; } = new List<Person>();

        /// <summary>
        /// List of checkers.
        /// </summary>
        internal List<IChecker> Checkers { get; set; } = new List<IChecker>();

        /// <summary>
        /// Add a checker.
        /// </summary>
        /// <param name="checker">A checker.</param>
        public void UseChecker(IChecker checker)
        {
            if (Checkers != null && !Checkers.Any(c => c.GetType() == checker.GetType()))
            {
                Checkers.Add(checker);
            }
        }
    }
}
