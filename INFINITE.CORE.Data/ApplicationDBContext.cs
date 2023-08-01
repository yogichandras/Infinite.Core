using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using INFINITE.CORE.Data.Model;


namespace INFINITE.CORE.Data
{
    public partial class ApplicationDBContext : DbContext
    {
        public virtual DbSet<Config> Config { get; set; }
        public virtual DbSet<EmailTemplate> EmailTemplate { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<Repository> Repository { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolePermissions> RolePermissions { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>(entity =>
            {
                entity.ToTable("CONFIG");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.ConfigKey)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CONFIG_KEY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.ConfigValue)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("CONFIG_VALUE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Module)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODULE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");
            });

            modelBuilder.Entity<EmailTemplate>(entity =>
            {
                entity.ToTable("EMAIL_TEMPLATE");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("DISPLAY_NAME")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.MailContent)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("MAIL_CONTENT")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.MailFrom)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MAIL_FROM")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Module)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("MODULE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("SUBJECT")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("NOTIFICATION");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.Property(e => e.IsOpen).HasColumnName("IS_OPEN");

                entity.Property(e => e.Navigation)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("NAVIGATION")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("SUBJECT")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UserFullname)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("USER_FULLNAME")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UserMail)
                    .HasMaxLength(150)
                    .HasColumnName("USER_MAIL")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(50)
                    .HasColumnName("USER_PHONE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Notification)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("NOTIFICATION_FK");
            });

            modelBuilder.Entity<Repository>(entity =>
            {
                entity.ToTable("REPOSITORY");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("CODE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description)
                    .IsUnicode(false)
                    .HasColumnName("DESCRIPTION")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("EXTENSION")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("FILE_NAME")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.IsPublic).HasColumnName("IS_PUBLIC");

                entity.Property(e => e.MimeType)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("MIME_TYPE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Modul)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("MODUL")
                    .UseCollation("Latin1_General_CI_AS");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("ROLE");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("NAME")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");
            });

            modelBuilder.Entity<RolePermissions>(entity =>
            {
                entity.ToTable("ROLE_PERMISSIONS");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdRole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_ROLE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Permission)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("PERMISSION")
                    .UseCollation("Latin1_General_CI_AS");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_ROLE_PERMISSIONS_ROLE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("USER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AccessFailedCount).HasColumnName("ACCESS_FAILED_COUNT");

                entity.Property(e => e.IsDeleted).HasColumnName("IS_DELETED");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Fullname)
                    .IsUnicode(false)
                    .HasColumnName("FULLNAME")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.IsLockout).HasColumnName("IS_LOCKOUT");

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("MAIL")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("PASSWORD")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("PHONE_NUMBER")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.Token)
                    .HasMaxLength(250)
                    .HasColumnName("TOKEN")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateBy)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("UPDATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("UPDATE_DATE");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("USERNAME")
                    .UseCollation("Latin1_General_CI_AS");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("USER_ROLE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateBy)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("CREATE_BY")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("CREATE_DATE")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IdRole)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("ID_ROLE")
                    .UseCollation("Latin1_General_CI_AS");

                entity.Property(e => e.IdUser).HasColumnName("ID_USER");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_USER_ROLE_ROLE");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_USER_ROLE_USER");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
