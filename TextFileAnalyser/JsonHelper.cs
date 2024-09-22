using Newtonsoft.Json.Linq;

namespace TextFileAnalyser;

internal class JsonHelper
{
    public static JObject CreateEmptyJson()
    {
        return [];
    }

    public static void AddEntryToJson(ref JObject jsonObject, string key, string value)
    {
        ArgumentNullException.ThrowIfNull(jsonObject);

        jsonObject[key] = value;
    }
}
