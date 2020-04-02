using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommandControl_Web.Models
{
    public class Pending
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CommandId { get; set; }
        public DateTime QueuedDT { get; set; }
        public DateTime SentDT { get; set; }
    }
}