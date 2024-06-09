using Core.Enums;

namespace Core.Attributes;
[AttributeUsage(AttributeTargets.Property)]
public class PropertyField : Attribute
{
    public string PropertyName { get; set; } = string.Empty;
    public PropertyTypeEnum PropertyType { get; set; }
    public bool IsShow { get; set; }
    
}