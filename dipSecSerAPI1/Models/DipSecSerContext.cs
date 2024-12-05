using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace dipSecSerAPI1.Models;

public partial class DipSecSerContext : DbContext
{
    public DipSecSerContext()
    {
    }

    public DipSecSerContext(DbContextOptions<DipSecSerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Gallery> Galleries { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost; Port=5432; Database=dipSecSer; Username = postgres; Password= 111;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Contacts_pkey");

            entity.Property(e => e.Email).HasMaxLength(160);
            entity.Property(e => e.Phone).HasMaxLength(40);
            entity.Property(e => e.WorkTime).HasMaxLength(40);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Feedback_pkey");

            entity.ToTable("Feedback");

            entity.Property(e => e.Name).HasMaxLength(155);
        });

        modelBuilder.Entity<Gallery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Gallery_pkey");

            entity.ToTable("Gallery");

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.ImagePath).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(155);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Project_pkey");

            entity.ToTable("Project");

            entity.Property(e => e.ComplitionDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(155);
            entity.Property(e => e.StartDate).HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Gallery).WithMany(p => p.Projects)
                .HasForeignKey(d => d.GalleryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Project_GalleryId_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.Projects)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Project_StatusId_fkey");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role_pkey");

            entity.ToTable("Role");

            entity.Property(e => e.Name).HasMaxLength(155);
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Services_pkey");

            entity.ToTable("Service");

            entity.Property(e => e.Id).HasDefaultValueSql("nextval('\"Services_Id_seq\"'::regclass)");
            entity.Property(e => e.Description).HasColumnName("Description ");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Gallery).WithMany(p => p.Services)
                .HasForeignKey(d => d.GalleryId)
                .HasConstraintName("Services_GalleryId_fkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Status_pkey");

            entity.ToTable("Status");

            entity.Property(e => e.Name).HasColumnType("character varying");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.ToTable("User");

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Login).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(155);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Patronymic).HasMaxLength(155);
            entity.Property(e => e.Phone).HasMaxLength(40);
            entity.Property(e => e.Surname).HasMaxLength(155);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("User_RoleId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
