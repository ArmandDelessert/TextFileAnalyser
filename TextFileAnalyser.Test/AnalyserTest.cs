namespace TextFileAnalyser.Test;

[TestClass]
public class AnalyserTest
{
    private readonly File file = new();

    [DataTestMethod]
    [DataRow("",   0, 0, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0)]
    [DataRow(" ",  1, 1, 1, 0, 0, false, 0, 0, 0, false, 1, 1, 1)]
    [DataRow("\t", 1, 1, 0, 0, 1, false, 0, 0, 0, false, 1, 1, 1)]

    [DataRow("\r",   1, 2, 0, 0, 0, false, 1, 0, 0, false, 0, 2, 2)]
    [DataRow("\n",   1, 2, 0, 0, 0, false, 0, 1, 0, false, 0, 2, 2)]
    [DataRow("\r\n", 2, 2, 0, 0, 0, false, 0, 0, 1, false, 0, 2, 2)]

    [DataRow("\n\r", 2, 3, 0, 0, 0, false, 1, 1, 0, true, 0, 3, 3)]

    [DataRow(" \r",    2, 2, 1, 0, 0, false, 1, 0, 0, false, 1, 2, 2)]
    [DataRow(" \n",    2, 2, 1, 0, 0, false, 0, 1, 0, false, 1, 2, 2)]
    [DataRow(" \r\n",  3, 2, 1, 0, 0, false, 0, 0, 1, false, 1, 2, 2)]
    [DataRow("\t\r",   2, 2, 0, 0, 1, false, 1, 0, 0, false, 1, 2, 2)]
    [DataRow("\t\n",   2, 2, 0, 0, 1, false, 0, 1, 0, false, 1, 2, 2)]
    [DataRow("\t\r\n", 3, 2, 0, 0, 1, false, 0, 0, 1, false, 1, 2, 2)]

    [DataRow("a",     1, 1, 0, 0, 0, false, 0, 0, 0, false, 0, 0, 0)]
    [DataRow("a\r",   2, 2, 0, 0, 0, false, 1, 0, 0, false, 0, 1, 1)]
    [DataRow("a\n",   2, 2, 0, 0, 0, false, 0, 1, 0, false, 0, 1, 1)]
    [DataRow("a\r\n", 3, 2, 0, 0, 0, false, 0, 0, 1, false, 0, 1, 1)]

    [DataRow("a ",      2, 1, 1, 0, 0, false, 0, 0, 0, false, 1, 0, 0)]
    [DataRow("a \r",    3, 2, 1, 0, 0, false, 1, 0, 0, false, 1, 1, 1)]
    [DataRow("a \n",    3, 2, 1, 0, 0, false, 0, 1, 0, false, 1, 1, 1)]
    [DataRow("a \r\n",  4, 2, 1, 0, 0, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("a\t",     2, 1, 0, 0, 1, false, 0, 0, 0, false, 1, 0, 0)]
    [DataRow("a\t\r",   3, 2, 0, 0, 1, false, 1, 0, 0, false, 1, 1, 1)]
    [DataRow("a\t\n",   3, 2, 0, 0, 1, false, 0, 1, 0, false, 1, 1, 1)]
    [DataRow("a\t\r\n", 4, 2, 0, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]

    [DataRow(" \ra\r\t\r", 6, 4, 1, 0, 1, false, 3, 0, 0, false, 2, 3, 2)]
    //[DataRow("\t a\r",     4, 2, 1, 0, 1, true,  1, 0, 0, false, 0, 2, 1)] // TODO : Corriger `HasMixedSpaceAndTab`.
    [DataRow("a \ra \ra \ra \ra \ra ", 17, 6, 6, 0, 0, false, 5, 0, 0, false, 6, 0, 0)]
    [DataRow("a \ra \ra \ra \ra \ra \r", 18, 7, 6, 0, 0, false, 6, 0, 0, false, 6, 1, 1)]
    [DataRow("a \ra \ra \ra \ra \ra \r ", 19, 7, 7, 0, 0, false, 6, 0, 0, false, 7, 1, 1)]

    [DataRow(" \t\r\nA", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow(" \t\rA\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow(" \t\n\rA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow(" \t\nA\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow(" \tA\r\n", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 0, 1, 1)]
    [DataRow(" \tA\n\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 2)]
    [DataRow(" \r\t\nA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow(" \r\tA\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow(" \r\n\tA", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow(" \r\nA\t", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 2, 1, 0)]
    [DataRow(" \rA\t\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow(" \rA\n\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow(" \n\t\rA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow(" \n\tA\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow(" \n\r\tA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow(" \n\rA\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow(" \nA\t\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow(" \nA\r\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow(" A\t\r\n", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow(" A\t\n\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow(" A\r\t\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow(" A\r\n\t", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow(" A\n\t\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow(" A\n\r\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]

    [DataRow("\t \r\nA", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow("\t \rA\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\t \n\rA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\t \nA\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\t A\r\n", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 0, 1, 1)]
    [DataRow("\t A\n\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 2)]
    [DataRow("\t\r \nA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow("\t\r A\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\t\r\n A", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow("\t\r\nA ", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 2, 1, 0)]
    [DataRow("\t\rA \n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\t\rA\n ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\t\n \rA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow("\t\n A\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\t\n\r A", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\t\n\rA ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow("\t\nA \r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\t\nA\r ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\tA \r\n", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("\tA \n\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("\tA\r \n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("\tA\r\n ", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("\tA\n \r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("\tA\n\r ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]

    [DataRow("\r \t\nA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\r \tA\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 1)]
    [DataRow("\r \n\tA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\r \nA\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow("\r A\t\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\r A\n\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\r\t \nA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\r\t A\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 1)]
    [DataRow("\r\t\n A", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\r\t\nA ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow("\r\tA \n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\r\tA\n ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\r\n \tA", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 0, 1, 0)]
    [DataRow("\r\n A\t", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow("\r\n\t A", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 0, 1, 0)]
    [DataRow("\r\n\tA ", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow("\r\nA \t", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow("\r\nA\t ", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 0)]
    [DataRow("\rA \t\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\rA \n\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\rA\t \n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\rA\t\n ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\rA\n \t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\rA\n\t ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]

    [DataRow("\n \t\rA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\n \tA\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 1)]
    [DataRow("\n \r\tA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\n \rA\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow("\n A\t\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\n A\r\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\n\t \rA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\n\t A\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 1)]
    [DataRow("\n\t\r A", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\n\t\rA ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 0)]
    [DataRow("\n\tA \r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\n\tA\r ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\n\r \tA", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 0)]
    [DataRow("\n\r A\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\n\r\t A", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 0, 2, 0)]
    [DataRow("\n\r\tA ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\n\rA \t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\n\rA\t ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 0)]
    [DataRow("\nA \t\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\nA \r\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\nA\t \r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\nA\t\r ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 1)]
    [DataRow("\nA\r \t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]
    [DataRow("\nA\r\t ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 1)]

    [DataRow("A \t\r\n", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("A \t\n\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("A \r\t\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A \r\n\t", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 2, 1, 1)]
    [DataRow("A \n\t\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A \n\r\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\t \r\n", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("A\t \n\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("A\t\r \n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\t\r\n ", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 2, 1, 1)]
    [DataRow("A\t\n \r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\t\n\r ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\r \t\n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("A\r \n\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\r\t \n", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("A\r\t\n ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\r\n \t", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("A\r\n\t ", 5, 2, 1, 0, 1, false, 0, 0, 1, false, 1, 1, 1)]
    [DataRow("A\n \t\r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("A\n \r\t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\n\t \r", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("A\n\t\r ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 2, 2, 2)]
    [DataRow("A\n\r \t", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]
    [DataRow("A\n\r\t ", 5, 3, 1, 0, 1, false, 1, 1, 0, true, 1, 2, 2)]

    [DataRow("{\r  a\r}\r", 8, 4, 2, 1, 0, false, 3, 0, 0, false, 0, 1, 1)]
    [DataRow("{\r\ta\r}\r", 7, 4, 0, 0, 1, false, 3, 0, 0, false, 0, 1, 1)]

    [DataRow("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\nabcdefghijklmnopqrstuvwxyz\r\n0123456789\r\n",
        68, 4, 0, 0, 0, false, 0, 0, 3, false, 0, 1, 1)]

    [DataRow("😃 😀 😄 😁 😊 🙂 😬 😐 😶 😑 😌 😔 😕 🙁 😟 ☹️ 😣 😖 😫 😩 😰 😥 😨 😢 😱 😭\r\n",
        54 + 25, 2, 25, 0, 0, false, 0, 0, 1, false, 0, 1, 1)]

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

        Console.WriteLine("Test input:");
        DisplayChars(input.Select(ConvertCharToString1), " ", "", "");
        DisplayChars(input.Select(ConvertCharToString2), "", "\"", "\"");

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

    private static string ConvertCharToString1(char c)
    {
        return c switch
        {
            ' ' => "<Space>",
            '\t' => "<Tab>",
            '\r' => "<CR>",
            '\n' => "<LF>",
            _ => $"'{c}'",
        };
    }

    private static string ConvertCharToString2(char c)
    {
        return c switch
        {
            ' ' => " ",
            '\t' => "\\t",
            '\r' => "\\r",
            '\n' => "\\n",
            _ => $"{c}",
        };
    }

    private static void DisplayChars(IEnumerable<string> chars, string separator, string startDelimiter, string endDelimiter)
    {
        Console.Write(startDelimiter);
        foreach (string c in chars)
        {
            Console.Write($"{c}{separator}");
        }
        Console.WriteLine(endDelimiter);
    }
}
