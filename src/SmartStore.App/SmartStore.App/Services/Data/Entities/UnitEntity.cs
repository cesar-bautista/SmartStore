using SQLite;

namespace SmartStore.App.Services.Data.Entities
{
    [Table("Units")]
    public class UnitEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}