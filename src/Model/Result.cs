﻿using System.Collections.Generic;

namespace DegreeClassEstimator.Model
{
    public class Result<T>
    {
        public bool Success { get; set; }

        public IList<string> Errors { get; set; }

        public T ReturnObject { get; set; }


        public Result()
        {
            Errors = new List<string>();
        }

        public Result(bool success, T returnObject)
        {
            Success = success;
            Errors = new List<string>();
            ReturnObject = returnObject;
        }

        public Result(bool success, IList<string> errors)
        {
            Success = success;
            Errors = errors;
        }
    }
}
