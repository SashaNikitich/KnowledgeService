using KnowledgeService.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KnowledgeService.Configurations;

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