namespace TextFileAnalyser.Test
{
    [TestClass]
    public class AnalyserWindowTest
    {
        [TestMethod]
        public void AddChar_5chars()
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
        public void GetChar_()
        {

        }

        [TestMethod]
        public void GetWindow_()
        {

        }
    }
}
