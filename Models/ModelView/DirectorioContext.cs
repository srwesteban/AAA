using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DirectorioMVC.Models.ModelView;

public partial class DirectorioContext : DbContext
{
    public DirectorioContext()
    {
    }

    public DirectorioContext(DbContextOptions<DirectorioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contacto> Contactos { get; set; }

    public virtual DbSet<Telefono> Telefonos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contacto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("contactos_pkey");

            entity.ToTable("contactos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(50)
                .HasColumnName("apellido");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Telefono>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("telefonos_pkey");

            entity.ToTable("telefonos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContactoId).HasColumnName("contacto_id");
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(15)
                .HasColumnName("numero_telefono");
            entity.Property(e => e.TipoTelefono)
                .HasMaxLength(20)
                .HasColumnName("tipo_telefono");

            entity.HasOne(d => d.Contacto).WithMany(p => p.Telefonos)
                .HasForeignKey(d => d.ContactoId)
                .HasConstraintName("telefonos_contacto_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}