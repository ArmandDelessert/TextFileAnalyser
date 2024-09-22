namespace TextFileAnalyser.Test;

[TestClass]
public class AnalyserTest
{
    private readonly File file = new();

    [DataTestMethod]
    [DataRow("", 0, 0, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0)]
    [DataRow(" ", 1, 1, 1, 0, 0, false, 0, 0, 0, false, 1, 1, 1)]
    [DataRow("\t", 1, 1, 0, 0, 1, false, 0, 0, 0, false, 1, 1, 1)]
    [DataRow("\r", 1, 1, 0, 0, 0, false, 1, 0, 0, false, 0, 1, 1)]
    [DataRow("\n", 1, 1, 0, 0, 0, false, 0, 1, 0, false, 0, 1, 1)]
    [DataRow("\r\n", 2, 1, 0, 0, 0, false, 0, 0, 1, false, 0, 1, 1)]
    [DataRow("a\r", 2, 1, 0, 0, 0, false, 1, 0, 0, false, 0, 1, 1)] // TODO : Il y a une dernière ligne vide ou pas là ?
    [DataRow("a\n", 2, 1, 0, 0, 0, false, 0, 1, 0, false, 0, 1, 1)] // TODO : Il y a une dernière ligne vide ou pas là ?
    [DataRow("a\r\n", 3, 1, 0, 0, 0, false, 0, 0, 1, false, 0, 1, 1)] // TODO : Il y a une dernière ligne vide ou pas là ?
    [DataRow(" \ra\r\t\r", 6, 3, 1, 0, 1, false, 3, 0, 0, false, 2, 2, 1)]
    [DataRow("\t a\r", 4, 1, 1, 0, 1, true, 1, 0, 0, false, 0, 0, 0)] // TODO : Corriger `HasMixedSpaceAndTab`.
    [DataRow("\n\r", 2, 2, 0, 0, 0, false, 1, 1, 0, true, 0, 2, 2)]
    [DataRow(" \r", 2, 1, 1, 0, 0, false, 1, 0, 0, false, 1, 1, 1)]
    [DataRow(" \n", 2, 1, 1, 0, 0, false, 0, 1, 0, false, 1, 1, 1)]
    [DataRow(" \r\n", 3, 1, 1, 0, 0, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("\t\r", 2, 1, 0, 0, 1, false, 1, 0, 0, false, 1, 1, 1)]
    [DataRow("\t\n", 2, 1, 0, 0, 1, false, 0, 1, 0, false, 1, 1, 1)]
    [DataRow("\t\r\n", 3, 1, 0, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\nabcdefghijklmnopqrstuvwxyz\r\n0123456789\r\n", 68, 3, 0, 0, 0, false, 0, 0, 3, false, 0, 0, 0)]
    public void AnalyzeStreamCharByChar_TestCases(
    string input,
    int expectedCharCount,
    int expectedLineCount,
    int expectedTotalSpaceCount,
    int expectedDoubleSpaceCount,
    int expectedTotalTabCount,
    bool expectedHasMixedSpaceAndTab,
    int expectedCrCount,
    int expectedLfCount,
    int expectedCrLfCount,
    bool expectedHasMixedEndLine,
    int expectedLineWithTrailingWhitespaceCount,
    int expectedTotalEmptyLineCount,
    int expectedFinalEmptyLineCount)
    {
        // Préparation

        var analyser = new Analyser();
        var reader = new StringReader(input);

        // Action

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Assertions

        Assert.AreEqual(expectedCharCount, result.CharCount, "CharCount");
        Assert.AreEqual(expectedLineCount, result.LineCount, "LineCount");

        Assert.AreEqual(expectedTotalSpaceCount, result.TotalSpaceCount, "TotalSpaceCount");
        Assert.AreEqual(expectedDoubleSpaceCount, result.DoubleSpaceCount, "DoubleSpaceCount");
        Assert.AreEqual(expectedTotalTabCount, result.TotalTabCount, "TotalTabCount");
        Assert.AreEqual(expectedHasMixedSpaceAndTab, result.HasMixedSpaceAndTab, "HasMixedSpaceAndTab");

        Assert.AreEqual(expectedCrCount, result.CrCount, "CrCount");
        Assert.AreEqual(expectedLfCount, result.LfCount, "LfCount");
        Assert.AreEqual(expectedCrLfCount, result.CrLfCount, "CrLfCount");
        Assert.AreEqual(expectedHasMixedEndLine, result.HasMixedEndLine, "HasMixedEndLine");

        Assert.AreEqual(expectedLineWithTrailingWhitespaceCount, result.LineWithTrailingWhitespaceCount, "LineWithTrailingWhitespaceCount");
        Assert.AreEqual(expectedTotalEmptyLineCount, result.TotalEmptyLineCount, "TotalEmptyLineCount");
        Assert.AreEqual(expectedFinalEmptyLineCount, result.FinalEmptyLineCount, "FinalEmptyLineCount");
    }
}
