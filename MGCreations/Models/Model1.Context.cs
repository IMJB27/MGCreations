﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MGCreations.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mgcreationsEntities : DbContext
    {
        public mgcreationsEntities()
            : base("name=mgcreationsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<cart> carts { get; set; }
        public virtual DbSet<delivery_address> delivery_address { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<product> products { get; set; }
        public virtual DbSet<product_category> product_category { get; set; }
        public virtual DbSet<product_images> product_images { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
