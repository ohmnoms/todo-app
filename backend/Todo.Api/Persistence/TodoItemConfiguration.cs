using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Todo.Api.Models.Domain;

namespace Todo.Api.Persistence;

public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
{
    public void Configure(EntityTypeBuilder<TodoItem> builder)
    {
        builder.ToTable("TodoItems");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(240);

        builder.Property(t => t.IsCompleted)
            .IsRequired();
        
        builder.Property(t => t.DueDate)
            .HasConversion(
                d => d.HasValue ? d.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null,
                dt => dt.HasValue ? DateOnly.FromDateTime(dt.Value) : null)
            .HasColumnType("TEXT");

        builder.Property(t => t.DueTime)
            .HasConversion(
                t => t.HasValue ? t.Value.ToTimeSpan() : (TimeSpan?)null,
                ts => ts.HasValue ? TimeOnly.FromTimeSpan(ts.Value) : null)
            .HasColumnType("TEXT");

        builder.Property(t => t.CreatedDate)
            .IsRequired();
        
        builder.Property(t => t.CreatedBy)
            .IsRequired();
    }
}