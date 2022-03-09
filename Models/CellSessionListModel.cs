using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScheduleAutomation.Models
{
    public class CellSessionListModel
    {
        public Guid empID { get; set; }
        public Guid sessionID { get; set; }
        public string sessionName { get; set; }
        public string sessionCode { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public DateTime sessionCreationDate { get; set; }

    }
}