using MiniMax;
using System.Data;
using System.Text;

namespace MiniMaxTests
{
    internal class TicTacToe : MiniMax.INode
    {
        StringBuilder displayState;
        int[,] cells;
        int count = 0;
        public TicTacToe(string state = "...\n...\n...")
        {
            displayState = new StringBuilder(state);
            cells = new int[3,3];
            // parse state to rows
            var rows = state.Split('\n');
            if (rows.Length != 3)
            {
                throw new Exception("Invalid board. Must be 3 rows");
            }

            for (var i = 0; i < 3; ++i)
            {
                var row = rows[i];
                if (row.Length != 3)
                {
                    throw new Exception("Inbvalid row. Must be 3 elements");
                }
                for (var j = 0; j < 3; ++j)
                {
                    // convert '.', 'x', 'o' to 0, 1, 2.
                    var value = ".xo".IndexOf(row[j]);

                    if (value == -1)
                    {
                        throw new Exception("Invalid value. Must be one of 'x', 'o' or '.'");
                    }
                    cells[j,i] = value;
                    if (value > 0)
                    {
                        count++;
                    }
                }
            }
        }

        private int checkRow(int row)
        {
            var total = 0;
            for (var i = 0; i < 3; i++)
            {
                if (cells[i, row] == 0)
                {
                    return 0;
                }
                total += cells[i, row];
            }

            return total % 3 == 0 ? total / 3 : 0;
        }

        private int checkCol(int col)
        {
            var total = 0;
            for (var i = 0; i < 3; i++)
            {
                if (cells[col, i] == 0)
                {
                    return 0;
                }
                total += cells[col, i];
            }

            return total % 3 == 0 ? total / 3 : 0;
        }
        private int checkDiagonals()
        {
            var topLeftTotal = 0;
            var bottomLeftTotal = 0;
            for (var i = 0; i < 3; i++)
            {
                // top-left diagonal
                if (cells[i, i] == 0)
                {
                    topLeftTotal = -1000;
                }
                else
                {
                    topLeftTotal += cells[i, i];
                }
                // bottom-left diagonal
                if (cells[i, 2 - i] == 0)
                {
                    bottomLeftTotal = -1000;
                }
                else
                {
                    bottomLeftTotal += cells[i, 2 - i];
                }
            }

            if (topLeftTotal == 3 || topLeftTotal == 6)
            {
                return topLeftTotal / 3;
            }
            if (bottomLeftTotal == 3 || bottomLeftTotal == 6)
            {
                return topLeftTotal / 3;
            }

            return 0;
        }

        private int checkVictory()
        {
            for (int i = 0; i < 3; ++i)
            {
                var winner = checkRow(i) + checkCol(i);
                if (winner != 0)
                {
                    return winner;
                }
            }
            return checkDiagonals();
        }

        public void Place(int col, int row, Player player)
        {
            if (cells[col, row] != 0) {
                throw new Exception("cell is occupied");
            }
            cells[col, row] = (int)player;
            count++;

            // update display state
            var index = row * 4 + col;
            displayState[index] = player == Player.One ? 'x' : 'o';
        }

        private IEnumerable<INode> generateBoards()
        {
            var player = count % 2 == 0 ? Player.One : Player.Two;
            var children = new List<INode>();
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (cells[j, i] == 0)
                    {
                        var node = new TicTacToe(displayState.ToString());
                        node.Place(j, i, player);
                        children.Add(node);
                    }
                }
            }

            return children;
        }
        public bool Terminal => checkVictory() > 0 || count == 9;

        public IEnumerable<INode> Children => generateBoards();

        public int CalculateScore(Player maximizingPlayer)
        {
            var winner = checkVictory();
            if (winner == (int)maximizingPlayer)
            {
                return 1000;
            }
            if (winner != 0) // other player
            {
                return -1000;
            }

            // check for center
            var score = 0;
            var center = cells[1, 1];
            if (center == (int)maximizingPlayer)
            {
                score += 6;
            }
            else if (center != 0) // other player got the center
            {
                score -= 6;
            }

            // check for potential victories
            var victoryCount = 0;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (cells[j, i] == 0)
                    {
                        var node = new TicTacToe(displayState.ToString());
                        node.Place(j, i, maximizingPlayer);
                        if (node.checkVictory() == (int)maximizingPlayer)
                        {
                            victoryCount++;
                            if (victoryCount > 1) // multipl victory positions
                            {
                                return 1000;
                            }
                        }
                    }
                }
            }
            if (victoryCount > 0)
            {
                score += 2;
            }

            return score;
        }
    }
}
