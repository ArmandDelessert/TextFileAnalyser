namespace TextFileAnalyser.Test;

[TestClass]
public class AnalyserTest
{
    private readonly File file = new();

    [DataTestMethod]
    [DataRow("",            0, 0, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0)]
    [DataRow(" ",           1, 1, 1, 0, 0, false, 0, 0, 0, false, 1, 1, 1)]
    [DataRow("\t",          1, 1, 0, 0, 1, false, 0, 0, 0, false, 1, 1, 1)]
    [DataRow("\r",          1, 2, 0, 0, 0, false, 1, 0, 0, false, 0, 2, 2)]
    [DataRow("\n",          1, 2, 0, 0, 0, false, 0, 1, 0, false, 0, 2, 2)]
    [DataRow("\r\n",        2, 2, 0, 0, 0, false, 0, 0, 1, false, 0, 2, 2)]

    [DataRow("a",           1, 1, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0)]
    [DataRow("a\r",         2, 2, 0, 0, 0, false, 1, 0, 0, false, 0, 1, 1)]
    [DataRow("a\n",         2, 2, 0, 0, 0, false, 0, 1, 0, false, 0, 1, 1)]
    [DataRow("a\r\n",       3, 2, 0, 0, 0, false, 0, 0, 1, false, 0, 1, 1)]
    [DataRow("a ",          2, 1, 1, 0, 0, false, 0, 0, 0, false, 1, 0, 0)]
    [DataRow("a \r",        3, 2, 1, 0, 0, false, 1, 0, 0, false, 1, 1, 1)]
    [DataRow("a \n",        3, 2, 1, 0, 0, false, 0, 1, 0, false, 1, 1, 1)]
    [DataRow("a \r\n",      4, 2, 1, 0, 0, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("a ",          2, 1, 1, 0, 0, false, 0, 0, 0, false, 1, 0, 0)]
    [DataRow("a\t\r",       3, 2, 0, 0, 1, false, 1, 0, 0, false, 1, 1, 1)]
    [DataRow("a\t\n",       3, 2, 0, 0, 1, false, 0, 1, 0, false, 1, 1, 1)]
    [DataRow("a\t\r\n",     4, 2, 0, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]

    [DataRow(" \ra\r\t\r",  6, 4, 1, 0, 1, false, 3, 0, 0, false, 2, 3, 2)]
    [DataRow("\t a\r",      4, 2, 1, 0, 1, true,  1, 0, 0, false, 0, 2, 1)] // TODO : Corriger `HasMixedSpaceAndTab`.
    [DataRow("\n\r",        2, 3, 0, 0, 0, false, 1, 1, 0, true,  0, 3, 3)]
    [DataRow(" \r",         2, 2, 1, 0, 0, false, 1, 0, 0, false, 1, 2, 2)]
    [DataRow(" \n",         2, 2, 1, 0, 0, false, 0, 1, 0, false, 1, 2, 2)]
    [DataRow(" \r\n",       3, 2, 1, 0, 0, false, 0, 0, 1, false, 1, 2, 2)]
    [DataRow("\t\r",        2, 2, 0, 0, 1, false, 1, 0, 0, false, 1, 2, 2)]
    [DataRow("\t\n",        2, 2, 0, 0, 1, false, 0, 1, 0, false, 1, 2, 2)]
    [DataRow("\t\r\n",      3, 2, 0, 0, 1, false, 0, 0, 1, false, 1, 2, 2)]

    [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\nabcdefghijklmnopqrstuvwxyz\r\n0123456789\r\n", 68, 4, 0, 0, 0, false, 0, 0, 3, false, 0, 1, 1)]
    public void AnalyzeStreamCharByCharTest(
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

        Assert.AreEqual(expectedCharCount, result.CharCount, "Wrong 'CharCount'.");
        Assert.AreEqual(expectedLineCount, result.LineCount, "Wrong 'LineCount'.");

        Assert.AreEqual(expectedTotalSpaceCount, result.TotalSpaceCount, "Wrong 'TotalSpaceCount'.");
        Assert.AreEqual(expectedDoubleSpaceCount, result.DoubleSpaceCount, "Wrong 'DoubleSpaceCount'.");
        Assert.AreEqual(expectedTotalTabCount, result.TotalTabCount, "Wrong 'TotalTabCount'.");
        Assert.AreEqual(expectedHasMixedSpaceAndTab, result.HasMixedSpaceAndTab, "Wrong 'HasMixedSpaceAndTab'.");

        Assert.AreEqual(expectedCrCount, result.CrCount, "Wrong 'CrCount'.");
        Assert.AreEqual(expectedLfCount, result.LfCount, "Wrong 'LfCount'.");
        Assert.AreEqual(expectedCrLfCount, result.CrLfCount, "Wrong 'CrLfCount'.");
        Assert.AreEqual(expectedHasMixedEndLine, result.HasMixedEndLine, "Wrong 'HasMixedEndLine'.");

        Assert.AreEqual(expectedLineWithTrailingWhitespaceCount, result.LineWithTrailingWhitespaceCount, "Wrong 'LineWithTrailingWhitespaceCount'.");
        Assert.AreEqual(expectedTotalEmptyLineCount, result.TotalEmptyLineCount, "Wrong 'TotalEmptyLineCount'.");
        Assert.AreEqual(expectedFinalEmptyLineCount, result.FinalEmptyLineCount, "Wrong 'FinalEmptyLineCount'.");
    }
}
