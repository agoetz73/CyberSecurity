using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommandControl_Web.Models
{
    public class Execution
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int CommandId { get; set; }
        public string Result { get; set; }
        public DateTime SaveTime { get; set; }
    }
}