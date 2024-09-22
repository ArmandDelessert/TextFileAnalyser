namespace TextFileAnalyser.Test;

[TestClass]
public class AnalyserWindowTest
{
    [TestMethod]
    public void AddChar_Then_GetChar_Test()
    {
        // Prepare
        const int windowSize = 4;
        var analyserWindow = new AnalyserWindow(windowSize);

        const int testCharCount = 5;
        char[] expectedChars = ['a', 'b', 'c', 'd', 'e'];
        char[] actualChars = new char[testCharCount];

        // Act
        for (int i = 0; i < testCharCount; i++)
        {
            analyserWindow.AddChar(expectedChars[i]);
            actualChars[i] = analyserWindow.GetChar();
        }

        // Test
        Assert.AreEqual(windowSize, analyserWindow.Window.Length);

        for (int i = 0; i < testCharCount; i++)
        {
            Assert.AreEqual(expectedChars[i], actualChars[i]);
        }
    }

    [TestMethod]
    public void GetWindow_Test()
    {
        // Prepare
        const int windowSize = 4;
        var analyserWindow = new AnalyserWindow(windowSize);

        const int testCharCount = 9;
        char[] expectedChars = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i'];
        IList<IList<char>> windows = [];

        // Act
        for (int i = 0; i < testCharCount; i++)
        {
            analyserWindow.AddChar(expectedChars[i]);
            windows.Add(analyserWindow.GetWindow());
        }

        // Test
        for (int i = 0; i < windows.Count; i++)
        {
            int k = i >= windowSize ? i - windowSize + 1 : 0;
            for (int j = 0; j < windows[i].Count; j++)
            {
                Assert.AreEqual(expectedChars[j + k], windows[i][j]);
            }
        }
    }
}
