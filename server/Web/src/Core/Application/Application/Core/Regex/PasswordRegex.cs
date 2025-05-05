using System.Text.RegularExpressions;

namespace Application.Core.Regex;

public static partial class PasswordRegex
{
    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$", RegexOptions.Compiled)]
    public static partial System.Text.RegularExpressions.Regex PasswordPattern();
}