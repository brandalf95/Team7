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

            

            Artist c1 = new Artist();
            c1.ArtistName = "LMFAO";
            context.Artists.AddOrUpdate(a => a.ArtistName, c1);
            context.SaveChanges();

            c1 = context.Artists.FirstOrDefault(a => a.ArtistName == "LMFAO");
            c1.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c1);
            context.SaveChanges();


            Artist c2 = new Artist();
            c2.ArtistName = "ADELE";
            context.Artists.AddOrUpdate(a => a.ArtistName, c2);
            context.SaveChanges();
            c2 = context.Artists.FirstOrDefault(a => a.ArtistName == "ADELE");
            c2.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c2);
            context.SaveChanges();

            Artist c3 = new Artist();
            c3.ArtistName = "Foster the People";
            context.Artists.AddOrUpdate(a => a.ArtistName, c3);
            context.SaveChanges();
            c3 = context.Artists.FirstOrDefault(a => a.ArtistName == "Foster the People");
            c3.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Alternative"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c3);
            context.SaveChanges();

            Artist c4 = new Artist();
            c4.ArtistName = "Maroon 5";
            context.Artists.AddOrUpdate(a => a.ArtistName, c4);
            context.SaveChanges();
            c4 = context.Artists.FirstOrDefault(a => a.ArtistName == "Maroon 5");
            c4.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c4);
            context.SaveChanges();

            Artist c5 = new Artist();
            c5.ArtistName = "David Guetta";
            context.Artists.AddOrUpdate(a => a.ArtistName, c5);
            context.SaveChanges();
            c5 = context.Artists.FirstOrDefault(a => a.ArtistName == "David Guetta");
            c5.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Dance"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c5);
            context.SaveChanges();

            Artist c1000 = new Artist();
            c1000.ArtistName = "Usher";
            context.Artists.AddOrUpdate(a => a.ArtistName, c1000);
            context.SaveChanges();
            c1000 = context.Artists.FirstOrDefault(a => a.ArtistName == "Usher");
            c1000.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Dance"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c1000);
            context.SaveChanges();

            Artist c6 = new Artist();
            c6.ArtistName = "Lady GaGa";
            context.Artists.AddOrUpdate(a => a.ArtistName, c6);
            context.SaveChanges();
            c6 = context.Artists.FirstOrDefault(a => a.ArtistName == "Lady GaGa");
            c6.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c6);
            context.SaveChanges();

            Artist c7 = new Artist();
            c7.ArtistName = "Rihanna";
            context.Artists.AddOrUpdate(a => a.ArtistName, c7);
            context.SaveChanges();
            c7 = context.Artists.FirstOrDefault(a => a.ArtistName == "Rihanna");
            c7.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c7);
            context.SaveChanges();

            Artist c8 = new Artist();
            c8.ArtistName = "Blake Shelton";
            context.Artists.AddOrUpdate(a => a.ArtistName, c8);
            context.SaveChanges();
            c8 = context.Artists.FirstOrDefault(a => a.ArtistName == "Blake Shelton");
            c8.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            c8.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c8);
            context.SaveChanges();

            Artist c9 = new Artist();
            c9.ArtistName = "Nicki Minaj";
            context.Artists.AddOrUpdate(a => a.ArtistName, c9);
            context.SaveChanges();
            c9 = context.Artists.FirstOrDefault(a => a.ArtistName == "Nicki Minaj");
            c9.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c9);
            context.SaveChanges();

            Artist c10 = new Artist();
            c10.ArtistName = "Kanye West & JAY Z";
            context.Artists.AddOrUpdate(a => a.ArtistName, c10);
            context.SaveChanges();
            c10 = context.Artists.FirstOrDefault(a => a.ArtistName == "Kanye West & JAY Z");
            c10.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c10);
            context.SaveChanges();

            Artist c1001 = new Artist();
            c1001.ArtistName = "Jay-Z";
            context.Artists.AddOrUpdate(a => a.ArtistName, c1001);
            context.SaveChanges();
            c1001 = context.Artists.FirstOrDefault(a => a.ArtistName == "Jay-Z");
            c1001.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c1001);
            context.SaveChanges();

            Artist c11 = new Artist();
            c11.ArtistName = "Luke Bryan";
            context.Artists.AddOrUpdate(a => a.ArtistName, c11);
            context.SaveChanges();
            c11 = context.Artists.FirstOrDefault(a => a.ArtistName == "Luke Bryan");
            c11.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c11);
            context.SaveChanges();

            Artist c12 = new Artist();
            c12.ArtistName = "The Band Perry";
            context.Artists.AddOrUpdate(a => a.ArtistName, c12);
            context.SaveChanges();
            c12 = context.Artists.FirstOrDefault(a => a.ArtistName == "The Band Perry");
            c12.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c12);
            context.SaveChanges();

            Artist c13 = new Artist();
            c13.ArtistName = "Selena Gomez & the Scene";
            context.Artists.AddOrUpdate(a => a.ArtistName, c13);
            context.SaveChanges();
            c13 = context.Artists.FirstOrDefault(a => a.ArtistName == "Selena Gomez & the Scene");
            c13.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c13);
            context.SaveChanges();

            Artist c14 = new Artist();
            c14.ArtistName = "Lady Antebellum";
            context.Artists.AddOrUpdate(a => a.ArtistName, c14);
            context.SaveChanges();
            c14 = context.Artists.FirstOrDefault(a => a.ArtistName == "Lady Antebellum");
            c14.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c14);
            context.SaveChanges();

            Artist c15 = new Artist();
            c15.ArtistName = "Eli Young Band";
            context.Artists.AddOrUpdate(a => a.ArtistName, c15);
            context.SaveChanges();
            c15 = context.Artists.FirstOrDefault(a => a.ArtistName == "Eli Young Band");
            c15.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c15);
            context.SaveChanges();

            Artist c16 = new Artist();
            c16.ArtistName = "The Byars Family";
            context.Artists.AddOrUpdate(a => a.ArtistName, c16);
            context.SaveChanges();
            c16 = context.Artists.FirstOrDefault(a => a.ArtistName == "The Byars Family");
            c16.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c16);
            context.SaveChanges();

            Artist c17 = new Artist();
            c17.ArtistName = "Drake";
            context.Artists.AddOrUpdate(a => a.ArtistName, c17);
            context.SaveChanges();
            c17 = context.Artists.FirstOrDefault(a => a.ArtistName == "Drake");
            c17.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c17);
            context.SaveChanges();

            Artist c18 = new Artist();
            c18.ArtistName = "Gym Class Heroes";
            context.Artists.AddOrUpdate(a => a.ArtistName, c18);
            context.SaveChanges();
            c18 = context.Artists.FirstOrDefault(a => a.ArtistName == "Gym Class Heroes");
            c18.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c18);
            context.SaveChanges();

            Artist c19 = new Artist();
            c19.ArtistName = "Justin Bieber";
            context.Artists.AddOrUpdate(a => a.ArtistName, c19);
            context.SaveChanges();
            c19 = context.Artists.FirstOrDefault(a => a.ArtistName == "Justin Bieber");
            c19.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Holiday"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c19);
            context.SaveChanges();

            Artist c20 = new Artist();
            c20.ArtistName = "Coldplay";
            context.Artists.AddOrUpdate(a => a.ArtistName, c20);
            context.SaveChanges();
            c20 = context.Artists.FirstOrDefault(a => a.ArtistName == "Coldplay");
            c20.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Alternative"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c20);
            context.SaveChanges();

            Artist c21 = new Artist();
            c21.ArtistName = "Snoop Dogg";
            context.Artists.AddOrUpdate(a => a.ArtistName, c21);
            context.SaveChanges();
            c21 = context.Artists.FirstOrDefault(a => a.ArtistName == "Snoop Dogg");
            c21.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c21);
            context.SaveChanges();

            Artist c2100 = new Artist();
            c2100.ArtistName = "Wiz Khalifa";
            context.Artists.AddOrUpdate(a => a.ArtistName, c2100);
            context.SaveChanges();
            c2100 = context.Artists.FirstOrDefault(a => a.ArtistName == "Wiz Khalifa");
            c2100.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c2100);
            context.SaveChanges();

            Artist c22 = new Artist();
            c22.ArtistName = "Cobra Starship";
            context.Artists.AddOrUpdate(a => a.ArtistName, c22);
            context.SaveChanges();
            c22 = context.Artists.FirstOrDefault(a => a.ArtistName == "Cobra Starship");
            c22.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c22);
            context.SaveChanges();

            Artist c23 = new Artist();
            c23.ArtistName = "Jason Derulo";
            context.Artists.AddOrUpdate(a => a.ArtistName, c23);
            context.SaveChanges();
            c23 = context.Artists.FirstOrDefault(a => a.ArtistName == "Jason Derulo");
            c23.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c23);
            context.SaveChanges();

            Artist c24 = new Artist();
            c24.ArtistName = "Kelly Clarkson";
            context.Artists.AddOrUpdate(a => a.ArtistName, c24);
            context.SaveChanges();
            c24 = context.Artists.FirstOrDefault(a => a.ArtistName == "Kelly Clarkson");
            c24.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c24);
            context.SaveChanges();

            Artist c25 = new Artist();
            c25.ArtistName = "T-Pain";
            context.Artists.AddOrUpdate(a => a.ArtistName, c25);
            context.SaveChanges();
            c25 = context.Artists.FirstOrDefault(a => a.ArtistName == "T-Pain");
            c25.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c25);
            context.SaveChanges();

            Artist c26 = new Artist();
            c26.ArtistName = "Flo Rida";
            context.Artists.AddOrUpdate(a => a.ArtistName, c26);
            context.SaveChanges();
            c26 = context.Artists.FirstOrDefault(a => a.ArtistName == "Flo Rida");
            c26.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c26);
            context.SaveChanges();

            Artist c27 = new Artist();
            c27.ArtistName = "DEV";
            context.Artists.AddOrUpdate(a => a.ArtistName, c27);
            context.SaveChanges();
            c27 = context.Artists.FirstOrDefault(a => a.ArtistName == "DEV");
            c27.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c27);
            context.SaveChanges();

            Artist c28 = new Artist();
            c28.ArtistName = "Bruno Mars";
            context.Artists.AddOrUpdate(a => a.ArtistName, c28);
            context.SaveChanges();
            c28 = context.Artists.FirstOrDefault(a => a.ArtistName == "Bruno Mars");
            c28.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c28);
            context.SaveChanges();

            Artist c29 = new Artist();
            c29.ArtistName = "Christina Perri";
            context.Artists.AddOrUpdate(a => a.ArtistName, c29);
            context.SaveChanges();
            c29 = context.Artists.FirstOrDefault(a => a.ArtistName == "Christina Perri");
            c29.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c29);
            context.SaveChanges();

            Artist c30 = new Artist();
            c30.ArtistName = "B.o.B";
            context.Artists.AddOrUpdate(a => a.ArtistName, c30);
            context.SaveChanges();
            c30 = context.Artists.FirstOrDefault(a => a.ArtistName == "B.o.B");
            c30.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c30);
            context.SaveChanges();

            Artist c31 = new Artist();
            c31.ArtistName = "Pitbull";
            context.Artists.AddOrUpdate(a => a.ArtistName, c31);
            context.SaveChanges();
            c31 = context.Artists.FirstOrDefault(a => a.ArtistName == "Pitbull");
            c31.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c31);
            context.SaveChanges();

            Artist c32 = new Artist();
            c32.ArtistName = "Wale";
            context.Artists.AddOrUpdate(a => a.ArtistName, c32);
            context.SaveChanges();
            c32 = context.Artists.FirstOrDefault(a => a.ArtistName == "Wale");
            c32.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c32);
            context.SaveChanges();

            Artist c33 = new Artist();
            c33.ArtistName = "Alexandra Stan";
            context.Artists.AddOrUpdate(a => a.ArtistName, c33);
            context.SaveChanges();
            c33 = context.Artists.FirstOrDefault(a => a.ArtistName == "Alexandra Stan");
            c33.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Dance"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c33);
            context.SaveChanges();

            Artist c34 = new Artist();
            c34.ArtistName = "Nickelback";
            context.Artists.AddOrUpdate(a => a.ArtistName, c34);
            context.SaveChanges();
            c34 = context.Artists.FirstOrDefault(a => a.ArtistName == "Nickelback");
            c34.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Rock"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c34);
            context.SaveChanges();

            Artist c35 = new Artist();
            c35.ArtistName = "Rick Ross";
            context.Artists.AddOrUpdate(a => a.ArtistName, c35);
            context.SaveChanges();
            c35 = context.Artists.FirstOrDefault(a => a.ArtistName == "Rick Ross");
            c35.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c35);
            context.SaveChanges();

            Artist c36 = new Artist();
            c36.ArtistName = "Waka Flocka Flame";
            context.Artists.AddOrUpdate(a => a.ArtistName, c36);
            context.SaveChanges();
            c36 = context.Artists.FirstOrDefault(a => a.ArtistName == "Waka Flocka Flame");
            c36.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c36);
            context.SaveChanges();

            Artist c37 = new Artist();
            c37.ArtistName = "Florence + the Machine";
            context.Artists.AddOrUpdate(a => a.ArtistName, c37);
            context.SaveChanges();
            c37 = context.Artists.FirstOrDefault(a => a.ArtistName == "Florence + the Machine");
            c37.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Alternative"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c37);
            context.SaveChanges();

            Artist c38 = new Artist();
            c38.ArtistName = "Jessie J";
            context.Artists.AddOrUpdate(a => a.ArtistName, c38);
            context.SaveChanges();
            c38 = context.Artists.FirstOrDefault(a => a.ArtistName == "Jessie J");
            c38.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c38);
            context.SaveChanges();

            Artist c39 = new Artist();
            c39.ArtistName = "Martin Solveig";
            context.Artists.AddOrUpdate(a => a.ArtistName, c39);
            context.SaveChanges();
            c39 = context.Artists.FirstOrDefault(a => a.ArtistName == "Martin Solveig");
            c39.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Dance"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c39);
            context.SaveChanges();

            Artist c3900 = new Artist();
            c3900.ArtistName = "Dragonette";
            context.Artists.AddOrUpdate(a => a.ArtistName, c3900);
            context.SaveChanges();
            c3900 = context.Artists.FirstOrDefault(a => a.ArtistName == "Dragonette");
            c3900.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Dance"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c3900);
            context.SaveChanges();

            Artist c40 = new Artist();
            c40.ArtistName = "Jake Owen";
            context.Artists.AddOrUpdate(a => a.ArtistName, c40);
            context.SaveChanges();
            c40 = context.Artists.FirstOrDefault(a => a.ArtistName == "Jake Owen");
            c40.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c40);
            context.SaveChanges();

            Artist c41 = new Artist();
            c41.ArtistName = "Sean Paul";
            context.Artists.AddOrUpdate(a => a.ArtistName, c41);
            context.SaveChanges();
            c41 = context.Artists.FirstOrDefault(a => a.ArtistName == "Sean Paul");
            c41.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c41);
            context.SaveChanges();

            Artist c42 = new Artist();
            c42.ArtistName = "Miranda Lambert";
            context.Artists.AddOrUpdate(a => a.ArtistName, c42);
            context.SaveChanges();
            c42 = context.Artists.FirstOrDefault(a => a.ArtistName == "Miranda Lambert");
            c42.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c42);
            context.SaveChanges();

            Artist c43 = new Artist();
            c43.ArtistName = "Hot Chelle Rae";
            context.Artists.AddOrUpdate(a => a.ArtistName, c43);
            context.SaveChanges();
            c43 = context.Artists.FirstOrDefault(a => a.ArtistName == "Hot Chelle Rae");
            c43.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c43);
            context.SaveChanges();

            Artist c44 = new Artist();
            c44.ArtistName = "Roscoe Dash";
            context.Artists.AddOrUpdate(a => a.ArtistName, c44);
            context.SaveChanges();
            c44 = context.Artists.FirstOrDefault(a => a.ArtistName == "Roscoe Dash");
            c44.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c44);
            context.SaveChanges();

            Artist c45 = new Artist();
            c45.ArtistName = "Chevelle";
            context.Artists.AddOrUpdate(a => a.ArtistName, c45);
            context.SaveChanges();
            c45 = context.Artists.FirstOrDefault(a => a.ArtistName == "Chevelle");
            c45.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Rock"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c45);
            context.SaveChanges();

            Artist c46 = new Artist();
            c46.ArtistName = "James Bay";
            context.Artists.AddOrUpdate(a => a.ArtistName, c46);
            context.SaveChanges();
            c46 = context.Artists.FirstOrDefault(a => a.ArtistName == "James Bay");
            c46.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Alternative"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c46);
            context.SaveChanges();

            Artist c47 = new Artist();
            c47.ArtistName = "Ariana Grande";
            context.Artists.AddOrUpdate(a => a.ArtistName, c47);
            context.SaveChanges();
            c47 = context.Artists.FirstOrDefault(a => a.ArtistName == "Ariana Grande");
            c47.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c47);
            context.SaveChanges();

            Artist c48 = new Artist();
            c48.ArtistName = "Sam Hunt";
            context.Artists.AddOrUpdate(a => a.ArtistName, c48);
            context.SaveChanges();
            c48 = context.Artists.FirstOrDefault(a => a.ArtistName == "Sam Hunt");
            c48.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Country"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c48);
            context.SaveChanges();

            Artist c49 = new Artist();
            c49.ArtistName = "One Direction";
            context.Artists.AddOrUpdate(a => a.ArtistName, c49);
            context.SaveChanges();
            c49 = context.Artists.FirstOrDefault(a => a.ArtistName == "One Direction");
            c49.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c49);
            context.SaveChanges();

            Artist c50 = new Artist();
            c50.ArtistName = "Nick Jonas";
            context.Artists.AddOrUpdate(a => a.ArtistName, c50);
            context.SaveChanges();
            c50 = context.Artists.FirstOrDefault(a => a.ArtistName == "Nick Jonas");
            c50.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c50);
            context.SaveChanges();

            Artist c51 = new Artist();
            c51.ArtistName = "Mark Ronson";
            context.Artists.AddOrUpdate(a => a.ArtistName, c51);
            context.SaveChanges();
            c51 = context.Artists.FirstOrDefault(a => a.ArtistName == "Mark Ronson");
            c51.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c51);
            context.SaveChanges();

            Artist c52 = new Artist();
            c52.ArtistName = "Hozier";
            context.Artists.AddOrUpdate(a => a.ArtistName, c52);
            context.SaveChanges();
            c52 = context.Artists.FirstOrDefault(a => a.ArtistName == "Hozier");
            c52.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Alternative"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c52);
            context.SaveChanges();

            Artist c53 = new Artist();
            c53.ArtistName = "Kendrick Lamar";
            context.Artists.AddOrUpdate(a => a.ArtistName, c53);
            context.SaveChanges();
            c53 = context.Artists.FirstOrDefault(a => a.ArtistName == "Kendrick Lamar");
            c53.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c53);
            context.SaveChanges();

            Artist c54 = new Artist();
            c54.ArtistName = "FLOW";
            context.Artists.AddOrUpdate(a => a.ArtistName, c54);
            context.SaveChanges();
            c54 = context.Artists.FirstOrDefault(a => a.ArtistName == "FLOW");
            c54.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            c54.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "J-Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c54);
            context.SaveChanges();

            Artist c55 = new Artist();
            c55.ArtistName = "Hans Zimmer";
            context.Artists.AddOrUpdate(a => a.ArtistName, c55);
            context.SaveChanges();
            c55 = context.Artists.FirstOrDefault(a => a.ArtistName == "Hans Zimmer");
            c55.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Classical"));
            c55.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Soundtrack"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c55);
            context.SaveChanges();

            Artist c550 = new Artist();
            c550.ArtistName = "James Newton Howard";
            context.Artists.AddOrUpdate(a => a.ArtistName, c550);
            context.SaveChanges();
            c550 = context.Artists.FirstOrDefault(a => a.ArtistName == "James Newton Howard");
            c550.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Classical"));
            c550.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Soundtrack"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c550);
            context.SaveChanges();

            Artist c56 = new Artist();
            c56.ArtistName = "Andain";
            context.Artists.AddOrUpdate(a => a.ArtistName, c56);
            context.SaveChanges();
            c56 = context.Artists.FirstOrDefault(a => a.ArtistName == "Andain");
            c56.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Progressive Trance"));
            c56.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Dance"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c56);
            context.SaveChanges();

            Artist c57 = new Artist();
            c57.ArtistName = "Bryant Oden";
            context.Artists.AddOrUpdate(a => a.ArtistName, c57);
            context.SaveChanges();
            c57 = context.Artists.FirstOrDefault(a => a.ArtistName == "Bryant Oden");
            c57.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Singer/Songwriter"));
            c57.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Chidren's Music"));
            c57.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Comedy"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c57);
            context.SaveChanges();

            Artist c58 = new Artist();
            c58.ArtistName = "Jay-Z";
            context.Artists.AddOrUpdate(a => a.ArtistName, c58);
            context.SaveChanges();
            c58 = context.Artists.FirstOrDefault(a => a.ArtistName == "Jay-Z");
            c58.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            c58.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Nu Metal"));
            c58.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Alternative"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c58);
            context.SaveChanges();

            Artist c580 = new Artist();
            c580.ArtistName = "Linkin Park";
            context.Artists.AddOrUpdate(a => a.ArtistName, c580);
            context.SaveChanges();
            c580 = context.Artists.FirstOrDefault(a => a.ArtistName == "Linkin Park");
            c580.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Alternative"));
            c580.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Nu Metal"));
            c580.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Hip Hop/Rap"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c580);
            context.SaveChanges();
            
            Artist c59 = new Artist();
            c59.ArtistName = "Julian Smith";
            context.Artists.AddOrUpdate(a => a.ArtistName, c59);
            context.SaveChanges();
            c59 = context.Artists.FirstOrDefault(a => a.ArtistName == "Julian Smith");
            c59.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Comedy"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c59);
            context.SaveChanges();

            Artist c60 = new Artist();
            c60.ArtistName = "Malvina Reynolds";
            context.Artists.AddOrUpdate(a => a.ArtistName, c60);
            context.SaveChanges();
            c60 = context.Artists.FirstOrDefault(a => a.ArtistName == "Malvina Reynolds");
            c60.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Folk"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c60);
            context.SaveChanges();

            Artist c61 = new Artist();
            c61.ArtistName = "Peter, Paul & Mary";
            context.Artists.AddOrUpdate(a => a.ArtistName, c61);
            context.SaveChanges();
            c61 = context.Artists.FirstOrDefault(a => a.ArtistName == "Peter, Paul & Mary");
            c61.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Singer/Songwriter"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c61);
            context.SaveChanges();

            Artist c62 = new Artist();
            c62.ArtistName = "Bobby McFerrin";
            context.Artists.AddOrUpdate(a => a.ArtistName, c62);
            context.SaveChanges();
            c62 = context.Artists.FirstOrDefault(a => a.ArtistName == "Bobby McFerrin");
            c62.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Reggae"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c62);
            context.SaveChanges();

            Artist c63 = new Artist();
            c63.ArtistName = "Calvin Harris";
            context.Artists.AddOrUpdate(a => a.ArtistName, c63);
            context.SaveChanges();
            c63 = context.Artists.FirstOrDefault(a => a.ArtistName == "Calvin Harris");
            c63.ArtistGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            context.Artists.AddOrUpdate(a => a.ArtistName, c63);
            context.SaveChanges();

            Album a1 = new Album();
            a1.AlbumTitle = "Sorry for Party Rocking (Deluxe Version)";
            a1.AlbumPrice = Convert.ToDecimal("9.99");
            context.Albums.AddOrUpdate(a => a.AlbumTitle, a1);
            context.SaveChanges();

            a1 = context.Albums.FirstOrDefault(a => a.AlbumTitle == "Sorry for Party Rocking (Deluxe Version)");
            a1.AlbumGenres.Add(context.Genres.FirstOrDefault(g => g.GenreName == "Pop"));
            a1.AlbumArtists.Add(context.Artists.FirstOrDefault(g => g.ArtistName == "LMFAO"));
            context.Albums.AddOrUpdate(a => a.AlbumTitle, a1);
            context.SaveChanges();


        }
    }
}
