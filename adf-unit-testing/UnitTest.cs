using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace adf_unit_testing 
{
    public class Tests : datafactoryhelper
    {
        private datafactoryhelper _helper;
        public IDictionary<string, object> _parameters;

        [OneTimeSetUp]
        public async Task WhenPipelineIsRun()
        {
            _helper = new datafactoryhelper();
            await _helper.RunPipeline(_parameters["pl_name"].ToString(),_parameters);
        }

        [Test]
        public void ThenPipelineOutcomeIsSucceeded()
        {
            _helper.PipelineOutcome.Should().Be("Succeeded");
        }

        [Test]
        public void CheckTargetCount()
        {
            _helper.StagedRowCount(_parameters).Should().Be(10);
        }

        public Tests()
        {
            _parameters = new Dictionary<string, object>();
            _parameters.Add("parm_schema", "SalesLT");
            _parameters.Add("parm_table", "SalesOrderDetail");
            _parameters.Add("pl_name", "pl_unit_testing");
        }
    }
}