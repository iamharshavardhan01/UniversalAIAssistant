using System.ComponentModel.DataAnnotations.Schema;

namespace DigiSoft.Database.Entities.CommonFields
{
    public class IdAndDatesWithIsActiveAndIsDeleted : IdAndDatesWithIsActive
    {
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
