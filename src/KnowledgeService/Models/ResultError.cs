namespace KnowledgeService.Models;

public class ResultError {
  public int Status { get; }

  public ResultError(int status) {
    Status = status;
  }

  public static void AddError(List<ResultError> errors, int status) {
    var error = new ResultError(status);
    errors.Add(error);
  }
}
