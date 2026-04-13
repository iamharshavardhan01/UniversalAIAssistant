using System.ComponentModel.DataAnnotations.Schema;

namespace DigiSoft.Database.Entities.CommonFields
{
    public class IdAndDatesWithIsActive : IdWithDateColumns
    {
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}
