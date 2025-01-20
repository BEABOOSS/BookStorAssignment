namespace BookStore.Models
{
    public class DatabaseSettings
    {

        public string? ConnectionString { get; set; }
        public string? PersistenceProvider { get; set; }
        public int MaxRetryCount { get; set; }
        public TimeSpan CommandTimeout { get; set; }


    }
}
