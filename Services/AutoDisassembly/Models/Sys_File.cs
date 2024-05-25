using System.ComponentModel.DataAnnotations;

namespace LoadImageConsoleApp.Services.AutoDisassembly.Models;

public class Sys_File
{
    [Key]
    public int FILE_ID { get; set; }
    public int? ITEM_ID { get; set; }
    public string? FORM_NAME { get; set; }
    public string? FILE_NAME { get; set; }
    public string? FILE_DATA { get; set; }
    public string? FILE_LINK { get; set; }
}
