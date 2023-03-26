using Microsoft.Azure.Management.DataFactory;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace adf_unit_testing
{
    public class datafactoryhelper
    {
        public string PipelineOutcome { get; private set; }


        public async Task RunPipeline(string pipelineName, IDictionary<string, object> _parameters)
        {
            PipelineOutcome = "Unknown";

            // authenticate against Azure
            var context = new AuthenticationContext("https://login.windows.net/" + "dfb4373b-3e2c-4e81-8443-b058054222a9");
            var cc = new ClientCredential("f827eff9-2d41-49f9-a28d-cde151cd0521", "vyA8Q~o~Nm7tTpKhr1YKky2~pgFz3ZFghUGBkbv7");
            var authResult = await context.AcquireTokenAsync("https://management.azure.com/", cc);

            // prepare ADF client
            var cred = new TokenCredentials(authResult.AccessToken);
            using (var adfClient = new DataFactoryManagementClient(cred) { SubscriptionId = "dabd9cc0-2774-4e8f-9655-84de111a8ffe" })
            {
                var adfName = "adfunittest";
                var rgName = "app-grp";

                // run pipeline
                var response = await adfClient.Pipelines.CreateRunWithHttpMessagesAsync(rgName, adfName, pipelineName,parameters: _parameters);
                string runId = response.Body.RunId;

                // wait for pipeline to finish
                var run = await adfClient.PipelineRuns.GetAsync(rgName, adfName, runId);
                while (run.Status == "Queued" || run.Status == "InProgress" || run.Status == "Canceling")
                {
                    Thread.Sleep(2000);
                    run = await adfClient.PipelineRuns.GetAsync(rgName, adfName, runId);
                }
                PipelineOutcome = run.Status;
            }
        }

        public int StagedRowCount (IDictionary<string, object> _parameters)
        {
            var tableName = String.Concat(_parameters["parm_schema"], ".", _parameters["parm_table"]);
            return new databasehelper().RowCount(tableName);
            
        }

        public datafactoryhelper()
        {
            PipelineOutcome = "Unknown";
        }
    }
}
