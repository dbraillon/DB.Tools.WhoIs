namespace DB.Tools.WhoIs
{
    /// <summary>
    /// A person's device.
    /// </summary>
    public class Device
    {
        /// <summary>
        /// A device name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A device host name or address. 
        /// </summary>
        public string HostNameOrAddress { get; set; }

        /// <summary>
        /// Initialize a new device.
        /// </summary>
        /// <param name="name">A device name.</param>
        /// <param name="hostNameOrAddress">A device host name or address.</param>
        public Device(string name, string hostNameOrAddress)
        {
            Name = name;
            HostNameOrAddress = hostNameOrAddress;
        }

        public override bool Equals(object obj)
        {
            if (obj is Device)
            {
                return
                    (obj as Device).Name == Name ||
                    (obj as Device).HostNameOrAddress == HostNameOrAddress;
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
