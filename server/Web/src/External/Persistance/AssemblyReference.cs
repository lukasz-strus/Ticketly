using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Persistance;

[ExcludeFromCodeCoverage]
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}