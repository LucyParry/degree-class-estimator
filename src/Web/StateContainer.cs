using DegreeClassEstimator.Model;

namespace DegreeClassEstimator.Web
{
    public class StateContainer
    {
        public StateContainer()
        {
            Degree = new Degree();
        }

        public Degree Degree { get; set; }

        public event Action OnChange;


        public void AddModule(Module newModule)
        {
            var module = new Module()
            {
                Code = newModule.Code,
                Points = newModule.Points,
                Compulsory = newModule.Compulsory,
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


        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
