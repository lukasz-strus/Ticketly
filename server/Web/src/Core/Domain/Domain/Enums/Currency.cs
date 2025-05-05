using Domain.Core.Primitives;

namespace Domain.Enums;

public sealed class Currency : Enumeration<Currency>
{
    public static readonly Currency Usd = new(1, "Dollar", "USD");
    public static readonly Currency Eur = new(2, "Euro", "EUR");
    public static readonly Currency Pln = new(3, "Złoty", "PLN");

    internal static readonly Currency None = new(0, string.Empty, string.Empty);

    private Currency(int value, string name, string code)
        : base(value, name)
    {
        Code = code;
    }

    // Ctor for EF
    // ReSharper disable once UnusedMember.Local
    private Currency()
    {
        Code = string.Empty;
    }

    public string Code { get; }

    public string Format(decimal amount) => $"{amount:n2} {Code}";
}