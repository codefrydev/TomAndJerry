namespace TomAndJerry.Utils
{
    public interface IRandomFactsService
    {
        /// <summary>
        /// Gets a random Tom & Jerry fact
        /// </summary>
        /// <returns>A random fact string</returns>
        string GetRandomFact();

        /// <summary>
        /// Gets multiple random facts without duplicates
        /// </summary>
        /// <param name="count">Number of facts to return</param>
        /// <returns>Array of unique random facts</returns>
        string[] GetRandomFacts(int count);

        /// <summary>
        /// Gets all available facts
        /// </summary>
        /// <returns>Array of all facts</returns>
        string[] GetAllFacts();

        /// <summary>
        /// Gets the total number of available facts
        /// </summary>
        /// <returns>Count of facts</returns>
        int GetFactCount();
    }
}
