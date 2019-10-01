using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IgniteConnectionTest.ignite
{
    public class IgniteClientService
    {
        private readonly IgniteClientConfiguration _igniteClientConfiguration;
        Boolean _inProgress = false; //to check connection close or not
        Timer _timer; //connection timed out
        IIgniteClient _client = null; //thin client 
        TelemetryClient tc = new TelemetryClient();
        public IgniteClientService()
        {
            _igniteClientConfiguration = new IgniteClientConfiguration
            {
                Endpoints = new string[] { "localhost" },//it is qa endpoint
                SocketTimeout = TimeSpan.FromSeconds(60)
            };
        }


        public async Task QueryAsync(string query,string cacheName)
        {
            try
            {
                var sqlquery = new SqlFieldsQuery(query);
                var client = GetIgnite("QueryAsync");
                var cache = client.GetCache<object, object>(cacheName);
                var res = cache.Query(sqlquery).GetAll();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }



        private IIgniteClient GetIgnite(string methodName)
        {
            if (this._timer == null)
            {
                Set_Schedule_Timer(1);
            }
            if (this._client==null)
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var startTime = DateTime.Now;
                //Establish Connection To The Ignite Server
                this._client = Ignition.StartClient(this._igniteClientConfiguration);
                watch.Stop();
                Console.WriteLine("Ignite Start Client {0}, {1}, {2}, {3} ----{4}", startTime, watch.Elapsed, "200", true, methodName);
                //tc.TrackRequest("Ignite Start Client", startTime, watch.Elapsed, "200", true);
            }
            return this._client;
        }



        void Set_Schedule_Timer(int minutes)
        {
            DateTime timeNow = DateTime.Now;
            DateTime scheduledTime = DateTime.Now.AddMinutes(2 * minutes);
            double totalTime = (scheduledTime - timeNow).TotalMilliseconds;
            this._timer = new System.Timers.Timer(totalTime);
            this._timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            this._timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this._timer.Stop();
            if (this._inProgress == false)
            {
                Close();
                this._timer = null;
            }
            else
            {
                Set_Schedule_Timer(3);
            }
        }

        private void Close()
        {
            if (this._client != null)
            {
                this._client.Dispose();
                this._client = null;
            }

            if (this._timer != null)
            {
                this._timer.Stop();
                this._timer = null;
            }
        }


    }
}
