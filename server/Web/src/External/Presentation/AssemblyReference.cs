using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Presentation;

[ExcludeFromCodeCoverage]
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}