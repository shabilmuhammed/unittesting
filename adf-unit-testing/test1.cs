using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace adf_unit_testing 
{
    public class test1 : datafactoryhelper
    {
        private datafactoryhelper _helper;
        public IDictionary<string, object> _parameters;
        public TestContext TestContext { get; set; }

        [OneTimeSetUp]
        public async Task WhenPipelineIsRun()
        {
            if (_parameters["pl_name"].ToString() != "pl_new_unit_test")
            {
                Assert.Ignore("unmatched pipeline");
            }
            else
            {
                _helper = new datafactoryhelper();
                await _helper.RunPipeline(_parameters["pl_name"].ToString(), _parameters);
            }
        }

        [Test]
        public void ThenPipelineOutcomeIsSucceeded()
        {
            _helper.PipelineOutcome.Should().Be("Succeeded");
        }

        [Test]
        public void CheckTargetCount()
        {
            IDictionary<string, object> src_params = new Dictionary<string, object>();
            src_params.Add("parm_schema", _parameters["parm_source_schema"]);
            src_params.Add("parm_table", _parameters["parm_table"]);
            var tgt_count = _helper.StagedRowCount(src_params);
            _helper.StagedRowCount(_parameters).Should().Be(tgt_count);
        }

        public test1()
        {
            _parameters = new Dictionary<string, object>();
            _parameters.Add("parm_schema", "SalesLT");
            _parameters.Add("parm_source_schema", "dev");
            _parameters.Add("parm_table", TestContext.Parameters["pl_table"]);

            var pl_names = TestContext.Parameters["pl_name"].Split("|");

            if (pl_names.Length > 0)
            {
                foreach (var name in pl_names)
                {
                    if (name.Contains("pl_new_unit_test"))
                    {
                        _parameters.Add("pl_name", "pl_new_unit_test");
                    }
                }
                
            }                 

        }
    }
}