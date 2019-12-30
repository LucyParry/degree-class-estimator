using Newtonsoft.Json;

namespace HonoursClassEstimator.Model
{
    public class ClassThresholds : IResult
    {
        [JsonProperty("availableCredit")]
        public int AvailableCredit { get; set; }

        [JsonProperty("firstUpperBound")]
        public decimal FirstUpperBound { get; set; }

        [JsonProperty("upperSecondUpperBound")]
        public decimal UpperSecondUpperBound { get; set; }

        [JsonProperty("lowerSecondUpperBound")]
        public decimal LowerSecondUpperBound { get; set; }

        [JsonProperty("thirdUpperBound")]
        public decimal ThirdUpperBound { get; set; }
    }
}
