using System;
using System.Data.Entity;
using FaaS.Entities.DataAccessModels;

namespace FaaS.Entities.Contexts
{
    internal class FaaSContext : DbContext
    {
        /// <summary>
        /// Exposes logging capabilities to <see cref="Repositories.UserRepository"/>.
        /// </summary>
        internal Action<string> Log { get; set; }

        /// <summary>
        /// This constructor is required by migrations
        /// </summary>
        public FaaSContext()
            : base()
        {
        }

        /// <summary>
        /// This constructors should be used in code
        /// </summary>
        /// <param name="nameOrConnectionString"></param>
        public FaaSContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            // safely invokes Log property action only if specified
            Database.Log = log => Log?.Invoke(log);
        }

        /// <summary>
        /// Method specifies specific database details related to the used models
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {/*
            // Solve M:N relation
            modelBuilder
                .Entity<Award>()
                .HasMany(award => award.Games)
                .WithMany(game => game.Awards)
                .Map(map => map.ToTable("GameAwards"));*/

            // Solve 1:N relation
            modelBuilder
                .Entity<User>()
                .HasMany(user => user.Projects)
                .WithRequired(project => project.User)
                .WillCascadeOnDelete();

            modelBuilder
                .Entity<Project>()
                .HasMany(project => project.Forms)
                .WithRequired(form => form.Project)
                .WillCascadeOnDelete();

            modelBuilder
                .Entity<Form>()
                .HasMany(form => form.Elements)
                .WithRequired(element => element.Form)
                .WillCascadeOnDelete();

            modelBuilder
                .Entity<Element>()
                .HasMany(element => element.Options)
                .WithRequired(option => option.Element)
                .WillCascadeOnDelete();

            modelBuilder
                .Entity<Element>()
                .HasMany(element => element.ElementValues)
                .WithRequired(elementValue => elementValue.Element)
                .WillCascadeOnDelete();

            modelBuilder
                .Entity<Session>()
                .HasMany(session => session.ElementValues)
                .WithRequired(elementValue => elementValue.Session)
                .WillCascadeOnDelete();

            // Base implementation is empty, however, this is not necessarily granted
            base.OnModelCreating(modelBuilder);
        }

        // Each relevant non-relational database table should be specified by a DbSet

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<Form> Forms { get; set; }

        public virtual DbSet<Element> Elements { get; set; }

        public virtual DbSet<Option> Options { get; set; }

        public virtual DbSet<ElementValue> ElementValues { get; set; }

        public virtual DbSet<Session> Sessions { get; set; }
    }
}
