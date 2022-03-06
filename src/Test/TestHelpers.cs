using DegreeClassEstimator.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace HonoursClassEstimator.Test
{
    public class TestHelpers
    {
        private string _dataPath;

        public TestHelpers(string dataPath)
        {
            _dataPath = dataPath;
        }

        /// <summary>
        /// Generate CreditClassThresholds by reading from JSON file
        /// </summary>
        public IList<ClassThresholds> GetCreditClassThresholds()
        {
            using (StreamReader streamReader = new StreamReader(_dataPath))
            {
                List<ClassThresholds> thresholds = new List<ClassThresholds>();
                string json = streamReader.ReadToEnd();
                thresholds = JsonConvert.DeserializeObject<List<ClassThresholds>>(json);
                return thresholds;
            }
        }
    }
}
