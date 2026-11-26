using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HousingRegistration.Model
{
    [Table("Accounts")]
    public class HousingAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountID { get; set; }

        [Required]
        [StringLength(40)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(200)]
        public string PayerName { get; set; }
    }

    [Table("Users")]
    public class RegisteredUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Index("UQ_Users_Login", IsUnique = true)]
        [Required]
        [StringLength(100)]
        public string Login { get; set; }

        [Required]
        [StringLength(64)]
        [Column(TypeName = "char")]
        public string PasswordHash { get; set; }

        public int AccountID { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class HousingContext : System.Data.Entity.DbContext
    {
        static HousingContext()
        {
            System.Data.Entity.Database.SetInitializer<HousingContext>(null);
        }

        public HousingContext() : base("HousingContext") { }

        public System.Data.Entity.DbSet<HousingAccount> Accounts { get; set; }
        public System.Data.Entity.DbSet<RegisteredUser> Users { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HousingAccount>().ToTable("Accounts");
            modelBuilder.Entity<RegisteredUser>().ToTable("Users");
            modelBuilder.Entity<RegisteredUser>().Property(u => u.AccountID).HasColumnName("AccountID");
            modelBuilder.Entity<RegisteredUser>().Property(u => u.CreatedAt).HasColumnName("CreatedAt");
            base.OnModelCreating(modelBuilder);
        }
    }
}
