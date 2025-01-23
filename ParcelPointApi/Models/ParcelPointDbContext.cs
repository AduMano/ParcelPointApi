using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ParcelPointApi.Models;

public partial class ParcelPointDbContext : DbContext
{
    public ParcelPointDbContext()
    {
    }

    public ParcelPointDbContext(DbContextOptions<ParcelPointDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<IncomingParcel> IncomingParcels { get; set; }

    public virtual DbSet<ParcelLog> ParcelLogs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserGroup> UserGroups { get; set; }

    public virtual DbSet<UserGroupMember> UserGroupMembers { get; set; }

    public virtual DbSet<UserInformation> UserInformations { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    public virtual DbSet<UserRelationship> UserRelationships { get; set; }

    public virtual DbSet<UserbioFp> UserbioFps { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-0GLFD43;Database=ParcelPointDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ACTIVITY__3213E83F24BAFDDC");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GENDER__3213E83FC97D2F73");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<IncomingParcel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__INCOMING__3213E83FE8056C96");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ParcelLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PARCEL_L__3213E83FE2F71526");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ROLES__3213E83F0D62D1D5");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USERS__3213E83FCEE53FC0");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Role).WithMany(p => p.Users).HasConstraintName("FK__USERS__role_id__5AEE82B9");
        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_GRO__3213E83FD34E93E9");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Owner).WithMany(p => p.UserGroups).HasConstraintName("FK__USER_GROU__owner__6754599E");
        });

        modelBuilder.Entity<UserGroupMember>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_GRO__3213E83FCEC9D398");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsAuthorized).HasDefaultValue(false);
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Group).WithMany(p => p.UserGroupMembers).HasConstraintName("FK__USER_GROU__group__6EF57B66");

            entity.HasOne(d => d.Member).WithMany(p => p.UserGroupMembers).HasConstraintName("FK__USER_GROU__membe__6E01572D");

            entity.HasOne(d => d.Relationship).WithMany(p => p.UserGroupMembers).HasConstraintName("FK__USER_GROU__relat__6FE99F9F");
        });

        modelBuilder.Entity<UserInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_INF__3213E83F826E9EAD");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Gender).WithMany(p => p.UserInformations).HasConstraintName("FK__USER_INFO__gende__60A75C0F");

            entity.HasOne(d => d.User).WithMany(p => p.UserInformations).HasConstraintName("FK__USER_INFO__user___619B8048");
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_LOG__3213E83FA86CEBDC");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogs).HasConstraintName("FK__USER_LOGS__user___74AE54BC");
        });

        modelBuilder.Entity<UserRelationship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_REL__3213E83F1E571D01");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserbioFp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USERBIO___3213E83FB0A7F282");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
