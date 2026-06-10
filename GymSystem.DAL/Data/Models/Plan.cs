namespace GymSystem.DAL.Data.Models
{
    public class Plan : BaseEntity
    {

        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }   // soft-delete flag


        #region Relation


        public ICollection<MemberShip> MemberShips { get; set; } = default!;

        #endregion


    }
}
