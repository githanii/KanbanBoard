using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BoardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int OwnerUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ListDto> Lists { get; set; } = new();


    
    }
}
