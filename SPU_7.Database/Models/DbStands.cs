using SPU_7.Common.Stand;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models
{
    public class DbStands : DbEntityGuid
    {
        /// <summary>
        /// Тип стенда
        /// </summary>
        public StandType StandType { get; set; }

        /// <summary>
        /// Название стенда
        /// </summary>
        public string StandName { get; set; }

        /// <summary>
        /// Номер стенда
        /// </summary>
        public string StandNumber { get; set; }
    }
}
