using LoadImageConsoleApp.Services.AutoDisassemblyExtension.Models;
using Microsoft.EntityFrameworkCore;

namespace LoadImageConsoleApp.Services.AutoDisassemblyExtension;

public class AutoDisassemblyExtensionContext: DbContext
{
    public AutoDisassemblyExtensionContext(DbContextOptions<AutoDisassemblyExtensionContext> options)
            : base(options)
    {
    }
public DbSet<Sys_File> Sys_Files => Set<Sys_File>();
}
