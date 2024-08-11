

namespace Domain.Models.Entities
{
    public class Card
    {
        public string Number { get; set; }
        public short CVV {  get; set; }
        public DateOnly Valid { get; set; }
        public string Holder { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
