using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleedinVampires.Control
{
    //Test Github Commit Change
    class MessgingSystem
    {
        List<string> MessagingOutbox;
        List<string> MessagingInbox;

        public MessgingSystem()
        {
            MessagingInbox = new List<string>();
            MessagingOutbox = new List<string>();
        }

        //Add an outgoing message
        public void AddMessage(string s)
        {
            MessagingOutbox.Add(s);
        }

        //Get a copy of the outbox
        public List<string> RetrieveOutbox()
        {
            List<string> toSend;
            toSend = MessagingOutbox;
            return toSend;
        }

        //Get a copy of the inbox
        public List<string> RetrieveInbox()
        {
            List<string> toSend;
            toSend = MessagingInbox;
            return toSend;
        }

        //Empty the inbox
        public bool ClearInbox()
        {
            try
            {
                MessagingInbox.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Empty the outbox
        public bool ClearOutbox()
        {
            try
            {
                MessagingOutbox.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
