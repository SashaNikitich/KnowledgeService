using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KnowledgeService.Entities;

public sealed class TestConfiguration : IEntityTypeConfiguration<Test>
{
  public void Configure(EntityTypeBuilder<Test> builder)
  {
    builder
        .Property(x => x.Title)
        .IsRequired();

    builder
        .Property(x => x.Description)
        .IsRequired();
  }
}
