using System;
using System.Collections.Generic;
using System.Text;

namespace IgniteConnectionTest
{
    public class MessageReceived
    {

        public void publishMessages()
        {
            ConnectToTheQaServer cs = new ConnectToTheQaServer();

            cs.ConnetToServer("6e7b08b5-3bfd-4026-b322-c4a34ffkafd7");
        }


        public void ReadData()
        {
            InternalAppService cs = new InternalAppService();
            cs.ReadData("6e7b08b5-3bfd-4026-b322-c4a34ffkafd7_Forms");
        }
    }
}
