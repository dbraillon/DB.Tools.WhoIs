using System.Collections.Generic;

namespace DB.Tools.WhoIs.Checkers
{
    /// <summary>
    /// A network checker configuration.
    /// </summary>
    public class NetworkCheckerConfiguration
    {
        /// <summary>
        /// Keep track of linked devices on every persons.
        /// </summary>
        protected Dictionary<Person, List<Device>> PersonDevices { get; set; } = new Dictionary<Person, List<Device>>();

        /// <summary>
        /// Time to wait before a ping request to be considered as lost in milliseconds.
        /// </summary>
        public int PingTimeout { get; set; } = 1000;

        /// <summary>
        /// Link a person to a device.
        /// </summary>
        /// <param name="person">A person.</param>
        /// <param name="device">A device.</param>
        public void LinkDevice(Person person, Device device)
        {
            if (!PersonDevices.ContainsKey(person))
                PersonDevices.Add(person, new List<Device>());

            var devices = PersonDevices[person];
            if (!devices.Contains(device))
                devices.Add(device);
        }

        /// <summary>
        /// Unlink a person to a device.
        /// </summary>
        /// <param name="person">A person.</param>
        /// <param name="device">A device.</param>
        public void UnlinkDevice(Person person, Device device)
        {
            if (PersonDevices.ContainsKey(person))
            {
                PersonDevices[person].Remove(device);
            }
        }

        /// <summary>
        /// Get all devices linked to a person.
        /// </summary>
        /// <param name="person">A person.</param>
        /// <returns>All devices linked to given person.</returns>
        public IEnumerable<Device> GetDevices(Person person)
        {
            var devices = new List<Device>();

            if (PersonDevices.ContainsKey(person))
            {
                devices.AddRange(PersonDevices[person]);
            }

            return devices;
        }
    }
}
