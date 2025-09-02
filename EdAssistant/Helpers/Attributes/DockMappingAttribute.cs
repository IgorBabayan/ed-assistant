using EdAssistant.Models.Enums;
using System;

namespace EdAssistant.Helpers.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class DockMappingAttribute(DockEnum dock) : Attribute
{
    public DockEnum Dock { get; } = dock;
}