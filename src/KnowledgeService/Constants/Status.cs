namespace KnowledgeService.Constants;

public static class Status {
  public static readonly int UserNotFound = 0;
  public static readonly int UserIsLocked = 1;
  public static readonly int SignInFailed = 2;
  public static readonly int SignUpCreateUserFailed = 3;
  public static readonly int SignUpAddRoleToUserFailed = 4;
}
