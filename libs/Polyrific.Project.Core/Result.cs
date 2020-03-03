using System.Collections.Generic;

namespace Polyrific.Project.Core
{
    public class Result
    {
        public Result()
        {

        }

        public Result(bool success)
        {
            Success = success;
        }

        public Result(bool success, IEnumerable<string> errors)
        {
            Success = success;
            Errors = errors;
        }

        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static Result SuccessResult => new Result(true);

        public static Result FailedResult(string error) => new Result(false, new List<string> { error });
    }

    public class Result<T> where T : class
    {
        public Result()
        {

        }

        public Result(bool success, T item)
        {
            Success = success;
            Item = item;
        }

        public Result(bool success, T item, IEnumerable<string> errors)
        {
            Success = success;
            Item = item;
            Errors = errors;
        }

        public bool Success { get; set; }
        public T Item { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static Result<T> SuccessResult(T item) => new Result<T>(true, item);
        public static Result<T> FailedResult(T item, string error) => new Result<T>(false, item, new List<string> { error });
    }
}
