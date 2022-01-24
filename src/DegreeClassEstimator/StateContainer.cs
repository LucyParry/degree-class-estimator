using HonoursClassEstimator.Model;

namespace DegreeClassEstimator
{
    public class StateContainer
    {
        public Degree Degree { get; set; }

        public static string AppBaseHref => "/degree-class-estimator/";

        public IList<string> Errors { get; set; }

        public event Action OnChange;

        public StateContainer()
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


        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
