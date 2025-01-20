namespace BookStore.RegistrationExtensions
{
    public static class SqliteRegistrationExtension
    {
        public static void UseSqliteStorageProvider(this IServiceCollection services)
        {
            //var settings = new DatabaseSettings();

            //services.AddSingleton(e =>
            //{
            //    //var configuration = e.GetRequiredService<ApplicationCon>
            //    //var settings = new DatabaseSettings();
            //    //// binds every possible value to the setting obj
            //    //builder.Configuration.GetSection("DatabaseSettings").Bind(settings);
            //    //// registers the settings obj as singleton
            //    //builder.Services.AddSingleton(settings);
            //});
        }
    }
}
