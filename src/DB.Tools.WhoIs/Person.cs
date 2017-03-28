using System;

namespace DB.Tools.WhoIs
{
    /// <summary>
    /// A person.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// A person's name.
        /// </summary>
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Person)
            {
                return (obj as Person).Name.ToLower() == Name.ToLower();
            }
            else
            {
                return base.Equals(obj);
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
