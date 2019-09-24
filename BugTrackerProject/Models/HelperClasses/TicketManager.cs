using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrackerProject.Models.HelperClasses
{
    public class TicketManager
    {
        ApplicationDbContext db;

        public TicketManager(ApplicationDbContext db)
        {
            this.db = db;
        }
        //get all the ticktes
        public List<Ticket> GetAllTickets()
        {
            var allTickets = db.Tickets.ToList();
            return allTickets;
        }
    }
}