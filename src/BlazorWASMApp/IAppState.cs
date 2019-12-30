using HonoursClassEstimator.Model;
using System;

namespace HonoursClassEstimator.BlazorWASMApp
{
    public interface IAppState
    {
        event Action OnChange;
        
        void AddModule(Module newModule);

        void RemoveModule(Module module);

        void ClassifyDegree();

        void NotifyStateChanged();
    }
}
