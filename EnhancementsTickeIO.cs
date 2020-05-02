using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TicketTypeSystem
{
    class EnhancementsTickeIO
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // public property
        public string filePath { get; set; }
        public List<EnhancementsTicket> EnhancementsTickets { get; set; }

        public EnhancementsTickeIO(string path)
        {
            EnhancementsTickets = new List<EnhancementsTicket>();
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
                    EnhancementsTicket enhancementsTicket = new EnhancementsTicket();

                    //TODO parse out the individual tickets
                    string line = sr.ReadLine();

                    string[] ticketDetails = line.Split(',');

                    //don't try to parse the header line
                    if (ticketDetails[0] != "Ticket ID")
                    {
                        enhancementsTicket.TicketID = UInt64.Parse(ticketDetails[0]);
                        enhancementsTicket.Summary = ticketDetails[1];
                        enhancementsTicket.Status = ticketDetails[2];
                        enhancementsTicket.Priority = ticketDetails[3];
                        enhancementsTicket.Submitter = ticketDetails[4];
                        enhancementsTicket.Assigned = ticketDetails[5];

                        //TODO deal with the possibly many watchers...
                        if (ticketDetails[6].Contains("|"))
                        {
                            enhancementsTicket.Watching = ticketDetails[6].Replace("|", ", ").Split().ToList();
                        }
                        else
                        {
                            enhancementsTicket.Watching = ticketDetails[6].Split().ToList();
                        }

                        enhancementsTicket.Software = ticketDetails[7];
                        enhancementsTicket.Cost = ticketDetails[8];
                        enhancementsTicket.Reason = ticketDetails[9];
                        enhancementsTicket.Estimate = ticketDetails[10];


                        EnhancementsTickets.Add(enhancementsTicket);
                    }



                }
                // close file when done
                sr.Close();
                logger.Info("Enhancements tickets in file {Count}", EnhancementsTickets.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

        }

        public void PrintHeader()
        {
            //print header
            Console.WriteLine("{0,-10}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}{6,-50}{7, -20}{8, -20}{9, -20}\n", "ID", "Summary", "Status", "Priority", "Submitter", "Assigned To", "Watching", "Software", "Cost", "Reason");
        }

        public void GetTicketDetails()
        {
            EnhancementsTicket newTicket = new EnhancementsTicket();
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

            Console.Write("Software: ");
            newTicket.Software = Console.ReadLine();

            Console.Write("Cost: ");
            newTicket.Cost = Console.ReadLine();

            Console.Write("Reason: ");
            newTicket.Reason = Console.ReadLine();

            Console.Write("Estimate: ");
            newTicket.Estimate = Console.ReadLine();

            AddTicket(newTicket);
        }

        //method adds ticket from list to file
        public void AddTicket(EnhancementsTicket enhancementsTicket)
        {
            try
            {
                // first generate ticket id
                if (EnhancementsTickets.Count < 1)
                    enhancementsTicket.TicketID = 1;
                else
                    enhancementsTicket.TicketID = EnhancementsTickets.Last().TicketID + 1;

                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine($"{enhancementsTicket.TicketID},{enhancementsTicket.Summary},{enhancementsTicket.Status},{enhancementsTicket.Priority},{enhancementsTicket.Submitter}, {enhancementsTicket.Assigned},{string.Join("|", enhancementsTicket.Watching)},{enhancementsTicket.Software},{enhancementsTicket.Cost}{enhancementsTicket.Reason}");
                sw.Close();
                // add ticket to List
                EnhancementsTickets.Add(enhancementsTicket);
                // log transaction
                logger.Info("Bug Ticket id {Id} added", enhancementsTicket.TicketID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
