using SPU_7.Common.Stand;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models
{
    public class DbUsers : DbEntityGuid
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Employee { get; set; }

        public UserType UserType { get; set; }
        
        public ProgramType ProgramType { get; set; }

        public bool IsRememberUser { get; set; }
    }
}
