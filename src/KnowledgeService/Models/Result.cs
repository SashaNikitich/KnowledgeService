using Newtonsoft.Json;

namespace KnowledgeService.Models;

public class Result
{
    [JsonProperty]
    public bool Success => !Errors.Any();

    [JsonProperty]
    public List<ResultError> Errors { get; protected set; } = new();

    public static Result Fail(List<ResultError> errors)
    {
        return new Result()
        {
            Errors = errors,
        };
    }

    public static Result Ok()
    {
        return new Result();
    }

    public static Result<T> Ok<T>(T? value = null)
      where T : class
    {
        return new Result<T>(value);
    }

    public static Result<T> Fail<T>(List<ResultError> errors, T? value = null)
      where T : class
    {
        return new Result<T>(value)
        {
            Errors = errors,
        };
    }
}

public class Result<T> : Result
  where T : class
{
    [JsonProperty]
    public T? Value { get; private set; }

    public Result(T? value)
    {
        Value = value;
    }
}
