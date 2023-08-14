using MiniMax;

namespace MiniMaxTests
{
    class TestNode : MiniMax.INode
    {
        public bool Terminal => throw new NotImplementedException();

        public IEnumerable<INode> Children => throw new NotImplementedException();

        public int CalculateScore(Player maximizingPlayer)
        {
            throw new NotImplementedException();
        }
    }


    [TestClass]
    public class MiniMaxTests
    {
        [TestMethod]
        public void TestMiniMax_Empty()
        {
            var ttt = new TicTacToe();
            var score = MiniAax.MiniMax(ttt, 0, -10000, 10000, Player.One);

            // Expect 0 score with depth zero because the board is empty
            Assert.AreEqual(0, score);

            score = MiniAax.MiniMax(ttt, 1, -10000, 10000, Player.One);

            // Expect 6 score with depth 1 because the center cell is available
            Assert.AreEqual(6, score);
        }

        [TestMethod]
        public void TestMiniMax_SureVictory()
        {
            var ttt = new TicTacToe("xx.\noxo\no..");
            var score = MiniAax.MiniMax(ttt, 0, -10000, 10000, Player.One);

            // Expect 1,000 score because of victory
            Assert.AreEqual(1000, score);
        }

    }
}