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
        /// <summary>
        /// Deserializes a Json string to a <see cref="MeasurementsDTO"/> instance.
        /// </summary>
        /// <param name="json">The Json string to be used in the deserialization.</param>
        /// <returns>A <see cref="MeasurementsDTO"/> instance.</returns>
        public static MeasurementsDTO FromJson(string json) => JsonConvert.DeserializeObject<MeasurementsDTO>(json, Converter.Settings);
    }

    public static class Serialize
    {
        /// <summary>
        /// Serializes a <see cref="MeasurementsDTO"/> instance to a Json string.
        /// </summary>
        /// <returns>A Json string.</returns>
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