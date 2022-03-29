using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Data
{
    public class Add
    {
        public int Number { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public int LoginId { get; set; }
        public string Email { get; set; }
        public bool BelongsToUser { get; set; }
        public int id { get; set; }
    }
}
