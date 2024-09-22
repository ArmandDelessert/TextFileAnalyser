namespace TextFileAnalyser.Test;

[TestClass]
public class AnalyserTest
{
    private readonly File file = new();

    /* TODO : Combinaisons à tester :
    " \r"
    " \n"
    " \r\n"
    "\t\r"
    "\t\n"
    "\t\r\n"
     */

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithEmptyText()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(0, result.CharCount);
        Assert.AreEqual(0, result.LineCount);

        Assert.AreEqual(0, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(0, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(0, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(0, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(0, result.TotalEmptyLineCount);
        Assert.AreEqual(0, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithOneSpace()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader(" ");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(1, result.CharCount);
        Assert.AreEqual(1, result.LineCount);

        Assert.AreEqual(1, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(0, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(0, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(1, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(1, result.TotalEmptyLineCount);
        Assert.AreEqual(1, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithOneTab()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("\t");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(1, result.CharCount);
        Assert.AreEqual(1, result.LineCount);

        Assert.AreEqual(0, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(1, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(0, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(1, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(1, result.TotalEmptyLineCount);
        Assert.AreEqual(1, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithOneCr()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("\r");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(1, result.CharCount);
        Assert.AreEqual(1, result.LineCount);

        Assert.AreEqual(0, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(0, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(1, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(0, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(1, result.TotalEmptyLineCount);
        Assert.AreEqual(1, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithOneLf()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("\n");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(1, result.CharCount);
        Assert.AreEqual(1, result.LineCount);

        Assert.AreEqual(0, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(0, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(0, result.CrCount);
        Assert.AreEqual(1, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(0, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(1, result.TotalEmptyLineCount);
        Assert.AreEqual(1, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithOneCrLf()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("\r\n");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(2, result.CharCount);
        Assert.AreEqual(1, result.LineCount);

        Assert.AreEqual(0, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(0, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(0, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(1, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(0, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(1, result.TotalEmptyLineCount);
        Assert.AreEqual(1, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithTrailingWhitespaceAndEmptyLines()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader(" \ra\r\t\r");

        // Act
        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test
        Assert.AreEqual(6, result.CharCount);
        Assert.AreEqual(3, result.LineCount);

        Assert.AreEqual(1, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(1, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(3, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(2, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(2, result.TotalEmptyLineCount);
        Assert.AreEqual(1, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithMixedSpaceAndTab()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("\t a\r");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(4, result.CharCount);
        Assert.AreEqual(1, result.LineCount);

        Assert.AreEqual(1, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(1, result.TotalTabCount);
        Assert.IsTrue(result.HasMixedSpaceAndTab);

        Assert.AreEqual(1, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(0, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(0, result.TotalEmptyLineCount);
        Assert.AreEqual(0, result.FinalEmptyLineCount);
    }

    [TestMethod]
    public void AnalyzeStreamCharByChar_WithMixedEndLine()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("\n\r");

        // Act

        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test

        Assert.AreEqual(2, result.CharCount);
        Assert.AreEqual(2, result.LineCount);

        Assert.AreEqual(0, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(0, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(1, result.CrCount);
        Assert.AreEqual(1, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsTrue(result.HasMixedEndLine);

        Assert.AreEqual(0, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(2, result.TotalEmptyLineCount);
        Assert.AreEqual(2, result.FinalEmptyLineCount);
    }

    /*
    [TestMethod]
    public void AnalyzeStreamCharByChar_With()
    {
        // Prepare

        var analyser = new Analyser();

        var reader = new StringReader("");

        // Act
        var result = analyser.AnalyzeStreamCharByChar(reader, file);

        // Test
        Assert.AreEqual(0, result.CharCount);
        Assert.AreEqual(0, result.LineCount);

        Assert.AreEqual(0, result.TotalSpaceCount);
        Assert.AreEqual(0, result.DoubleSpaceCount);
        Assert.AreEqual(0, result.TotalTabCount);
        Assert.IsFalse(result.HasMixedSpaceAndTab);

        Assert.AreEqual(0, result.CrCount);
        Assert.AreEqual(0, result.LfCount);
        Assert.AreEqual(0, result.CrLfCount);
        Assert.IsFalse(result.HasMixedEndLine);

        Assert.AreEqual(0, result.LineWithTrailingWhitespaceCount);
        Assert.AreEqual(0, result.TotalEmptyLineCount);
        Assert.AreEqual(0, result.FinalEmptyLineCount);
    }
    */
}
