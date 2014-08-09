using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using SmsIn.Models;

namespace SmsIn.Controllers
{
    public class SmsController : Controller
    {
        //
        // GET: /Sms/
        public ActionResult Index(int page = 1, int sortBy = 1, bool isAsc = true)
        {
            SmsGrid model = SmsData.GetMessages(page, sortBy, isAsc);
            return View(model);
        }

        //
        // POST: /Sms/Purge
        [HttpPost]
        public ActionResult Purge()
        {
            SmsData.Purge();
            SmsGrid model = new SmsGrid();
            return View("Index",model);
        }


        //
        // POST: /Sms/ExetelVirtualMobileReceiver
        //  ?msgid=1&receivedat=2009-05-07 13:00:00&sender=0431112222 &vmn=0421114444 &message=Hello
        [HttpPost]
        public ActionResult ExetelVirtualMobileReceiver(string msgid, string receivedat, string sender, string vmn, string message)
        {
            SmsClass sms = new SmsClass();
            sms.ReceiverMobile = vmn;
            sms.SenderMobile = sender;
            sms.Message = message;
            sms.Received = DateTime.Now;
            SmsData.SaveSMS(sms);

            return View("SmsSaved");
        }

        // GET: /Sms/VirtualSmsGlobal
        // GET: /http-api.php
        public ActionResult VirtualSmsGlobal(string action, string user, string password, string from, string to, string text, string maxsplit, string scheduledatetime)
        {
            SmsClass sms = new SmsClass();
            sms.ReceiverMobile = to;
            sms.SenderMobile = from;
            sms.Message = text;
            sms.Received = DateTime.Now;
            SmsData.SaveSMS(sms);
            
            return View("SmsSaved");
        }

        // GET: /Sms/VirtualExetel
        // GET: /api_sms.php

        //EXETEL  ,A*     ,'https://exetel.com.au/sendsms/api_sms.php?username=lynx01&password=onimod'
        //lynx01&password=onimod&sender=0429651179&mobilenumber=0409651179&
        //message=Test this sms&messagetype=Text&referencenumber=12349'

        public ActionResult VirtualExetel(string username, string password, string sender, string mobilenumber,
            string message, string messagetype, string referencenumber)
        {
            SmsClass sms = new SmsClass();
            sms.ReceiverMobile = mobilenumber;
            sms.SenderMobile = sender;
            sms.Message = message;
            sms.Received = DateTime.Now;
            SmsData.SaveSMS(sms);

            return View("SmsSaved");
        }


        //
        // POST: /Sms/SmsSync
        [HttpPost]
        public ActionResult SmsSync(string from, string send_to, string message)
        {
            SmsClass sms = new SmsClass();
            sms.ReceiverMobile = send_to;
            sms.SenderMobile = from;
            sms.Message = message;
            sms.Received = DateTime.Now;
            SmsData.SaveSMS(sms);

            return View("SmsSaved");
        }
	}
}