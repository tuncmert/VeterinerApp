using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Veterinerapp.Models.DB
{
    public partial class VeterinerDBContext : DbContext
    {
        public VeterinerDBContext()
        {
        }

        public VeterinerDBContext(DbContextOptions<VeterinerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Asi> Asis { get; set; }
        public virtual DbSet<Kurum> Kurums { get; set; }
        public virtual DbSet<Not> Nots { get; set; }
        public virtual DbSet<Pet> Pets { get; set; }
        public virtual DbSet<PetSahip> PetSahips { get; set; }
        public virtual DbSet<Tedavi> Tedavis { get; set; }
        public virtual DbSet<Veteriner> Veteriners { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-1N9UI6V\\SQLEXPRESS;Database=VeterinerDB;User Id=sa;Password=Password1");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("Admin");

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.KullaniciAdi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Sifre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Soyad).HasMaxLength(50);
            });

            modelBuilder.Entity<Asi>(entity =>
            {
                entity.ToTable("Asi");

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Tarih).HasColumnType("date");

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.Asis)
                    .HasForeignKey(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Asi_Pet");

                entity.HasOne(d => d.Veteriner)
                    .WithMany(p => p.Asis)
                    .HasForeignKey(d => d.VeterinerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Asi_Veteriner");
            });

            modelBuilder.Entity<Kurum>(entity =>
            {
                entity.ToTable("Kurum");

                entity.Property(e => e.KurumAd)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.KurumIlce)
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.KurumLogo).HasColumnType("image");

                entity.Property(e => e.KurumSehir).HasMaxLength(50);
            });

            modelBuilder.Entity<Not>(entity =>
            {
                entity.ToTable("Not");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Aciklama).IsRequired();

                entity.Property(e => e.PetId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.Nots)
                    .HasForeignKey(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Not_Pet");

                entity.HasOne(d => d.Sahip)
                    .WithMany(p => p.Nots)
                    .HasForeignKey(d => d.SahipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Not_PetSahip");
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.ToTable("Pet");

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Cins).HasMaxLength(50);

                entity.Property(e => e.Cinsiyet).HasMaxLength(10);

                entity.Property(e => e.DogumTarihi).HasColumnType("date");

                entity.Property(e => e.Foto).HasColumnType("image");

                entity.Property(e => e.Kilo).HasMaxLength(50);

                entity.Property(e => e.PetTc).HasMaxLength(50);

                entity.Property(e => e.Tur).HasMaxLength(50);

                entity.Property(e => e.Yas).HasMaxLength(50);

                entity.HasOne(d => d.PetSahip)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(d => d.PetSahipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pet_PetSahip");

                entity.HasOne(d => d.Veteriner)
                    .WithMany(p => p.Pets)
                    .HasForeignKey(d => d.VeterinerId)
                    .HasConstraintName("FK_Pet_Veteriner");
            });

            modelBuilder.Entity<PetSahip>(entity =>
            {
                entity.ToTable("PetSahip");

                entity.Property(e => e.KullaniciAdi)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SahipAd)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SahipSifre)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SahipSoyad).HasMaxLength(50);

                entity.Property(e => e.SahipTelefon)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Tedavi>(entity =>
            {
                entity.ToTable("Tedavi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Ad).HasMaxLength(50);

                entity.Property(e => e.Tarih).HasColumnType("date");

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.Tedavis)
                    .HasForeignKey(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Islem_Islem");

                entity.HasOne(d => d.Veteriner)
                    .WithMany(p => p.Tedavis)
                    .HasForeignKey(d => d.VeterinerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tedavi_Veteriner");
            });

            modelBuilder.Entity<Veteriner>(entity =>
            {
                entity.ToTable("Veteriner");

                entity.Property(e => e.VeterinerAd)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VeterinerImage).HasColumnType("image");

                entity.Property(e => e.VeterinerKullaniciAdi)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.VeterinerSifre)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.VeterinerSoyad)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VeterinerTelefon)
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.HasOne(d => d.Kurum)
                    .WithMany(p => p.Veteriners)
                    .HasForeignKey(d => d.KurumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Veteriner_Kurum");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
