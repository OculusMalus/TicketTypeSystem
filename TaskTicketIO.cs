using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TicketTypeSystem
{
    class TaskTicketIO
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        // public property
        public string filePath { get; set; }
        public List<TaskTicket> TaskTickets { get; set; }

        public TaskTicketIO(string path)
        {
            TaskTickets = new List<TaskTicket>();
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
                    TaskTicket taskTicket = new TaskTicket();

                    //TODO parse out the individual tickets
                    string line = sr.ReadLine();

                    string[] ticketDetails = line.Split(',');

                    //don't try to parse the header line
                    if (ticketDetails[0] != "Ticket ID")
                    {
                        taskTicket.TicketID = UInt64.Parse(ticketDetails[0]);
                        taskTicket.Summary = ticketDetails[1];
                        taskTicket.Status = ticketDetails[2];
                        taskTicket.Priority = ticketDetails[3];
                        taskTicket.Submitter = ticketDetails[4];
                        taskTicket.Assigned = ticketDetails[5];

                        //TODO deal with the possibly many watchers...
                        if (ticketDetails[6].Contains("|"))
                        {
                            taskTicket.Watching = ticketDetails[6].Replace("|", ", ").Split().ToList();
                        }
                        else
                        {
                            taskTicket.Watching = ticketDetails[6].Split().ToList();
                        }

                        taskTicket.ProjectName = ticketDetails[7];
                        taskTicket.DueDate = ticketDetails[8];

                        TaskTickets.Add(taskTicket);
                    }



                }
                // close file when done
                sr.Close();
                logger.Info("Task tickets in file {Count}", TaskTickets.Count);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public void PrintHeader()
        {
            //print header
            Console.WriteLine("{0,-10}{1,-20}{2,-20}{3,-20}{4,-20}{5,-20}{6,-50}{7, -20}{8, -20}\n", "ID", "Summary", "Status", "Priority", "Submitter", "Assigned To", "Watching", "Project Name", "Due Date");
        }

        public void GetTicketDetails()
        {
            TaskTicket newTicket = new TaskTicket();
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

            Console.Write("Project Name: ");
            newTicket.ProjectName = Console.ReadLine();

            Console.Write("Due Date: ");
            newTicket.DueDate = Console.ReadLine();

            AddTicket(newTicket);
        }

        //method adds ticket from list to file
        public void AddTicket(TaskTicket taskTicket)
        {
            try
            {
                // first generate ticket id
                if (TaskTickets.Count < 1)
                    taskTicket.TicketID = 1;
                else
                    taskTicket.TicketID = TaskTickets.Last().TicketID + 1;
                
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine($"{taskTicket.TicketID},{taskTicket.Summary},{taskTicket.Status},{taskTicket.Priority},{taskTicket.Submitter}, {taskTicket.Assigned},{string.Join("|", taskTicket.Watching)},{taskTicket.ProjectName},{taskTicket.DueDate}");
                sw.Close();
                // add ticket to List
                TaskTickets.Add(taskTicket);
                // log transaction
                logger.Info("Task Ticket id {Id} added", taskTicket.TicketID);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
