namespace TextFileAnalyser.Test
{
    [TestClass]
    public class RollingIndexTest
    {
        [TestMethod]
        public void MinIndex_MaxIndex_Test()
        {
            // Prepare
            const int minIndex = 0;
            const int maxIndex = 3;

            // Act
            var rollingIndex = new RollingIndex(minIndex, maxIndex);

            // Test
            Assert.AreEqual(minIndex, rollingIndex.MinIndex);
            Assert.AreEqual(maxIndex, rollingIndex.MaxIndex);
            Assert.AreEqual(maxIndex - minIndex + 1, rollingIndex.Range);
        }

        [TestMethod]
        public void Increase_Test()
        {
            // Prepare
            const int minIndex = 0;
            const int maxIndex = 3;
            var rollingIndex = new RollingIndex(minIndex, maxIndex);

            // Act
            var index_0 = rollingIndex.Index;
            var downRollingCount_A_0 = rollingIndex.DownRollingCount;
            var upRollingCount_A_0 = rollingIndex.UpRollingCount;

            rollingIndex.Increase();
            var index_1 = rollingIndex.Index;
            var downRollingCount_B_0 = rollingIndex.DownRollingCount;
            var upRollingCount_B_0 = rollingIndex.UpRollingCount;

            rollingIndex.Increase(2);
            var index_3 = rollingIndex.Index;
            var downRollingCount_C_0 = rollingIndex.DownRollingCount;
            var upRollingCount_C_0 = rollingIndex.UpRollingCount;

            rollingIndex.Increase(1);
            var index_4 = rollingIndex.Index;
            var downRollingCount_D_0 = rollingIndex.DownRollingCount;
            var upRollingCount_D_1 = rollingIndex.UpRollingCount;

            rollingIndex.Increase(3);
            var index_7 = rollingIndex.Index;
            var downRollingCount_E_0 = rollingIndex.DownRollingCount;
            var upRollingCount_E_1 = rollingIndex.UpRollingCount;

            rollingIndex.Increase(2);
            var index_9 = rollingIndex.Index;
            var downRollingCount_F_0 = rollingIndex.DownRollingCount;
            var upRollingCount_F_2 = rollingIndex.UpRollingCount;

            rollingIndex.Increase(-10);
            var index_01 = rollingIndex.Index;
            var downRollingCount_G_3 = rollingIndex.DownRollingCount;
            var upRollingCount_G_2 = rollingIndex.UpRollingCount;

            // Test
            Assert.AreEqual(0, index_0);
            Assert.AreEqual(0, downRollingCount_A_0);
            Assert.AreEqual(0, upRollingCount_A_0);
            Assert.AreEqual(1, index_1);
            Assert.AreEqual(0, downRollingCount_B_0);
            Assert.AreEqual(0, upRollingCount_B_0);
            Assert.AreEqual(3, index_3);
            Assert.AreEqual(0, downRollingCount_C_0);
            Assert.AreEqual(0, upRollingCount_C_0);
            Assert.AreEqual(0, index_4);
            Assert.AreEqual(0, downRollingCount_D_0);
            Assert.AreEqual(1, upRollingCount_D_1);
            Assert.AreEqual(3, index_7);
            Assert.AreEqual(0, downRollingCount_E_0);
            Assert.AreEqual(1, upRollingCount_E_1);
            Assert.AreEqual(1, index_9);
            Assert.AreEqual(0, downRollingCount_F_0);
            Assert.AreEqual(2, upRollingCount_F_2);
            Assert.AreEqual(3, index_01);
            Assert.AreEqual(3, downRollingCount_G_3);
            Assert.AreEqual(2, upRollingCount_G_2);
        }
    }
}
