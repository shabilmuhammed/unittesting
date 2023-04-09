using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace adf_unit_testing
{
    public class runplstest : datafactoryhelper
    {
        private datafactoryhelper _helper;
        public IDictionary<string, object> _parameters;
        public TestContext TestContext { get; set; }

        [OneTimeSetUp]
        public async Task WhenPipelineIsRun()
        {

            var pl_names = TestContext.Parameters["pl_name"].Split("|");

            if (pl_names.Length > 0)
            {
                foreach (var name in pl_names)
                {
                    if (name.Contains("pl_"))
                    {
                        await _helper.RunPipeline(name, _parameters);
                    }
                }

            }

        }

        [Test]
        public void ThenPipelineOutcomeIsSucceeded()
        {
            _helper.PipelineOutcome.Should().Be("Succeeded");
        }

 

        public runplstest()
        {
            _helper = new datafactoryhelper();

        }
    }
}