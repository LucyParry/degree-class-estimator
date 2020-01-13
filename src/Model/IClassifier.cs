using HonoursClassEstimator.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonoursClassEstimator.Model
{
    /// <summary>
    /// TODO: Look at moving calculation logic into Strategy pattern using this interface?
    /// </summary>
    public interface IClassifier
    {
        void Classify(Degree degree, ClassThresholds[] thresholdsList);
    }
}
