using Domain.Models.Entities.SQLEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.VievModels
{
    public class CardModel
    {
        public CardModel(Card card)
        {
            Number = card.Number;
            CVV = card.CVV;
            Valid = card.Valid;
            Holder = card.Holder;
        }
        public string Number { get; set; }
        public short CVV { get; set; }
        public DateOnly Valid { get; set; }
        public string Holder { get; set; }
        public Card ToCard(Guid userId)
        {
            return new Card() { Number =  Number, CVV = CVV, Holder = Holder, Valid = Valid, UserId = userId };
        }
    }
}
