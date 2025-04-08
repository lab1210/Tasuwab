using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Staff
{

    public class Position : BaseEntity
    {
        public int position_id { get; set; } 
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty; 
    }

}
