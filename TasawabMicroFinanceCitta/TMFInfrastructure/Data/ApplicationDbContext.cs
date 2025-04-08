using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using TMFDomain.Entities.Authentication;
using TMFDomain.Entities.Loan;
using TMFDomain.Entities.Staff;
using TMFDomain.Interfaces.Loan;
using TMFDomain.Shared;
using TMFDomain.Entities.Transaction;
using TMFDomain.Entities.Accounts;
using TMFDomain.Entities.Client;

namespace TMFInfrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Existing DbSets
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<StaffPersonalInformation> StaffPersonalInformation { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ClientInformation> ClientInformation { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<LoanApplication> LoanApplications { get; set; }
        public DbSet<ApprovalTracking> ApprovalTrackings { get; set; }

        // New DbSets for microfinance
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountEntityType> AccountEntityTypes { get; set; }
        public DbSet<AccountEntity> Accounts { get; set; }
        public DbSet<AccountOwner> AccountOwners { get; set; }
        public DbSet<Trans> Transactions { get; set; }
        public DbSet<TransactionCharge> TransactionCharges { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity entity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entity.created_at = DateTime.Now;
                    }
                    entity.updated_at = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Exclude BaseEntity from being created as a table
            modelBuilder.Ignore<BaseEntity>();

            // Define primary keys for existing entities
            modelBuilder.Entity<User>().HasKey(u => u.staff_code);
            modelBuilder.Entity<StaffPersonalInformation>().HasKey(spi => spi.staff_code);
            modelBuilder.Entity<ClientInformation>().HasKey(ci => ci.ClientId);
            modelBuilder.Entity<Branch>().HasKey(b => b.branch_id);
            modelBuilder.Entity<Department>().HasKey(d => d.department_id);
            modelBuilder.Entity<Position>().HasKey(p => p.position_id);
            modelBuilder.Entity<Role>().HasKey(r => r.role_id);
            modelBuilder.Entity<LoanType>().HasKey(lt => lt.LoanTypeId);
            modelBuilder.Entity<LoanApplication>().HasKey(la => la.LoanApplicationId);
            modelBuilder.Entity<ApprovalTracking>().HasKey(at => at.ApprovalTrackingId);

            // Define primary keys for new entities
            modelBuilder.Entity<AccountType>().HasKey(at => at.AccountTypeCode);
            modelBuilder.Entity<AccountEntityType>().HasKey(aet => aet.EntityTypeCode);
            modelBuilder.Entity<AccountEntity>().HasKey(a => a.AccountCode);
            modelBuilder.Entity<AccountOwner>().HasKey(ao => ao.Id);
            modelBuilder.Entity<Trans>().HasKey(t => t.Id);
            modelBuilder.Entity<TransactionCharge>().HasKey(tc => tc.Id);

            // Configure relationships
            modelBuilder.Entity<AccountEntity>()
                .HasOne(a => a.AccountType)
                .WithMany()
                .HasForeignKey(a => a.AccountTypeCode);

            modelBuilder.Entity<AccountEntity>()
                .HasOne(a => a.AccountEntityType)
                .WithMany()
                .HasForeignKey(a => a.EntityTypeCode);

            modelBuilder.Entity<AccountOwner>()
                .HasOne(ao => ao.Account)
                .WithMany(a => a.AccountOwners)
                .HasForeignKey(ao => ao.AccountCode);

            modelBuilder.Entity<AccountOwner>()
                .HasOne(ao => ao.Client)
                .WithMany(c => c.AccountOwners)
                .HasForeignKey(ao => ao.ClientId);

            modelBuilder.Entity<Trans>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountCode);

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Privileges)
                .WithMany()
                .UsingEntity(j => j.ToTable("RolePrivileges"));

            modelBuilder.Entity<Branch>()
                .Property(b => b.branch_id)
                .ValueGeneratedOnAdd();

            // Configure ClientId as auto-increment
            modelBuilder.Entity<ClientInformation>()
                .Property(c => c.ClientId)
                .ValueGeneratedOnAdd();
        }
    }
}