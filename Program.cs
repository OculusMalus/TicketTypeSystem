using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TicketTypeSystem
{
    class Program
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        static void Main(string[] args)
        {
            // config is loaded using xml (NLog.config saved in debug folder)
            logger.Info("Program started");
            
            //path to bug ticket file
            string bugTicketsPath = "Tickets.csv";
            //path to enhancement ticket file
            string enhancementsTicketPath = "Enhancements.csv";
            // path to task tickets file;
            string taskTicketsPath = "Task.csv";

            //create instance of File classes
            BugTicketIO bugTicketIO = new BugTicketIO(bugTicketsPath);
            EnhancementsTickeIO enhancementsTicketIO = new EnhancementsTickeIO(enhancementsTicketPath);
            TaskTicketIO taskTicketIO = new TaskTicketIO(taskTicketsPath);

            string choice = "";
            string file = "";
            do
            {
                Console.WriteLine("1) Display ticket data from file?");
                Console.WriteLine("2) Add new ticket to file?");
                Console.WriteLine("3) Search tickets?");
                Console.WriteLine("Enter any other key to exit.");
               
                choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.WriteLine("1) Display bug tickets.");
                    Console.WriteLine("2) Display enhancement tickets.");
                    Console.WriteLine("3) Display task tickets.");
                    Console.WriteLine("Enter any other key to exit.");
                    var displayChoice = Console.ReadLine();

                    if (displayChoice == "1")
                    {
                        file = bugTicketsPath;
                        if (File.Exists(file))
                        {
                            foreach (BugTicket m in bugTicketIO.BugTickets)
                                Console.WriteLine(m.Display());
                        }
                    }
                    if (displayChoice == "2")
                    {
                        file = enhancementsTicketPath;
                        if (File.Exists(file))
                        {
                            foreach (EnhancementsTicket m in enhancementsTicketIO.EnhancementsTickets)
                                Console.WriteLine(m.Display());
                        }
                    }
                    if (displayChoice == "3")
                    {
                        file = taskTicketsPath;
                        if (File.Exists(file))
                        {
                            foreach (TaskTicket m in taskTicketIO.TaskTickets)
                                Console.WriteLine(m.Display());
                        }                      
                    }

                }
                if (choice == "2")
                {
                    Console.WriteLine("1) Add bug ticket?");
                    Console.WriteLine("2) Add enhancement ticket?");
                    Console.WriteLine("3) Add task tickets?");
                    Console.WriteLine("Enter any other key to exit.");
                    var ticketChoice = Console.ReadLine();

                    if (ticketChoice == "1")
                        bugTicketIO.GetTicketDetails();

                    if (ticketChoice == "2")
                        enhancementsTicketIO.GetTicketDetails();

                    if (ticketChoice == "3")
                        taskTicketIO.GetTicketDetails();

                }

                if (choice == "3")
                {
                    Console.WriteLine("1) Search by status?");
                    Console.WriteLine("2) Search by priority?");
                    Console.WriteLine("3) Search by submitter?");
                    Console.WriteLine("Enter any other key to exit.");
                    var searchChoice = Console.ReadLine();

                    Console.WriteLine("Enter search term: ");
                    var term = Console.ReadLine().ToLower();

                    IEnumerable<BugTicket> list1 = new List<BugTicket>();
                    IEnumerable<EnhancementsTicket> list2 = new List<EnhancementsTicket>();
                    IEnumerable<TaskTicket> list3 = new List<TaskTicket>();

                    if (searchChoice == "1")
                    {
                        list1 = bugTicketIO.BugTickets.Where(m => m.Status.ToLower().Contains(term));
                        list2 =enhancementsTicketIO.EnhancementsTickets.AsParallel().Where(m => m.Status.ToLower().Contains(term));
                        list3 = taskTicketIO.TaskTickets.Where(m => m.Status.ToLower().Contains(term));  
                    }

                    if (searchChoice == "2")
                    {
                        list1 = bugTicketIO.BugTickets.AsParallel().Where(m => m.Priority.ToLower().Contains(term));
                        list2 = enhancementsTicketIO.EnhancementsTickets.AsParallel().Where(m => m.Priority.ToLower().Contains(term));
                        list3 = taskTicketIO.TaskTickets.Where(m => m.Priority.ToLower().Contains(term));
                    }

                    if (searchChoice == "3")
                    {
                        list1 = bugTicketIO.BugTickets.AsParallel().Where(m => m.Submitter.ToLower().Contains(term));
                        list2 = enhancementsTicketIO.EnhancementsTickets.AsParallel().Where(m => m.Submitter.ToLower().Contains(term));
                        list3 = taskTicketIO.TaskTickets.Where(m => m.Submitter.ToLower().Contains(term));                        
                    }

                    var result = list1.Concat(list2.Cast<Ticket>()).Concat(list3.Cast<Ticket>()).ToList();  
                    foreach (Ticket item in result)
                    {
                        Console.WriteLine(item.Display());
                    }
                }

            } while (choice == "1" || choice == "2" || choice == "3");
        }
    }
}
