using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace ManagementServer.ConsoleApp.dto
{
    public partial class MeasurementsDTO
    {
        [JsonProperty("MeasurementDate")]
        public string MeasurementDate { get; set; }

        [JsonProperty("HeartPulse")]
        public string HeartPulse { get; set; }

        [JsonProperty("BloodOxygenLevel")]
        public string BloodOxygenLevel { get; set; }

        [JsonProperty("BodyTemperature")]
        public string BodyTemperature { get; set; }
    }

    public partial class MeasurementsDTO
    {
        public static MeasurementsDTO FromJson(string json) => JsonConvert.DeserializeObject<MeasurementsDTO>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this MeasurementsDTO self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}