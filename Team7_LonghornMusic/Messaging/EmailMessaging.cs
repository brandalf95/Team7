﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Team7_LonghornMusic.Messaging
{
    public class EmailMessaging
    {
        public static void SendEmail(String toEmailAddress, String emailSubject, String emailBody)
        {

            //Create an email client to send the emails
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("team7lhm@gmail.com", "team7pass"),
                EnableSsl = true
            };

            //Add anything that you need to the body of the message
            // /n is a new line – this will add some white space after the main body of the message
            String finalMessage = emailBody + "\n\n Longhorn Music - 2016 - Team 7";

            //Create an email address object for the sender address
            MailAddress senderEmail = new MailAddress("team7lhm@gmail.com", "Longhorn Music");


            MailMessage mm = new MailMessage();
            mm.IsBodyHtml = true;
            mm.Subject = emailSubject;
            mm.Sender = senderEmail;
            mm.From = senderEmail;
            mm.To.Add(new MailAddress(toEmailAddress));
            mm.Body = finalMessage;
            client.Send(mm);
        }

    }
}
