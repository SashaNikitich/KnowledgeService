namespace KnowledgeService.Settings;

public interface IAuthorizationSettings
{
  string SigningKey { get; }
  string EncryptionKey { get; }
  int ExpirationTimeInHours { get; }
}
