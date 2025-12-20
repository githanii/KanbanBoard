using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ListDto
    {
        public int Id { get; set; }
        public int BoardId { get; set; }
        public string Title { get; set; } = default!;
        public int OrderIndex { get; set; }
        public List<CardDto> Cards { get; set; } = new();


    }
}
