using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigiSoft.Database.Entities.CommonFields
{
    public class PrimaryKey
    {
        [Key, Column("id")]
        public long Id { get; set; }
    }
}
