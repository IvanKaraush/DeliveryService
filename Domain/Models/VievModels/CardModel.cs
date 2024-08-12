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
        public string Number { get; set; }
        public short CVV { get; set; }
        public DateOnly Valid { get; set; }
        public string Holder { get; set; }
    }
}
