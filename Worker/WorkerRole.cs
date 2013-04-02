using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;

namespace Worker
{
    public class WorkerRole : RoleEntryPoint
    {
        private const int MaxInterval = 15;
        private DataSet _data = new DataSet();

        public override void Run()
        {
            // This is a sample worker implementation. Replace with your logic.
            Trace.WriteLine("Worker entry point called", "Information");

            int currentInterval = 0;

            while (true)
            {
                try
                {
                    //Get Data to work with
                    _data = GetData();
                    if (_data == null)
                    {
                        Trace.WriteLine("There was no resultset; raise interval");

                        if (currentInterval < MaxInterval)
                        {
                            currentInterval++;
                        }
                        Trace.TraceInformation("Waiting for {0} seconds", currentInterval);
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(currentInterval));
                    }
                    else
                    {
                        currentInterval = 0;
                        Trace.WriteLine("There was a resultset; reset interval");

                        try
                        {
                            //TODO: do some real work with the _data
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("There was no an error while processing");
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
        }

        private DataSet GetData()
        {
            return null;
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}
