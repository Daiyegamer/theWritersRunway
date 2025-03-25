using AdilBooks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace AdilBooks.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // AdilBooks Tables
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }

        // FashionVote Tables
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Designer> Designers { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<DesignerShow> DesignerShows { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<ParticipantShow> ParticipantShows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity configurations are applied

            // AdilBooks: Many-to-Many between Books and Authors
           

            // FashionVote: Many-to-Many between Designer and Show
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

            // FashionVote: One-to-Many Votes (Participant votes for Designer in a Show)
            modelBuilder.Entity<Vote>()
                .HasKey(v => v.VoteId);

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

            // Prevent duplicate votes: One vote per participant-designer-show
            modelBuilder.Entity<Vote>()
                .HasIndex(v => new { v.ParticipantId, v.DesignerId, v.ShowId })
                .IsUnique();

            // FashionVote: Many-to-Many for Participant and Shows
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
        }
    }
}
