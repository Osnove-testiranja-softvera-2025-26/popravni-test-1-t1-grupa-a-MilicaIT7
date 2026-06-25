

using Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing;
using NUnit.Framework;
using OTS2026_PT1_GrupaA.Exceptions;
using OTS2026_PT1_GrupaA.Models;
using System;

namespace OTS2026_PT1_GrupaA.Test
{
    public class GameTest
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
           _game = new Game(new Position(5, 5), new Position(6, 5));
        }

        #region Constructor
        //za dobru poziciju i izuzetke

        [Test]
        public void Constructor_PlayerAndBoatOnLand_PlayerCreated()
        {
            Assert.That(_game.Player.Position, Is.EqualTo(new Position(5, 5)));
        }

        [TestCase(20, 0)]

        [TestCase(25, 10)]
        public void Constructor_PlayerInPond_ThrowsException(int x, int y)
        {
            Exception ex = Assert.Throws<InvalidPlayerPositionException>((TestDelegate) => new Game(new Position(x, y), new Position(5, 5)));

            Assert.That(ex.Message, Is.EqualTo("Player and boat must be in Land zone"));
        }

        [TestCase(20, 0)]

        [TestCase(25, 10)]
        public void Constructor_BoatInPond_ThrowsException(int x, int y)
        {
            Exception ex = Assert.Throws<InvalidPlayerPositionException>((TestDelegate) => new Game(new Position(5, 5), new Position(x, y)));

            Assert.That(ex.Message, Is.EqualTo("Player and boat must be in Land zone"));
        }

        #endregion

        #region MovePlayer
        //za kretanje igraca

        [Test]
        public void MovePlayer_ValidMove_PlayerPositionChanged()
        {
            _game.MovePlayer(Move.Right);

            Assert.That(_game.Player.Position, Is.EqualTo(new Position(6, 5)));
        }

        [Test]
        public void MovePlayer_MoveToInvalidZone_PlayerPositionUnchanged()
        {
            _game.Player.Position = new Position(0, 5);

            _game.MovePlayer(Move.Left);

            Assert.That(_game.Player.Position, Is.EqualTo(new Position(0, 5)));
        }

        [Test]
        public void MovePlayer_MoveToPondWithoutBoat_PlayerPositionUnchanged()
        {
            _game.Player.Position = new Position(19, 5);

            _game.MovePlayer(Move.Right);

            Assert.That(_game.Player.Position, Is.EqualTo(new Position(19, 5)));
        }

        [Test]
        public void MovePlayer_MoveToPondWithBoat_PlayerPositionChanged()
        {
            _game.Player.Position = new Position(19, 5);
            _game.Player.HasBoat = true;

            _game.MovePlayer(Move.Right);

            Assert.That(_game.Player.Position, Is.EqualTo(new Position(20, 5)));
        }

        #endregion


        #region ResolvePlayerPosition
        //za obrade trenutne pozicije

        [Test]
        public void ResolvePlayerPosition_PlayerOnBait_BaitIncremented()
        {
            _game.Map.AddContentToFieldOnPosition(FieldContent.Bait,_game.Player.Position);

            _game.ResolvePlayerPosition();

            Assert.That(_game.Player.AmountOfBait, Is.EqualTo(1));

        }
        [Test]
        public void ResolvePlayerPosition_PlayerOnFishAndHasBait_FishIncremented()
        {
            _game.Player.AmountOfBait = 1;

            _game.Map.Fields[5, 5].Content = FieldContent.Fish;

            _game.ResolvePlayerPosition();

            Assert.That(_game.Player.AmountOfFish, Is.EqualTo(1));
        }
        [Test]
        public void ResolvePlayerPosition_PlayerOnFishAndNoBait_FishUnchanged()
        {
            _game.Map.Fields[5, 5].Content = FieldContent.Fish;

            _game.ResolvePlayerPosition();

            Assert.That(_game.Player.AmountOfFish, Is.EqualTo(0));
        }

        [Test]
        public void ResolvePlayerPosition_PlayerOnBoat_HasBoatSetToTrue()
        {
            _game.Map.Fields[5, 5].Content = FieldContent.Boat;

            _game.ResolvePlayerPosition();

            Assert.That(_game.Player.HasBoat, Is.True);
        }

        [Test]
        public void ResolvePlayerPosition_ContentPresent_TileEmptied()
        {
            _game.Map.Fields[5, 5].Content = FieldContent.Bait;
            _game.ResolvePlayerPosition();

            Assert.That(_game.Map.Fields[5, 5].Content, Is.EqualTo(FieldContent.Empty));
        }


        #endregion

        #region CalculateIncome

        [TestCaseSource(typeof(GameTestCaseData), nameof(GameTestCaseData.CalculateIncome_TestData))]
        public void CalculateIncome_PlayerState_ReturnsExpectedScore(
            int fish,
            int bait,
            bool hasBoat,
            Game.Score expectedScore)
        {
            //provera obracuna na osnovu ovoga
            _game.Player.AmountOfFish = fish;
            _game.Player.AmountOfBait = bait;
            _game.Player.HasBoat = hasBoat;

            var result = _game.CalculateIncome();

            Assert.That(result, Is.EqualTo(expectedScore));
        }

        #endregion
    }
}
