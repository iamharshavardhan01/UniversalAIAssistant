using System.ComponentModel.DataAnnotations.Schema;

namespace DigiSoft.Database.Entities.CommonFields
{
    public class IdAndDatesWithIsDeleted : IdWithDateColumns
    {
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
