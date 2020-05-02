using System;
using System.Collections.Generic;
using System.Text;

namespace TicketTypeSystem
{
    public class Ticket
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        // public properties
        public UInt64 TicketID { get; set; }
        public string Summary { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string Submitter { get; set; }
        public string Assigned { get; set; }
        public List<string> Watching { get; set; }

        public Ticket()
        {
            Watching = new List<string>();
        }

        // public method
        public virtual string Display()
        {
            return $"Id: {TicketID}\nSummary: {Summary}\nStatus: {Status}\nPriority: {Priority}\nSubmitter: {Submitter}\nAssigned: {Assigned}\nWatching: {string.Join(", ", Watching)}\n";
        }
    }
}


