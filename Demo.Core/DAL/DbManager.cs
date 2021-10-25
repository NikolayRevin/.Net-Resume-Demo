using System;
using Demo.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Core.DAL
{
    public class DbManager: DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserVoteEntity> UserVotes { get; set; }
        public DbSet<DeviceEntity> Devices { get; set; }

        public DbManager(DbContextOptions<DbManager> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<UserEntity>()
                .HasOne(u => u.Vote)
                .WithOne(v => v.User)
                .HasForeignKey<UserVoteEntity>(v => v.UserId);

            modelBuilder
                .Entity<UserEntity>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(t => t.User)
                .HasForeignKey(u => u.UserId);
        }
    }
}
