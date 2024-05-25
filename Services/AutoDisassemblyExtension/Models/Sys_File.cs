using System.ComponentModel.DataAnnotations;

namespace LoadImageConsoleApp.Services.AutoDisassemblyExtension.Models;

public class Sys_File
{
    [Key]
    public int FILE_ID { get; set; }
    public string? Link { get; set; }
    public string? FILE_NAME { get; set; }
}
