namespace Team7_LonghornMusic.Migrations
{
    using CsvHelper;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<Team7_LonghornMusic.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Team7_LonghornMusic.Models.AppDbContext context)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "Team7_LonghornMusic.Data.Genres.CSV";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    var genres = csvReader.GetRecords<Genre>().ToArray();
                    context.Genres.AddOrUpdate(g => g.GenreName, genres);
                }
            }
        }
    }
}
