namespace KnowledgeService.Settings;

public interface IAuthorizationSettings
{
  string SigningKey { get; }
  string EncryptionKey { get; }
  int ExpirationTimeInHours { get; }
  string Issuer { get; }
  string Audience { get; }
}
