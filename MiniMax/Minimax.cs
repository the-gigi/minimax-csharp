using System.Drawing;

namespace MiniMax
{
    public enum Player
    {
        One = 1,
        Two
    }

    /// <summary>
    /// Extension method to get the othe player
    /// </summary>
    static class PlayerIdMethods
    {
        public static Player Other(this Player player)
        {
            return player == Player.One ? Player.Two : Player.One;
        }
    }

    public interface INode
    {

        bool Terminal { get; }
        int CalculateScore(Player maximizingPlayer);
        IEnumerable<INode> Children { get; }
    }

    public static class MiniAax
    {
        /// <summary>
        /// Minimax implements the Negamax variant of the Minimax algoeithm
        /// with alpha-beta pruning.
        /// 
        /// See https://en.wikipedia.org/wiki/Negamax
        /// </summary>
        /// <param name="node"></param>
        /// <param name="depth"></param>
        /// <param name="α"></param>
        /// <param name="β"></param>
        /// <param name="maximixingPlayer"></param>
        /// <returns></returns>
        public static int MiniMax(INode node, int depth, int α, int β, Player maximixingPlayer)
        {
            if (node.Terminal || depth <= 0)
            {
                return node.CalculateScore(maximixingPlayer);
            }

            var value = int.MinValue;
            foreach (var child in node.Children)
            {
                value = Math.Max(value, -MiniMax(child, depth - 1, -β, -α, maximixingPlayer.Other()));
                α = Math.Max(α, value);
                if (α >= β)
                {
                    break;
                }
            }
             
            return value;
        }
    }
}