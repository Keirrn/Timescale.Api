using Microsoft.EntityFrameworkCore;
using Timecale.Domain.Entities;
namespace Timecale.Infrastructure.Data;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UploadedFile> UploadedFiles => Set<UploadedFile>();

    public DbSet<Value> Values => Set<Value>();

    public DbSet<Result> Results => Set<Result>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UploadedFile>()
            .HasMany(x => x.Values)
            .WithOne(x => x.File)
            .HasForeignKey(x => x.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UploadedFile>()
            .HasOne(x => x.Result)
            .WithOne(x => x.File)
            .HasForeignKey<Result>(x => x.FileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
