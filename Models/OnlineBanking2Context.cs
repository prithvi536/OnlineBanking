using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineBanking_Final.Models;

public partial class OnlineBanking2Context : DbContext
{
    public OnlineBanking2Context()
    {
    }

    public OnlineBanking2Context(DbContextOptions<OnlineBanking2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountHolder> AccountHolders { get; set; }

    public virtual DbSet<DebitCard> DebitCards { get; set; }

    public virtual DbSet<FixedDeposit> FixedDeposits { get; set; }

    public virtual DbSet<FundTransfer> FundTransfers { get; set; }

    public virtual DbSet<OtherBankDetail> OtherBankDetails { get; set; }

    public virtual DbSet<Payee> Payees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-RQDNNSE\\SQLEXPRESS;Initial Catalog=OnlineBanking2;user id=sa;password=greysoft;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountHolder>(entity =>
        {
            entity.HasKey(e => e.AccountNumber).HasName("PK__AccountH__BE2ACD6E6E06622F");

            entity.HasIndex(e => e.CustomerId, "UQ__AccountH__A4AE64D9AD10C1D5").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__AccountH__C9F2845693857024").IsUnique();

            entity.Property(e => e.AccountNumber).ValueGeneratedNever();
            entity.Property(e => e.AccountBalance).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.AccountHoldername)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AccountOpeningDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.AccountType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ifsccode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IFSCCode");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<DebitCard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DebitCar__3214EC07D9F55DF9");

            entity.ToTable("DebitCard");

            entity.HasIndex(e => e.CardNumber, "UQ__DebitCar__A4E9FFE92FD8A267").IsUnique();

            entity.Property(e => e.AccountHoldername)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountNumberNavigation).WithMany(p => p.DebitCards)
                .HasForeignKey(d => d.AccountNumber)
                .HasConstraintName("FK__DebitCard__Accou__70DDC3D8");
        });

        modelBuilder.Entity<FixedDeposit>(entity =>
        {
            entity.HasKey(e => e.FdId).HasName("PK__FixedDep__2C2ED09E1587BFE6");

            entity.Property(e => e.FdAmount).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.MaturityAmount).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.WithdrawlDate).HasColumnName("withdrawlDate");

            entity.HasOne(d => d.AccountNumberNavigation).WithMany(p => p.FixedDeposits)
                .HasForeignKey(d => d.AccountNumber)
                .HasConstraintName("FK__FixedDepo__Accou__45F365D3");
        });

        modelBuilder.Entity<FundTransfer>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("PK__FundTran__95490091F88B5DC2");

            entity.ToTable("FundTransfer");

            entity.Property(e => e.Amount).HasColumnType("decimal(15, 2)");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.AccountHolderNumberNavigation).WithMany(p => p.FundTransfers)
                .HasForeignKey(d => d.AccountHolderNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FundTrans__Accou__46E78A0C");

            entity.HasOne(d => d.SelectedPayee).WithMany(p => p.FundTransfers)
                .HasForeignKey(d => d.SelectedPayeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FundTrans__Selec__47DBAE45");
        });

        modelBuilder.Entity<OtherBankDetail>(entity =>
        {
            entity.HasKey(e => e.OaccountNumber).HasName("PK__OtherBan__D140F784A3143065");

            entity.HasIndex(e => e.OcustomerId, "UQ__OtherBan__7223C488204CEFE0").IsUnique();

            entity.HasIndex(e => e.OuserName, "UQ__OtherBan__BE5BE36955343EAB").IsUnique();

            entity.Property(e => e.OaccountNumber)
                .ValueGeneratedNever()
                .HasColumnName("OAccountNumber");
            entity.Property(e => e.OaccountBalance)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("OAccountBalance");
            entity.Property(e => e.OaccountHoldername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OAccountHoldername");
            entity.Property(e => e.OaccountOpeningDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("OAccountOpeningDate");
            entity.Property(e => e.OaccountType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OAccountType");
            entity.Property(e => e.OcustomerId).HasColumnName("OCustomerId");
            entity.Property(e => e.Oemail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OEmail");
            entity.Property(e => e.Oifsccode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OIFSCCode");
            entity.Property(e => e.Opassword)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OPassword");
            entity.Property(e => e.Ophone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("OPhone");
            entity.Property(e => e.OtransactionPassword).HasColumnName("OTransactionPassword");
            entity.Property(e => e.OuserName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("OUserName");
        });

        modelBuilder.Entity<Payee>(entity =>
        {
            entity.HasKey(e => e.PayeeId).HasName("PK__Payee__0BC3E4D91FECB777");

            entity.ToTable("Payee");

            entity.Property(e => e.PayeeId).ValueGeneratedNever();
            entity.Property(e => e.NickName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PayeeAccountNumber).HasColumnName("payeeAccountNumber");

            entity.HasOne(d => d.AccountNumberHolderNavigation).WithMany(p => p.Payees)
                .HasForeignKey(d => d.AccountNumberHolder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payee__AccountNu__48CFD27E");

            entity.HasOne(d => d.PayeeAccountNumberNavigation).WithMany(p => p.Payees)
                .HasForeignKey(d => d.PayeeAccountNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payee__payeeAcco__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
