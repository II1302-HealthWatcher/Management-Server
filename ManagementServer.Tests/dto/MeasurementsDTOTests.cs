using ManagementServer.ConsoleApp.dto;
using Xunit;

namespace ManagementServer.Tests.dto
{
    public class MeasurementsDTOTests
    {
        [Fact]
        public void MeasurementsDTOJsonTest()
        {
            MeasurementsDTO firstInstance = new MeasurementsDTO();
            firstInstance.MeasurementDate = "2021-05-08 17:30";
            firstInstance.BloodOxygenLevel = "91";
            firstInstance.BodyTemperature = "36.55";
            firstInstance.HeartPulse = "99";
            string firstInstanceJson = firstInstance.ToJson();

            MeasurementsDTO secondInstance = new MeasurementsDTO();
            secondInstance = MeasurementsDTO.FromJson(firstInstanceJson);

            string secondInstanceJson = secondInstance.ToJson();

            Assert.Equal(firstInstanceJson, secondInstanceJson);
        }
    }
}