﻿using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using EasyAccess.Model.EDMs;

namespace EasyAccess.Repository.Configurations.EntityFramework
{
    public class EasyAccessContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Menu> Menus { get; set; } 

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<Register> Registers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Account>()
                .HasRequired(x => x.Register)
                .WithRequiredDependent(x => x.Account);
            modelBuilder.Entity<Account>()
                .HasMany(x => x.Roles)
                .WithMany(x=>x.Accounts)
                .Map(x => x.MapLeftKey("AccountId").MapRightKey("RoleId").ToTable("AccountRole"));
            modelBuilder.Entity<Role>()
                .HasMany(x => x.Permissions)
                .WithMany(x => x.Roles)
                .Map(x => x.MapLeftKey("RoleId").MapRightKey("PermissionId").ToTable("RolePermission"));
            modelBuilder.Entity<Menu>()
                .HasMany(x => x.Permissions)
                .WithRequired(x => x.Menu);
            modelBuilder.Entity<Menu>()
                .HasOptional(x => x.ParentMenu)
                .WithMany(x => x.SubMenus)
                .HasForeignKey(x => x.ParentId);
        }
    }
}