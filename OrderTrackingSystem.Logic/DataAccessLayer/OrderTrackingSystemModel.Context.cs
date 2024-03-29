﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderTrackingSystem.Logic.DataAccessLayer
{
    using OrderTrackingSystem.Logic.Services;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OrderTrackingSystemEntities : DbContext
    {
        public OrderTrackingSystemEntities()
            : base(string.Format(ConfigurationService.D3EntityFrameworkPrefix, ConfigurationService.D3ConnectionString))
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ComplaintDefinitions> ComplaintDefinitions { get; set; }
        public virtual DbSet<ComplaintFolders> ComplaintFolders { get; set; }
        public virtual DbSet<ComplaintRelations> ComplaintRelations { get; set; }
        public virtual DbSet<ComplaintStates> ComplaintStates { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Localizations> Localizations { get; set; }
        public virtual DbSet<MailOrderRelations> MailOrderRelations { get; set; }
        public virtual DbSet<Mails> Mails { get; set; }
        public virtual DbSet<OrderCarts> OrderCarts { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderStates> OrderStates { get; set; }
        public virtual DbSet<Pickups> Pickups { get; set; }
        public virtual DbSet<ProductCategories> ProductCategories { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<SellCarts> SellCarts { get; set; }
        public virtual DbSet<Sellers> Sellers { get; set; }
        public virtual DbSet<Sells> Sells { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Vouchers> Vouchers { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Processes> Processes { get; set; }
    }
}
