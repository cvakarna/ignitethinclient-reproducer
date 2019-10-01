using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteConnectionTest
{
    class Program
    {

        MessageReceived mr = new MessageReceived();
        static void Main(string[] args)
        {
            Program pro = new Program();
           
            Task.Run(() =>
            {
                pro.PublishMessages();
                pro.GetData();
            });
           
           Thread.Sleep(Timeout.Infinite);
        }

        public void PublishMessages()   //External Application sending data to particulaer service with different instances ,each request serve cuncurrenlty 
        {
            for(int i = 0; i < 200; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    mr.publishMessages();
                });
            }
        }


        public void GetData() //frontend clients make get request to other services
        {
            for (int i = 0; i < 100; i++)
            {
                mr.ReadData();
            }
        }
    }

   
}
