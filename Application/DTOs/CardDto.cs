using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CardDto
    {
        public int Id { get; set; }
        public int ListId { get; set; }
        public string Title { get; set; }= default!;
        public string? Description { get; set; }

        public int OrderIndex { get; set; } 
        public bool IsDeleted { get; set; }=false;
    }
}
