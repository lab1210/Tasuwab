using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using TMFDomain.Shared;

namespace TMFDomain.Entities.Staff
{
    public class Branch : BaseEntity
    {
        public int branch_id { get; set; }
        public string name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string phone { get; set; } = string.Empty;
        public bool is_active { get; set; } = true;
        public bool is_visible { get; set; } = true;
    }
}
