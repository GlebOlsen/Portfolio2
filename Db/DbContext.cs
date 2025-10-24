using IMDB.DataService.Db.Entities;
using Microsoft.EntityFrameworkCore;


namespace IMDB.DataService.Db
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }


        public DatabaseContext() { }


        public DbSet<Title> Titles => Set<Title>();
        public DbSet<TitleAlias> TitleAliases => Set<TitleAlias>();
        public DbSet<Person> People => Set<Person>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Profession> Professions => Set<Profession>();
        public DbSet<Country> Countries => Set<Country>();


        public DbSet<TitlePerson> TitlePeople => Set<TitlePerson>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<Episode> Episodes => Set<Episode>();


        public DbSet<ImdbUser> ImdbUsers => Set<ImdbUser>();
        public DbSet<UserRating> UserRatings => Set<UserRating>();
        public DbSet<BookmarkTitle> BookmarkTitles => Set<BookmarkTitle>();
        public DbSet<BookmarkPerson> BookmarkPeople => Set<BookmarkPerson>();
        public DbSet<SearchHistory> SearchHistories => Set<SearchHistory>();


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var username = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASS");


            var connectionString = $"Host={host};Database={database};Username={username};Password={password}";


            if (!options.IsConfigured)
            {
                options.UseNpgsql(connectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder mb)
        {
            // table names
            mb.Entity<Title>().ToTable("title");
            mb.Entity<TitleAlias>().ToTable("title_alias");
            mb.Entity<Person>().ToTable("person");
            mb.Entity<TitlePerson>().ToTable("title_person");
            mb.Entity<Genre>().ToTable("genre");
            mb.Entity<Profession>().ToTable("profession");
            mb.Entity<Country>().ToTable("country");
            mb.Entity<Episode>().ToTable("episode");
            mb.Entity<Rating>().ToTable("rating");
            mb.Entity<ImdbUser>().ToTable("imdb_user");
            mb.Entity<UserRating>().ToTable("user_rating");
            mb.Entity<BookmarkTitle>().ToTable("bookmark_title");
            mb.Entity<BookmarkPerson>().ToTable("bookmark_person");
            mb.Entity<SearchHistory>().ToTable("search_history");


            // pks
            mb.Entity<Title>().HasKey(t => t.Tconst);
            mb.Entity<Person>().HasKey(p => p.Nconst);
            mb.Entity<Genre>().HasKey(g => g.GenreId);
            mb.Entity<Profession>().HasKey(pr => pr.ProfessionId);
            mb.Entity<Country>().HasKey(c => c.CountryId);
            mb.Entity<Rating>().HasKey(r => r.Tconst);
            mb.Entity<Episode>().HasKey(e => e.Tconst);
            mb.Entity<ImdbUser>().HasKey(u => u.UserId);
            mb.Entity<SearchHistory>().HasKey(sh => sh.HistoryId);
            mb.Entity<TitleAlias>().HasKey(t => new { t.Tconst, t.Ordering });
            mb.Entity<TitlePerson>().HasKey(tp => new { tp.Tconst, tp.Nconst, tp.Ordering });
            mb.Entity<UserRating>().HasKey(ur => new { ur.UserId, ur.Tconst });
            mb.Entity<BookmarkTitle>().HasKey(bt => new { bt.UserId, bt.Tconst });
            mb.Entity<BookmarkPerson>().HasKey(bp => new { bp.UserId, bp.Nconst });


            // fks and relationships
            mb.Entity<TitleAlias>()
                .HasOne(t => t.Title)
                .WithMany()
                .HasForeignKey(t => t.Tconst);


            mb.Entity<TitlePerson>()
                .HasOne(tp => tp.Title)
                .WithMany()
                .HasForeignKey(tp => tp.Tconst);


            mb.Entity<TitlePerson>()
                .HasOne(tp => tp.Person)
                .WithMany()
                .HasForeignKey(tp => tp.Nconst);


            mb.Entity<Episode>()
                .HasOne(e => e.Title)
                .WithMany()
                .HasForeignKey(e => e.Tconst);


            mb.Entity<Episode>()
                .HasOne(e => e.ParentTitle)
                .WithMany()
                .HasForeignKey(e => e.ParentTconst);


            mb.Entity<Rating>()
                .HasOne(r => r.Title)
                .WithMany()
                .HasForeignKey(r => r.Tconst);


            mb.Entity<UserRating>()
                .HasOne(ur => ur.User)
                .WithMany()
                .HasForeignKey(ur => ur.UserId);


            mb.Entity<UserRating>()
                .HasOne(ur => ur.Title)
                .WithMany()
                .HasForeignKey(ur => ur.Tconst);


            mb.Entity<BookmarkTitle>()
                .HasOne(bt => bt.User)
                .WithMany()
                .HasForeignKey(bt => bt.UserId);


            mb.Entity<BookmarkTitle>()
                .HasOne(bt => bt.Title)
                .WithMany()
                .HasForeignKey(bt => bt.Tconst);


            mb.Entity<BookmarkPerson>()
                .HasOne(bp => bp.User)
                .WithMany()
                .HasForeignKey(bp => bp.UserId);


            mb.Entity<BookmarkPerson>()
                .HasOne(bp => bp.Person)
                .WithMany()
                .HasForeignKey(bp => bp.Nconst);


            mb.Entity<SearchHistory>()
                .HasOne(sh => sh.User)
                .WithMany()
                .HasForeignKey(sh => sh.UserId);


            // many to many relationships
            mb.Entity<Person>()
                .HasMany(p => p.KnownForTitles)
                .WithMany(t => t.KnownForByPeople)
                .UsingEntity(j => j.ToTable("person_known_for_title"));


            mb.Entity<Person>()
                .HasMany(p => p.Professions)
                .WithMany(pr => pr.People)
                .UsingEntity(j => j.ToTable("person_profession"));


            mb.Entity<Title>()
                .HasMany(t => t.Genres)
                .WithMany(g => g.Titles)
                .UsingEntity(j => j.ToTable("title_genre"));


            mb.Entity<Title>()
                .HasMany(t => t.Countries)
                .WithMany(c => c.Titles)
                .UsingEntity(j => j.ToTable("title_country"));
        }
    }
}