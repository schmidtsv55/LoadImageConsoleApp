using LoadImageConsoleApp.Services.AutoDisassembly.Models;
using Microsoft.EntityFrameworkCore;

namespace LoadImageConsoleApp.Services.AutoDisassembly;

public class AutoDisassemblyContext : DbContext
{
    public AutoDisassemblyContext(DbContextOptions<AutoDisassemblyContext> options)
            : base(options)
    {
    }
    public DbSet<Sys_File> Sys_Files => Set<Sys_File>();
}
