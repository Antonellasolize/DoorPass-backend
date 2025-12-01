using Microsoft.EntityFrameworkCore;
using ReservasDiscoteca.API.Entities;

namespace ReservasDiscoteca.API.Data
{
    public class AppDbContext : DbContext
    {
        // 🔥 Constructor CORRECTO
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 📌 Registro de todas las tablas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Boliche> Boliches { get; set; }
        public DbSet<ManillaTipo> ManillaTipos { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<CompraManilla> CompraManillas { get; set; }

        // --- TABLAS NUEVAS ---
        public DbSet<Combo> Combos { get; set; }
        public DbSet<CompraCombo> CompraCombos { get; set; }
        // ----------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Relaciones de Boliche ---
            modelBuilder.Entity<Boliche>()
                .HasMany(b => b.ManillaTipos)
                .WithOne(m => m.Boliche)
                .HasForeignKey(m => m.BolicheId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Boliche>()
                .HasMany(b => b.Mesas)
                .WithOne(m => m.Boliche)
                .HasForeignKey(m => m.BolicheId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- RELACIÓN NUEVA ---
            modelBuilder.Entity<Boliche>()
                .HasMany(b => b.Combos)
                .WithOne(c => c.Boliche)
                .HasForeignKey(c => c.BolicheId)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Relaciones de Compra ---
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Compras)
                .WithOne(c => c.Usuario)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Boliche)
                .WithMany()
                .HasForeignKey(c => c.BolicheId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mesa>()
                .HasOne(m => m.Compra)
                .WithOne(c => c.MesaReservada)
                .HasForeignKey<Mesa>(m => m.CompraId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Relación M-M: Compra <-> ManillaTipo
            modelBuilder.Entity<Compra>()
                .HasMany(c => c.ManillasCompradas)
                .WithOne(cm => cm.Compra)
                .HasForeignKey(cm => cm.CompraId);

            modelBuilder.Entity<CompraManilla>()
                .HasOne(cm => cm.ManillaTipo)
                .WithMany()
                .HasForeignKey(cm => cm.ManillaTipoId);

            // --- RELACIÓN NUEVA M-M: Compra <-> Combo ---
            modelBuilder.Entity<Compra>()
                .HasMany(c => c.CombosComprados)
                .WithOne(cc => cc.Compra)
                .HasForeignKey(cc => cc.CompraId);

            modelBuilder.Entity<CompraCombo>()
                .HasOne(cc => cc.Combo)
                .WithMany()
                .HasForeignKey(cc => cc.ComboId);
        }
    }
}
