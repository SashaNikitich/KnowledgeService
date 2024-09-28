using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KnowledgeService.Entities;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder
        .Property(x => x.Email)
        .IsRequired();

    builder
        .Property(x => x.UserName)
        .IsRequired();

    builder
        .Property(x => x.PasswordHash)
        .IsRequired();
   }
}
