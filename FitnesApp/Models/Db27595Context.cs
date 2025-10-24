using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace FitnesApp.Models;

public partial class Db27595Context : DbContext
{
    public Db27595Context()
    {
    }

    public Db27595Context(DbContextOptions<Db27595Context> options)
        : base(options)
    {
    }

    public virtual DbSet<ClassSignup> ClassSignups { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientMembership> ClientMemberships { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<GroupClass> GroupClasses { get; set; }

    public virtual DbSet<MembershipType> MembershipTypes { get; set; }

    public virtual DbSet<Trainer> Trainers { get; set; }

    public virtual DbSet<Visit> Visits { get; set; }

    public virtual DbSet<VwActiveClientMembership> VwActiveClientMemberships { get; set; }

    public virtual DbSet<VwClientVisitHistory> VwClientVisitHistories { get; set; }

    public virtual DbSet<VwGroupClassSchedule> VwGroupClassSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClassSignup>(entity =>
        {
            entity.HasKey(e => e.SignupId).HasName("PK__ClassSig__0D799A08609BFBC6");

            entity.Property(e => e.SignupId).HasColumnName("SignupID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.SignupDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValue("Active");

            entity.HasOne(d => d.Class).WithMany(p => p.ClassSignups)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK_ClassSignups_Classes");

            entity.HasOne(d => d.Client).WithMany(p => p.ClassSignups)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_ClassSignups_Clients");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A043CD0E2D2");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Clients__85FB4E380BD914FE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Clients__A9D1053480777A5C").IsUnique();

            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<ClientMembership>(entity =>
        {
            entity.HasKey(e => e.ClientMembershipId).HasName("PK__ClientMe__D1328747DB2433B5");

            entity.Property(e => e.ClientMembershipId).HasColumnName("ClientMembershipID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
            entity.Property(e => e.PurchaseDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientMemberships)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_ClientMemberships_Clients");

            entity.HasOne(d => d.Employee).WithMany(p => p.ClientMemberships)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientMemberships_Employees");

            entity.HasOne(d => d.MembershipType).WithMany(p => p.ClientMemberships)
                .HasForeignKey(d => d.MembershipTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientMemberships_MembershipTypes");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF116CB8BAE");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Position).HasMaxLength(50);
        });

        modelBuilder.Entity<GroupClass>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__GroupCla__CB1927A0E45F4D3D");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Schedule).HasMaxLength(255);
            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");

            entity.HasOne(d => d.Trainer).WithMany(p => p.GroupClasses)
                .HasForeignKey(d => d.TrainerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GroupClasses_Trainers");
        });

        modelBuilder.Entity<MembershipType>(entity =>
        {
            entity.HasKey(e => e.MembershipTypeId).HasName("PK__Membersh__F35A3E59B0CEC676");

            entity.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
            entity.Property(e => e.AvailableZones).HasMaxLength(255);
            entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Trainer>(entity =>
        {
            entity.HasKey(e => e.TrainerId).HasName("PK__Trainers__366A1B9C7FBE57A1");

            entity.HasIndex(e => e.PhoneNumber, "UQ__Trainers__85FB4E38E2A60A8D").IsUnique();

            entity.Property(e => e.TrainerId).HasColumnName("TrainerID");
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Specialization).HasMaxLength(100);
            entity.Property(e => e.WorkSchedule).HasMaxLength(255);
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.HasKey(e => e.VisitId).HasName("PK__Visits__4D3AA1BED7BBDAFE");

            entity.Property(e => e.VisitId).HasColumnName("VisitID");
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");

            entity.HasOne(d => d.Client).WithMany(p => p.Visits)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK_Visits_Clients");
        });

        modelBuilder.Entity<VwActiveClientMembership>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ActiveClientMemberships");

            entity.Property(e => e.ClientFullName).HasMaxLength(101);
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.MembershipName).HasMaxLength(100);
        });

        modelBuilder.Entity<VwClientVisitHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ClientVisitHistory");

            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.ClientFullName).HasMaxLength(101);
        });

        modelBuilder.Entity<VwGroupClassSchedule>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_GroupClassSchedule");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName).HasMaxLength(100);
            entity.Property(e => e.Schedule).HasMaxLength(255);
            entity.Property(e => e.Specialization).HasMaxLength(100);
            entity.Property(e => e.TrainerName).HasMaxLength(150);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
