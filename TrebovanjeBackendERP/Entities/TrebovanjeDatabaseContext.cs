using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace TrebovanjeBackendERP.Entities
{
    public partial class TrebovanjeDatabaseContext : DbContext
    {
       

        public TrebovanjeDatabaseContext(DbContextOptions<TrebovanjeDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Distributer> Distributers { get; set; }
        public virtual DbSet<KategorijaProizvodum> KategorijaProizvodums { get; set; }
        public virtual DbSet<Korisnik> Korisniks { get; set; }
        public virtual DbSet<Lokacija> Lokacijas { get; set; }
        public virtual DbSet<Porudzbina> Porudzbinas { get; set; }
        public virtual DbSet<Proizvod> Proizvods { get; set; }
        public virtual DbSet<StavkaPorudzbine> StavkaPorudzbines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:TrebovanjeDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.KorisnikId).ValueGeneratedNever();

                entity.HasOne(d => d.Korisnik)
                    .WithOne(p => p.AdminNavigation)
                    .HasForeignKey<Admin>(d => d.KorisnikId)
                    .HasConstraintName("FK_Admin_Korisnik");
            });

            modelBuilder.Entity<Distributer>(entity =>
            {
                entity.Property(e => e.KorisnikId).ValueGeneratedNever();

                entity.HasOne(d => d.Korisnik)
                    .WithOne(p => p.Distributer)
                    .HasForeignKey<Distributer>(d => d.KorisnikId)
                    .HasConstraintName("FK_Distributer_Korisnik");
            });

            modelBuilder.Entity<KategorijaProizvodum>(entity =>
            {
                entity.Property(e => e.KategorijaId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Korisnik>(entity =>
            {
                entity.Property(e => e.KorisnikId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Lokacija>(entity =>
            {
                entity.Property(e => e.LokacijaId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Porudzbina>(entity =>
            {
                entity.Property(e => e.PorudzbinaId).ValueGeneratedNever();

                entity.HasOne(d => d.Distributer)
                    .WithMany(p => p.Porudzbinas)
                    .HasForeignKey(d => d.DistributerId)
                    .HasConstraintName("FK_Porudzbina_Distributer");
            });

            modelBuilder.Entity<Proizvod>(entity =>
            {
                entity.Property(e => e.ProizvodId).ValueGeneratedNever();

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Proizvods)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK_Proizvod_Korisnik");

                entity.HasOne(d => d.Kategorija)
                    .WithMany(p => p.Proizvods)
                    .HasForeignKey(d => d.KategorijaId)
                    .HasConstraintName("FK_Proizvod_Kategorija");
            });

            modelBuilder.Entity<StavkaPorudzbine>(entity =>
            {
                entity.Property(e => e.StavkaPorudzbineId).ValueGeneratedNever();

                entity.HasOne(d => d.Porudzbina)
                    .WithMany(p => p.StavkaPorudzbines)
                    .HasForeignKey(d => d.PorudzbinaId)
                    .HasConstraintName("FK_StavkaPorudzbine_Porudzbina");

                entity.HasOne(d => d.Proizvod)
                    .WithMany(p => p.StavkaPorudzbines)
                    .HasForeignKey(d => d.ProizvodId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StavkaPorudzbine_Proizvod");
            });

            modelBuilder.HasSequence("AdminSeq");

            modelBuilder.HasSequence("DistributerSeq");

            modelBuilder.HasSequence("KategorijaProizvodaSeq");

            modelBuilder.HasSequence("KorisnikSeq");

            modelBuilder.HasSequence("LokacijaSeq");

            modelBuilder.HasSequence("PorudzbinaSeq");

            modelBuilder.HasSequence("ProizvodSeq");

            modelBuilder.HasSequence("RefreshTokenSeq");

            modelBuilder.HasSequence("StavkaPorudzbineSeq");

            modelBuilder.HasSequence("TokenSeq");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
