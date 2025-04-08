using TMFDomain.Shared;

namespace TMFDomain.Entities.Staff
{
    public class Role : BaseEntity
    {
        public string? role_id { get; set; } = string.Empty;
        public string name { get; set; }
        public string description { get; set; }
        public List<Privilege> Privileges { get; set; } = new List<Privilege>();
    }
}