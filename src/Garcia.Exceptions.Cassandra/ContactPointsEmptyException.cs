namespace Garcia.Exceptions.Cassandra
{
    /// <summary>
    /// The exception that is thrown when contact points field of settings null or does not contain any element
    /// </summary>
    public class ContactPointsEmptyException : Exception
    {
        public ContactPointsEmptyException() 
            : base("ContactPoints field must be contain at least 1 contact point")
        {
        }
    }
}
