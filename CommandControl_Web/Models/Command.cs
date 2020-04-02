using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommandControl_Web.Models
{
        public class Command
        {
            public int Id { get; set; }
            public string scriptType { get; set; }
            public string scriptName { get; set; }
            public string scriptData { get; set; }
            public string scriptParameter { get; set; }
            public byte[] data { get; set; }
        }

}