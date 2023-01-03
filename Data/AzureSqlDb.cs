using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SaltOverFlowApi.Models;

    public class AzureSqlDb : DbContext
    {
        public AzureSqlDb (DbContextOptions<AzureSqlDb> options)
            : base(options)
        {
        }

        public DbSet<Comment> Comments { get; set; } = default!;

        public DbSet<Post> Posts { get; set; } = default!;

        public DbSet<User> Users { get; set; } = default!;
    }
