using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TicketTypeSystem
{
    class BugTicketIO
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // public property
        public string filePath { get; set; }
        public List<BugTicket> BugTickets { get; set; }

        public BugTicketIO(string path)
        {
            BugTickets = new List<BugTicket>();
            filePath = path;
            // to populate the list with data, read from the data file
            try
            {
                StreamReader sr = new StreamReader(filePath);
                // first line contains column headers
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    // create instance of Ticket class
                    BugTicket bugTicket = new BugTicket();

                    //TODO parse out the individual tickets
                    string line = sr.ReadLine();

                    string[] ticketDetails = line.Split(',');
                   
                    //don't try to parse the header line
                    if (ticketDetails[0] != "Ticket ID")
                    {
                        bugTicket.TicketID = UInt64.Parse(ticketDetails[0]);
                        bugTicket.Summary = ticketDetails[1];
                        bugTicket.Status = ticketDetails[2];
                        bugTicket.Priority = ticketDetails[3];
                        bugTicket.Submitter = ticketDetails[4];
                        bugTicket.Assigned = ticketDetails[5];

                        //TODO deal with the possibly many watchers...
                        if (ticketDetails[6].Contains("|"))
                        {
                            bugTicket.Watching = ticketDetails[6].Replace("|", ", ").Split().ToList();
                        }
                        else
                        {
                            bugTicket.Watching = ticketDetails[6].Split().ToList();
                        }

                        bugTicket.Severity = ticketDetails[7];

                        BugTickets.Add(bugTicket);
                    }



                }
                // close file when done
                sr.Close();
                logger.Info("Bug tickets in file {Count}", BugTickets.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void PrintHeader()
        {
            //print header
            Console.WriteLine("{0,-10}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}{6,-50}{7, -20}\n", "ID", "Summary", "Status", "Priority", "Submitter", "Assigned To", "Watching", "Severity");
        }

        public void GetTicketDetails()
        {
            BugTicket newTicket = new BugTicket();
            string moreWatchers = "Y";

            Console.Write("Enter Ticket Summary: ");
            newTicket.Summary = Console.ReadLine();

            Console.Write("Enter Ticket Status: ");
            newTicket.Status = Console.ReadLine();

            Console.Write("Enter Ticket Priority: ");
            newTicket.Priority = Console.ReadLine();

            Console.Write("Ticket Submitted by: ");
            newTicket.Submitter = Console.ReadLine();

            Console.Write("Ticket Assigned To: ");
            newTicket.Assigned = Console.ReadLine();

            while (moreWatchers == "Y")
            {
                Console.Write("Enter Ticket Watcher: ");
                newTicket.Watching.Add(Console.ReadLine());

                Console.WriteLine("Enter another watcher (Y/N)?");
                moreWatchers = Console.ReadLine().ToUpper();
                                
            }

            Console.Write("Severity: ");
            newTicket.Severity = Console.ReadLine();

            AddTicket(newTicket);
        }

        //method adds ticket from list to file
        public void AddTicket(BugTicket bugTicket)
        {
            try
            {
                // first generate ticket id
                if (BugTickets.Count < 1)
                    bugTicket.TicketID = 1;
                else
                    bugTicket.TicketID = BugTickets.Last().TicketID + 1;
                                
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine($"{bugTicket.TicketID},{bugTicket.Summary},{bugTicket.Status},{bugTicket.Priority},{bugTicket.Submitter}, {bugTicket.Assigned},{string.Join("|", bugTicket.Watching)},{bugTicket.Severity}");
                sw.Close();
                // add ticket to List
                BugTickets.Add(bugTicket);
                // log transaction
                logger.Info("Bug Ticket id {Id} added", bugTicket.TicketID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}