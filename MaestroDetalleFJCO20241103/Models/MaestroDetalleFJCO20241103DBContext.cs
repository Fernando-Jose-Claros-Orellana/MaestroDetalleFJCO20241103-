using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MaestroDetalleFJCO20241103.Models
{
    public partial class MaestroDetalleFJCO20241103DBContext : DbContext
    {
        public MaestroDetalleFJCO20241103DBContext()
        {
        }

        public MaestroDetalleFJCO20241103DBContext(DbContextOptions<MaestroDetalleFJCO20241103DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Componente> Componentes { get; set; } = null!;
        public virtual DbSet<Computadora> Computadoras { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(" workstation id=MaestroDetalleFJCO20241103DB.mssql.somee.com;packet size=4096;user id=Kamikaze_SQLLogin_3;pwd=1u9qdfiexx;data source=MaestroDetalleFJCO20241103DB.mssql.somee.com;persist security info=False;initial catalog=MaestroDetalleFJCO20241103DB;TrustServerCertificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Componente>(entity =>
            {
                entity.ToTable("Componente");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ComputadoraId).HasColumnName("computadora_id");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("precio");

                entity.Property(e => e.Tipo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("tipo");

                entity.HasOne(d => d.Computadora)
                    .WithMany(p => p.Componente)
                    .HasForeignKey(d => d.ComputadoraId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Component__preci__398D8EEE");
            });

            modelBuilder.Entity<Computadora>(entity =>
            {
                entity.ToTable("Computadora");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Marca)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("marca");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nombre");

                entity.Property(e => e.Precio)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("precio");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
