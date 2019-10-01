using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace IgniteConnectionTest
{
    public class InternalAppService
    {
        public void ReadData(string cacheName)
        {
            Console.WriteLine("ReadData:::");
             var _igniteClientConfiguration = new IgniteClientConfiguration
             {
                Endpoints = new string[] { "localhost" },
                SocketTimeout = TimeSpan.FromSeconds(60)
             };

            using (var client = Ignition.StartClient(_igniteClientConfiguration)) //in other services simply open connection and closing 
            {
                try
                {
                    var cache = client.GetCache<object, object>(cacheName);
                    string query = "select Name from Forms";
                    var sqlFieldsQuery = new SqlFieldsQuery(query);
                    var result = cache.Query(sqlFieldsQuery).GetAll();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            }
        }
    }
}
