namespace TextFileAnalyser;

internal static class Characters
{
    public const char Space = ' ';
    public const char Tabulation = '\t';
    public const char CarriageReturn = '\r';
    public const char LineFeed = '\n';

    public const string CrLf = "\r\n";

    public static string MultiSpaces(int spaceCount) => new(' ', spaceCount);
}
