namespace Uncas.BuildPipeline.ApplicationServices.Results
{
    using System.Collections.Generic;

    public abstract class BaseResult
    {
        private readonly IList<ResultError> errors;

        protected BaseResult()
        {
            errors = new List<ResultError>();
        }

        public bool Success { get; protected set; }

        public IEnumerable<ResultError> Errors
        {
            get { return errors; }
        }

        public bool HasErrors
        {
            get { return errors.Count > 0; }
        }

        public void AddError(ResultError error)
        {
            errors.Add(error);
        }
    }
}