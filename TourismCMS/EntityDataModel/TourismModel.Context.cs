﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourismCMS.EntityDataModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TourismDbContext : DbContext
    {
        public TourismDbContext()
            : base("name=TourismDbContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Channel> Channel { get; set; }
        public virtual DbSet<Hotel> Hotel { get; set; }
        public virtual DbSet<HotelOrder> HotelOrder { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<RouteOrder> RouteOrder { get; set; }
        public virtual DbSet<Supplier> Supplier { get; set; }
        public virtual DbSet<SupplierProduct> SupplierProduct { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketOrder> TicketOrder { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
    }
}
