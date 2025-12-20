using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Board

    {
        public int Id { get; set; }
        public string Name { get; set; }= string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int OwnerId { get; set; }

        public List< List> Lists{ get; set; } = new();
    }
}
