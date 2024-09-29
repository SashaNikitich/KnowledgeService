namespace KnowledgeService.Settings;

public interface IAuthorizationSettings
{
  string SigningKey { get; }
  string EncryptionKey { get; }
  string Issuer { get; }
  string Audience { get; }
  int ExpirationTimeInHours { get; }
}
