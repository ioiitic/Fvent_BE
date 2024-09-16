using System.Text.Json;

namespace Fvent.API.Policy;

public class KebabCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        // Convert the name to kebab-case
        return string.Concat(
            name.Select((ch, i) => i > 0 && char.IsUpper(ch) ? "-" + char.ToLower(ch).ToString() : char.ToLower(ch).ToString())
        );
    }
}
