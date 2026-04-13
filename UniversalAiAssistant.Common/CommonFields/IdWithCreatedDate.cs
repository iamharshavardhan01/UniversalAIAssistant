using System.ComponentModel.DataAnnotations.Schema;

namespace DigiSoft.Database.Entities.CommonFields
{
    public class IdWithCreatedDate : PrimaryKey
    {
        [Column("created_date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
