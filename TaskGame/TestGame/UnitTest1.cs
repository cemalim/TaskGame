using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

[TestClass]
public class MinesweeperGameTests
{
    [TestMethod]
    public void ValidMove_PlayerMovesUp_PlayerPositionUpdated()
    {
        // Arrange
        int boardSize = 3;
        int numLives = 3;
        IGameBoard board = new GameBoard(boardSize);
        IGame game = new MinesweeperGame(board, numLives);

        // Act
        game.Play("up"); // Simulate player moving up

        // Assert
        Tuple<int, int> playerPosition = board.FindPlayerPosition();
        Assert.IsNotNull(playerPosition);
        Assert.AreEqual(0, playerPosition.Item1); // Player should be in the first row
        Assert.AreEqual(0, playerPosition.Item2); // Player column should remain the same
    }

    [TestMethod]
    public void InvalidMove_PlayerMovesLeft_InvalidOperationExceptionThrown()
    {
        // Arrange
        int boardSize = 3;
        int numLives = 3;
        IGameBoard board = new GameBoard(boardSize);
        IGame game = new MinesweeperGame(board, numLives);

        // Act and Assert
        Assert.ThrowsException<InvalidOperationException>(() => game.Play("left"));
    }

}
