using HonoursClassEstimator.Model;
using System;
using System.Collections.Generic;

namespace HonoursClassEstimator.BlazorWASMApp
{
    public class AppState
    {
        public Degree Degree { get; set; }

        public static string AppBaseHref => "/degree-class-estimator/";

        public IList<string> Errors { get; set; }
        
        public event Action OnChange;

        public AppState()
        {
            Degree = new Degree();
            Errors = new List<string>();
        }


        public void AddModule(Module newModule)
        {
            var module = new Module()
            {
                Description = newModule.Description,
                Points = newModule.Points,
                Compulsary = newModule.Compulsary,
                Level = newModule.Level,
                Grade = newModule.Grade
            };
            Degree.AddModule(module);
            NotifyStateChanged();
        }


        public void RemoveModule(Module module)
        {
            Degree.RemoveModule(module);
            NotifyStateChanged();
        }


        public void ClassifyDegree(ClassThresholds[] thresholds)
        {
            var classifier = new StandardClassifier(thresholds);
            Degree.Classify(classifier);
            NotifyStateChanged();
        }


        private void NotifyStateChanged() => OnChange?.Invoke();

    }
}
