using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using pharmace.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace pharmace;

public partial class PharmacyContext : DbContext
{
    public PharmacyContext()
    {
    }

    public PharmacyContext(DbContextOptions<PharmacyContext> options)
        : base(options)
    {
    }
    public virtual DbSet<userpermations> userpermations { get; set; }
    public virtual DbSet<orderprodect> orderprodects { get; set; }
    public virtual DbSet<Cart> Carts { get; set; }
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<Orders> Orders { get; set; }
    public virtual DbSet<Proudect> Proudects { get; set; }
    public virtual DbSet<Offer> Offers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    // PharmacyContext.cs
    public DbSet<CarouselImage> CarouselImages { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseSqlServer("Server=db8580.public.databaseasp.net; Database=db8580; User Id=db8580; Password=i_Removed_IT_Lol; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>()
    .HasKey(e => new { e.proudectId, e.userId }); 
        modelBuilder.Entity<orderprodect>()
    .HasKey(e => new { e.proudectId, e.orderId });

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
