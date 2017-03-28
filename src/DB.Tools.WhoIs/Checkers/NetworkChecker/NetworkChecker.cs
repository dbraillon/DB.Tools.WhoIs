using System;
using System.Net.NetworkInformation;

namespace DB.Tools.WhoIs.Checkers
{
    /// <summary>
    /// Network checker tries to ping every registered person's devices to check a person presence.
    /// </summary>
    public class NetworkChecker : IChecker
    {
        /// <summary>
        /// A reference to a home configuration.
        /// </summary>
        public HomeConfiguration HomeConfiguration { get; }

        /// <summary>
        /// A network checker configuration.
        /// </summary>
        public NetworkCheckerConfiguration Configuration { get; set; }

        /// <summary>
        /// Initialize and configure a network checker.
        /// </summary>
        /// <param name="homeConfiguration">A reference to a home configuration.</param>
        /// <param name="configure">Action to configure network checker.</param>
        public NetworkChecker(HomeConfiguration homeConfiguration, Action<NetworkCheckerConfiguration> configure)
        {
            HomeConfiguration = homeConfiguration;

            Configuration = new NetworkCheckerConfiguration();
            configure(Configuration);

            if (Configuration == null) throw new ArgumentNullException(nameof(Configuration));
            if (HomeConfiguration == null) throw new ArgumentNullException(nameof(HomeConfiguration));
        }

        /// <summary>
        /// Ping an address to check its status.
        /// </summary>
        /// <param name="hostNameOrAddress">A host name or an IP address.</param>
        /// <returns>
        ///     True - ping reply status Success.
        ///     False - every other reply status.
        /// </returns>
        public bool Ping(string hostNameOrAddress)
        {
            var ping = new Ping();
            var result = ping.SendPingAsync(hostNameOrAddress, Configuration.PingTimeout).GetAwaiter().GetResult();

            return result.Status == IPStatus.Success;
        }

        /// <summary>
        /// Check a person presence with current checker.
        /// </summary>
        /// <param name="person">A person to check.</param>
        /// <returns>
        ///     True - Given person is here.
        ///     False - Given person is not here.
        /// </returns>
        public bool Check(Person person)
        {
            var personDevices = Configuration.GetDevices(person);

            foreach (var personDevice in personDevices)
            {
                var isHere = Ping(personDevice.HostNameOrAddress);
                if (isHere)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
