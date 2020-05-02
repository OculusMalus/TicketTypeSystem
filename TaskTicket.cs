using System;
using System.Collections.Generic;
using System.Text;

namespace TicketTypeSystem
{
    class TaskTicket : Ticket
    {
        public string ProjectName { get; set; }
        public string DueDate { get; set; }

        public override string Display()
        {
            return $"{base.Display()}Project Name: {ProjectName}\nDue Date: {DueDate}\n";
        }
    }
}
