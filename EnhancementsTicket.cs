using System;
using System.Collections.Generic;
using System.Text;

namespace TicketTypeSystem
{
    class EnhancementsTicket : Ticket
    {
        public string Software { get; set; }
        public string Cost { get; set; }
        public string Reason { get; set; }
        public string Estimate { get; set; }

        public override string Display()
        {
            return $"{base.Display()}Software: {Software}\nCost: {Cost}\nReason: {Reason}\nEstimate: {Estimate}\n";
        }
        
    }
}
