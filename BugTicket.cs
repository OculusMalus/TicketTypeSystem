using System;
using System.Collections.Generic;
using System.Text;

namespace TicketTypeSystem
{
    public class BugTicket : Ticket
    {
        public string Severity { get; set; }

        
        public override string Display()
        {
            return $"{base.Display()}Severity: {Severity}\n";
        }
    }
}
