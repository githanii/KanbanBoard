using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamBoards.Domain.Entities;

namespace Domain.Entities
{
    public class List
    {
        public int Id { get; set; } 
        public string Title { get; set; }= string.Empty;
        public int OrderIndex { get; set; }
        public int BoardId { get; set; }
        public Board Board { get; set; }= null!;
        public List<Card> Cards { get; set; } = new();
    }
}
