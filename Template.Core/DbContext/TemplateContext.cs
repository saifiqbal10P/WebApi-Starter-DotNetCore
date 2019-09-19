using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Template.Core.Entity;

namespace Template.Core.DbContext
{
    public class TemplateContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public TemplateContext(DbContextOptions options)
       : base(options)
        {

        }
        public DbSet<TestTable> TestTable { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.EntityRelationships(builder);
        }

        private void EntityRelationships(ModelBuilder builder) // Apply Relationships
        {
           // builder.ApplyConfiguration(new ApplicationUserConfig());
        }
    }
}
