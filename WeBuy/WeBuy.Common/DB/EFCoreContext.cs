using Microsoft.EntityFrameworkCore;
using WeBuy.Model.System;
using WeBuy.Model.User;

namespace WeBuy.Common
{
    /// <summary>
    /// efcore 上下文
    /// </summary>
    public class EFCoreContext: DbContext
    {
        public EFCoreContext(DbContextOptions<EFCoreContext> options): base(options)
        {
        }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<SystemLog> SystemLog { get; set; }
        public DbSet<MenuInfo> MenuInfo { get; set; }
        public DbSet<RoleInfo> RoleInfo { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Student>().ToTable("Student");
        //}
    }
}
