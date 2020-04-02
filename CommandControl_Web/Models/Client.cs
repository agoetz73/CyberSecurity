using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommandControl_Web.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public DateTime LastAccessed { get; set; }
        public DateTime FirstSeen { get; set; }
    }
}