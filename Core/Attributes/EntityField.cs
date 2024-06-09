namespace Core.Attributes;
[AttributeUsage(AttributeTargets.Class)]
public class EntityField : Attribute
{
    public string EntityName { get; set; } = string.Empty;
    public bool IsShow { get; set; }
    public int Sort { get; set; }
}