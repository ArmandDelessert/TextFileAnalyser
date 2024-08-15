namespace TextFileAnalyser.Test
{
    [TestClass]
    public class AnalyserWindowTest
    {
        [TestMethod]
        public void AddChar_Then_GetChar_And_GetWindow()
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

            for (int i = 0; i < analyserWindow.GetWindow().Length; i++)
            {
                Assert.AreEqual(expectedChars[i + 1], analyserWindow.GetWindow()[i]);
            }
        }
    }
}
