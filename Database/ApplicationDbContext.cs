using ImdbClone.Api.DTOs;
using ImdbClone.Api.Entities;
using ImdbClone.Api.Enums;
using ImdbClone.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace ImdbClone.Api.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public ApplicationDbContext() { }

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
        public DbSet<TitleSearchResultDto> TitleSearchResults { get; set; }
        public DbSet<PersonSearchResultDto> PersonSearchResults { get; set; }
        public DbSet<PersonWithProfessionDto> PersonWithProfessionDto { get; set; }
        public DbSet<WordFrequencyDto> WordFrequencies { get; set; }
        public DbSet<SimilarTitleDto> SimilarTitles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST");
            var database = Environment.GetEnvironmentVariable("DB_NAME");
            var username = Environment.GetEnvironmentVariable("DB_USER");
            var password = Environment.GetEnvironmentVariable("DB_PASS");

            var connectionString =
                $"Host={host};Database={database};Username={username};Password={password}";

            if (!options.IsConfigured)
            {
                options.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<TitleSearchResultDto>().HasNoKey().ToView(null);
            mb.Entity<PersonSearchResultDto>().HasNoKey().ToView(null);
            mb.Entity<PersonWithProfessionDto>().HasNoKey().ToView(null);
            mb.Entity<WordFrequencyDto>().HasNoKey().ToView(null);
            mb.Entity<SimilarTitleDto>().HasNoKey().ToView(null);

            mb.Entity<Title>().Property(t => t.TitleType).HasConversion<string>();
            mb.Entity<TitlePerson>().Property(tp => tp.Category).HasConversion<string>();

            mb.Entity<TitlePerson>()
                .Property(e => e.Category)
                .HasConversion(
                    v => StringHelper.ToSnakeCase(v.ToString()),
                    v =>
                        (PersonCategory)
                            Enum.Parse(typeof(PersonCategory), StringHelper.ToPascalCase(v))
                );

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
            mb.Entity<TitlePerson>()
                .HasKey(tp => new
                {
                    tp.Tconst,
                    tp.Nconst,
                    tp.Ordering,
                });
            mb.Entity<UserRating>().HasKey(ur => new { ur.UserId, ur.Tconst });
            mb.Entity<BookmarkTitle>().HasKey(bt => new { bt.UserId, bt.Tconst });
            mb.Entity<BookmarkPerson>().HasKey(bp => new { bp.UserId, bp.Nconst });

            // fks and relationships
            mb.Entity<TitleAlias>().HasOne(t => t.Title).WithMany().HasForeignKey(t => t.Tconst);

            mb.Entity<TitlePerson>()
                .HasOne(tp => tp.Title)
                .WithMany(t => t.TitlePeople)
                .HasForeignKey(tp => tp.Tconst)
                .HasPrincipalKey(t => t.Tconst);

            mb.Entity<TitlePerson>()
                .HasOne(tp => tp.Person)
                .WithMany()
                .HasForeignKey(tp => tp.Nconst)
                .HasPrincipalKey(p => p.Nconst);

            mb.Entity<Episode>()
                .HasOne(e => e.ParentTitle)
                .WithMany()
                .HasForeignKey(e => e.ParentTconst);

            mb.Entity<Rating>().HasOne(r => r.Title).WithMany().HasForeignKey(r => r.Tconst);

            mb.Entity<UserRating>()
                .HasOne(ur => ur.User)
                .WithMany(ur => ur.UserRatings)
                .HasForeignKey(ur => ur.UserId)
                .HasPrincipalKey(ur => ur.UserId);

            mb.Entity<UserRating>()
                .HasOne(ur => ur.Title)
                .WithMany(t => t.UserRatings)
                .HasForeignKey(ur => ur.Tconst)
                .HasPrincipalKey(ur => ur.Tconst);

            mb.Entity<BookmarkTitle>()
                .HasOne(bt => bt.User)
                .WithMany(u => u.BookmarkTitles)
                .HasForeignKey(b => b.UserId)
                .HasPrincipalKey(u => u.UserId);

            mb.Entity<BookmarkTitle>()
                .HasOne(bt => bt.Title)
                .WithMany(t => t.BookmarkTitles)
                .HasForeignKey(bt => bt.Tconst)
                .HasPrincipalKey(t => t.Tconst);

            mb.Entity<BookmarkPerson>()
                .HasOne(bp => bp.User)
                .WithMany(u => u.BookmarkPersons)
                .HasForeignKey(bp => bp.UserId)
                .HasPrincipalKey(u => u.UserId);

            mb.Entity<BookmarkPerson>()
                .HasOne(bp => bp.Person)
                .WithMany(p => p.BookmarkPersons)
                .HasForeignKey(bp => bp.Nconst)
                .HasPrincipalKey(p => p.Nconst);

            mb.Entity<SearchHistory>()
                .HasOne(sh => sh.User)
                .WithMany()
                .HasForeignKey(sh => sh.UserId);

            // many to many relationships + column mapping
            mb.Entity<Person>()
                .HasMany(p => p.KnownForTitles)
                .WithMany(t => t.KnownForByPeople)
                .UsingEntity(
                    "person_known_for_title",
                    l =>
                        l.HasOne(typeof(Title))
                            .WithMany()
                            .HasForeignKey("tconst")
                            .HasPrincipalKey(nameof(Title.Tconst)),
                    r =>
                        r.HasOne(typeof(Person))
                            .WithMany()
                            .HasForeignKey("nconst")
                            .HasPrincipalKey(nameof(Person.Nconst)),
                    j => j.HasKey("nconst", "tconst")
                );

            mb.Entity<Person>()
                .HasMany(p => p.Professions)
                .WithMany(pr => pr.People)
                .UsingEntity(
                    "person_profession",
                    l =>
                        l.HasOne(typeof(Profession))
                            .WithMany()
                            .HasForeignKey("profession_id")
                            .HasPrincipalKey(nameof(Profession.ProfessionId)),
                    r =>
                        r.HasOne(typeof(Person))
                            .WithMany()
                            .HasForeignKey("nconst")
                            .HasPrincipalKey(nameof(Person.Nconst)),
                    j => j.HasKey("profession_id", "nconst")
                );

            mb.Entity<Title>()
                .HasMany(t => t.Genres)
                .WithMany(g => g.Titles)
                .UsingEntity(
                    "title_genre",
                    l =>
                        l.HasOne(typeof(Genre))
                            .WithMany()
                            .HasForeignKey("genre_id")
                            .HasPrincipalKey(nameof(Genre.GenreId)),
                    r =>
                        r.HasOne(typeof(Title))
                            .WithMany()
                            .HasForeignKey("tconst")
                            .HasPrincipalKey(nameof(Title.Tconst)),
                    j => j.HasKey("tconst", "genre_id")
                );

            mb.Entity<Title>()
                .HasMany(t => t.Countries)
                .WithMany(c => c.Titles)
                .UsingEntity(
                    "title_country",
                    l =>
                        l.HasOne(typeof(Country))
                            .WithMany()
                            .HasForeignKey("country_id")
                            .HasPrincipalKey(nameof(Country.CountryId)),
                    r =>
                        r.HasOne(typeof(Title))
                            .WithMany()
                            .HasForeignKey("tconst")
                            .HasPrincipalKey(nameof(Title.Tconst)),
                    j =>
                    {
                        j.ToTable("title_country");
                        j.HasKey("tconst", "country_id");
                    }
                );
        }
    }
}
