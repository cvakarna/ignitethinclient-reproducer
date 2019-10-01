using IgniteConnectionTest.ignite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IgniteConnectionTest
{
    public class ConnectToTheQaServer
    {
        readonly IgniteClientService _igniteClientService;
        public ConnectToTheQaServer()
        {
            this._igniteClientService = new IgniteClientService();
        }


        public async Task ConnetToServer(string tenantId)
        {
            Console.WriteLine("ConnectoServer Method:::::");
            for(int i = 0; i < 10; i++)
            {
                Console.WriteLine("ConnectoServer Method:::"+"||:::"+i);
                string query = "select Name,Id from Persons";
                await this._igniteClientService.QueryAsync(query, tenantId);
                Thread.Sleep(5000);
            }
        }


    }

}
