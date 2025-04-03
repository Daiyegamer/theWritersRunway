using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


using AdilBooks.Models;


namespace AdilBooks.Data 
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // AdilBooks
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<PublisherShow> PublisherShows { get; set; }


        // FashionVote
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Designer> Designers { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<DesignerShow> DesignerShows { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<ParticipantShow> ParticipantShows { get; set; }
        public DbSet<DesignerBook> DesignerBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => ba.BookAuthorId);

            modelBuilder.Entity<DesignerShow>()
                .HasKey(ds => new { ds.DesignerId, ds.ShowId });

            modelBuilder.Entity<DesignerShow>()
                .HasOne(ds => ds.Designer)
                .WithMany(d => d.DesignerShows)
                .HasForeignKey(ds => ds.DesignerId);

            modelBuilder.Entity<DesignerShow>()
                .HasOne(ds => ds.Show)
                .WithMany(s => s.DesignerShows)
                .HasForeignKey(ds => ds.ShowId);

            modelBuilder.Entity<Vote>().HasKey(v => v.VoteId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Participant)
                .WithMany(p => p.Votes)
                .HasForeignKey(v => v.ParticipantId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Designer)
                .WithMany(d => d.Votes)
                .HasForeignKey(v => v.DesignerId);

            modelBuilder.Entity<Vote>()
                .HasOne(v => v.Show)
                .WithMany(s => s.Votes)
                .HasForeignKey(v => v.ShowId);

            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.ParticipantId, v.DesignerId, v.ShowId })
                .IsUnique();

            modelBuilder.Entity<ParticipantShow>()
                .HasKey(ps => new { ps.ParticipantId, ps.ShowId });

            modelBuilder.Entity<ParticipantShow>()
                .HasOne(ps => ps.Participant)
                .WithMany(p => p.ParticipantShows)
                .HasForeignKey(ps => ps.ParticipantId);

            modelBuilder.Entity<ParticipantShow>()
                .HasOne(ps => ps.Show)
                .WithMany(s => s.ParticipantShows)
                .HasForeignKey(ps => ps.ShowId);

            modelBuilder.Entity<PublisherShow>()
        .HasKey(ps => new { ps.PublisherId, ps.ShowId });

            modelBuilder.Entity<PublisherShow>()
                .HasOne(ps => ps.Publisher)
                .WithMany(p => p.PublisherShows)
                .HasForeignKey(ps => ps.PublisherId);

            modelBuilder.Entity<PublisherShow>()
                .HasOne(ps => ps.Show)
                .WithMany(s => s.PublisherShows)
                .HasForeignKey(ps => ps.ShowId);

            // Designer Book Model
            modelBuilder.Entity<DesignerBook>()
                .HasKey(db => new { db.DesignerId, db.BookId });

            modelBuilder.Entity<DesignerBook>()
                .HasOne(db => db.Designer)
                .WithMany(d => d.DesignerBooks)
                .HasForeignKey(db => db.DesignerId);

            modelBuilder.Entity<DesignerBook>()
                .HasOne(db => db.Book)
                .WithMany(b => b.DesignerBooks)
                .HasForeignKey(db => db.BookId);
        }
    }
}
