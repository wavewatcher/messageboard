using messageboard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace messageboard.Data;

public class MyDbContext : ApiAuthorizationDbContext<ApplicationUser> {
    public MyDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=Messageboard.db");

    public DbSet<Message>? Message { get; set; }

    public DbSet<User>? User { get; set; }

    public DbSet<Like>? Like { get; set; }
}