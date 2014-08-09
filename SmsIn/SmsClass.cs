using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmsIn
{
    public class SmsClass
    {
        public int Id { get; set; }
        public string SenderMobile { get; set; }
        public string ReceiverMobile { get; set; }
        public DateTime Received { get; set; }
        public string Message { get; set; }
     }
}