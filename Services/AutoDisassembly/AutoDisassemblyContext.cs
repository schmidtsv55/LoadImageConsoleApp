﻿using LoadImageConsoleApp.Services.AutoDisassembly.Models;
using Microsoft.EntityFrameworkCore;

namespace LoadImageConsoleApp.Services.AutoDisassembly;

public class AutoDisassemblyContext : DbContext
{
    public AutoDisassemblyContext(DbContextOptions<AutoDisassemblyContext> options)
            : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sys_File>()
            .ToTable(tb => tb.HasTrigger("Sys_Files_UPDATE"));
    }
    public DbSet<Sys_File> Sys_Files => Set<Sys_File>();
}
