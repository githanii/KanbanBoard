using Domain.Entities;
using System.Collections.Generic;
using System.Security.Principal;

namespace TeamBoards.Domain.Entities;

public class Card 
{
    public int Id { get; set; }
    public int ListId { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }=DateTime.UtcNow;

    public int OrderIndex { get; set; }
    public int CreatedById { get; set; }

    public bool IsDeleted { get; set; }=false;

    public  List List { get; set; } = null!;

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

}
