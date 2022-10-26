namespace Garcia.Exceptions.Persistence
{
    /// <summary>
    /// Thrown if entity caching is enabled but GarciaCache is not registered.
    /// </summary>
    public class GarciaCacheInstanceMissingException : Exception
    {
        public GarciaCacheInstanceMissingException() : base("GarciaCache instance cannot be found.",
            new Exception("If entity caching enabled you must register GarciaCache instance and pass as a parameter to the constructor from inherited class.")
            )
        {
        }
    }
}