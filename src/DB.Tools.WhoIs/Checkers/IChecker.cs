namespace DB.Tools.WhoIs.Checkers
{
    /// <summary>
    /// Base interface to create a Checker.
    /// </summary>
    public interface IChecker
    {
        bool Check(Person person);
    }
}
