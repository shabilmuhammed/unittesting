using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace adf_unit_testing
{
    public class Test2 : datafactoryhelper
    {
        private datafactoryhelper _helper;
        public IDictionary<string, object> _parameters;
        public TestContext TestContext { get; set; }

        [OneTimeSetUp]
        public async Task WhenPipelineIsRun()
        {
            if (_parameters["pl_name"].ToString() != "pl_list_test")
            {
                Assert.Ignore("unmatched pipeline");
            }

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

        public Test2()
        {
            _helper = new datafactoryhelper();
            _parameters = new Dictionary<string, object>();
            _parameters.Add("parm_schema", "SalesLT");
            _parameters.Add("parm_source_schema", "dev");
            _parameters.Add("parm_table", TestContext.Parameters["pl_table"]);

            var pl_names = TestContext.Parameters["pl_name"].Split("|");
            var foundFlag = 0;
            if (pl_names.Length > 0)
            {
                foreach (var name in pl_names)
                {
                    if (name=="pl_list_test")
                    {
                        foundFlag = 1;
                        _parameters.Add("pl_name", "pl_list_test");
                    }
                }

            }
            if (foundFlag == 0)
            {
                _parameters.Add("pl_name", "no_pl");
            }

        }
    }
}