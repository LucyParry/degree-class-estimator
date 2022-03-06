using System.Collections.Generic;

namespace DegreeClassEstimator.Model
{
    public class ResultCollection<T> where T : IEnumerable<IResult>
    {
        public bool Success { get; set; }

        public IList<string> Errors { get; set; }

        public T ReturnObjects { get; set; }


        public ResultCollection()
        {
            Errors = new List<string>();
        }

        public ResultCollection(bool success, T returnObjects)
        {
            Success = success;
            ReturnObjects = returnObjects;
        }

        public ResultCollection(bool success, IList<string> errors)
        {
            Success = success;
            Errors = errors;
        }
    }
}
