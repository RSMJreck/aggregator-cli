using System;

using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.Ordering;

namespace integrationtests.cli
{
    public class AddRuleTests : End2EndScenarioBase
    {
        public AddRuleTests(ITestOutputHelper output)
            : base(output)
        {
            // does nothing
        }

        [Fact, Order(1)]
        void Logon()
        {
            (int rc, string output) = RunAggregatorCommand(
                $"logon.azure --subscription {TestLogonData.SubscriptionId} --client {TestLogonData.ClientId} --password {TestLogonData.ClientSecret} --tenant {TestLogonData.TenantId}");
            Assert.Equal(0, rc);
            Assert.DoesNotContain("] Failed!", output);
            (int rc2, string output2) = RunAggregatorCommand(
                $"logon.ado --url {TestLogonData.DevOpsUrl} --mode PAT --token {TestLogonData.PAT}");
            Assert.Equal(0, rc2);
            Assert.DoesNotContain("] Failed!", output2);
        }

        [Fact, Order(2)]
        public void GivenAnInvalidRuleFile_WhenAddingThisRule_ThenTheProcessing_ShouldBeAborted()
        {
            //Given
            const string invalidRuleFileName = "invalid_rule1.rule";

            //When
            (int rc, string output) = RunAggregatorCommand(FormattableString.Invariant($"add.rule --verbose --instance foobar --resourceGroup foobar --name foobar --file {invalidRuleFileName}"));

            //Then
            Assert.Equal(1, rc);
            Assert.Contains(@"Errors in the rule file invalid_rule1.rule:", output);
        }
    }
}