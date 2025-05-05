using Application.Core.Abstractions.Common;

namespace Services.Common;

internal sealed class MachineDateTime : IDateTime
{
    public DateTime UtcNow => DateTime.UtcNow;
}
