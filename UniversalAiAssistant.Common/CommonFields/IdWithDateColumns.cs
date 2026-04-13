using System.ComponentModel.DataAnnotations.Schema;

namespace DigiSoft.Database.Entities.CommonFields
{
    public class IdWithDateColumns : PrimaryKey
    {
        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Column("updated_date")]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    }
}
