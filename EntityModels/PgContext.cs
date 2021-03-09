using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EntityModels
{
    public partial class PgContext : DbContext
    {
        public PgContext()
            : base("name=testDB")//name=PgContext
        {
        }

        public virtual DbSet<Blocks> Blocks { get; set; }
        public virtual DbSet<Burlets> Burlets { get; set; }
        public virtual DbSet<CutCompositionSides> CutCompositionSides { get; set; }
        public virtual DbSet<Cuts> Cuts { get; set; }
        public virtual DbSet<Materials> Materials { get; set; }
        public virtual DbSet<Mattresses> Mattresses { get; set; }
        public virtual DbSet<MtrsCompositions> MtrsCompositions { get; set; }
        //public virtual DbSet<MtrsCompositionSides> MtrsCompositionSides { get; set; }
        public virtual DbSet<Perimetrs> Perimetrs { get; set; }
        public virtual DbSet<Series> Series { get; set; }
        public virtual DbSet<Sizes> Sizes { get; set; }
        public virtual DbSet<Tables> Tables { get; set; }
        public virtual DbSet<TopperCompositions> TopperCompositions { get; set; }
        public virtual DbSet<TopperCutCompositiont> TopperCutCompositiont { get; set; }
        public virtual DbSet<TopperCuts> TopperCuts { get; set; }
        public virtual DbSet<Toppers> Toppers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blocks>()
                .HasMany(e => e.MtrsCompositions)
                .WithOptional(e => e.Blocks)
                .HasForeignKey(e => e.additionalBlockId);

            modelBuilder.Entity<Blocks>()
                .HasMany(e => e.MtrsCompositions1)
                .WithOptional(e => e.Blocks1)
                .HasForeignKey(e => e.blockId);

            modelBuilder.Entity<CutCompositionSides>()
                .HasMany(e => e.Cuts)
                .WithOptional(e => e.CutCompositionSides)
                .HasForeignKey(e => e.botSideCompositionId);

            modelBuilder.Entity<CutCompositionSides>()
                .HasMany(e => e.Cuts1)
                .WithOptional(e => e.CutCompositionSides1)
                .HasForeignKey(e => e.topSideCompositionId);

            modelBuilder.Entity<Cuts>()
                .HasMany(e => e.Mattresses)
                .WithRequired(e => e.Cuts)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MtrsCompositions>()
                .HasMany(e => e.Mattresses)
                .WithRequired(e => e.MtrsCompositions)
                .WillCascadeOnDelete(false);

            //modelBuilder.Entity<MtrsCompositionSides>()
            //    .HasMany(e => e.MtrsCompositions)
            //    .WithOptional(e => e.MtrsCompositionSides)
            //    .HasForeignKey(e => e.botSideCompositionId);

            //modelBuilder.Entity<MtrsCompositionSides>()
            //    .HasMany(e => e.MtrsCompositions1)
            //    .WithOptional(e => e.MtrsCompositionSides1)
            //    .HasForeignKey(e => e.topSideCompositionId);
        }
    }
}
