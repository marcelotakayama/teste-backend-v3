using System;

using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Services;

using Xunit;

namespace TheatricalPlayersRefactoringKata.Tests
{
    public class StatementCalculatorTests
    {
        private readonly StatementCalculator _calculator = new StatementCalculator();

        [Fact]
        public void CalculateAmount_TragedyWithSmallAudience_ReturnsBaseAmount()
        {
            var play = new Play("hamlet", "Hamlet", 3950, PlayType.Tragedy);
            var performance = new Performance("hamlet", 30);

            var result = _calculator.CalculateAmount(play, performance);

            Assert.Equal(39500, result); // 3950 * 10
        }

        [Fact]
        public void CalculateAmount_TragedyWithLargeAudience_ReturnsAmountWithBonus()
        {
            var play = new Play("hamlet", "Hamlet", 3500, PlayType.Tragedy);
            var performance = new Performance("hamlet", 35);

            var result = _calculator.CalculateAmount(play, performance);

            Assert.Equal(40000, result); // 35000 + (1000 * (35-30))
        }

        [Fact]
        public void CalculateAmount_ComedyWithSmallAudience_ReturnsBaseAmount()
        {
            var play = new Play("as-like", "As You Like It", 2670, PlayType.Comedy);
            var performance = new Performance("as-like", 19);

            var result = _calculator.CalculateAmount(play, performance);

            Assert.Equal(32400, result); // (2670 * 10) + (300 * 19)
        }

        [Fact]
        public void CalculateAmount_ComedyWithLargeAudience_ReturnsAmountWithBonus()
        {
            var play = new Play("as-like", "As You Like It", 2670, PlayType.Comedy);
            var performance = new Performance("as-like", 25);

            var result = _calculator.CalculateAmount(play, performance);

            Assert.Equal(46700, result); // (2670 * 10) + (300 * 25) + 10000 + (500 * 5)
        }

        [Fact]
        public void CalculateAmount_History_ReturnsCombinedTragedyAndComedyAmount()
        {
            var play = new Play("henry-v", "Henry V", 3227, PlayType.History);
            var performance = new Performance("henry-v", 30);

            var result = _calculator.CalculateAmount(play, performance);

            var tragedyAmount = 3227 * 10;
            var comedyAmount = (3227 * 10) + (300 * 30) + 10000 + (500 * 10);
            Assert.Equal(tragedyAmount + comedyAmount, result);
        }

        [Fact]
        public void CalculateVolumeCredits_ForTragedy_ReturnsCorrectCredits()
        {
            var play = new Play("othello", "Othello", 3560, PlayType.Tragedy);
            var performance = new Performance("othello", 35);

            var result = _calculator.CalculateVolumeCredits(play, performance);

            Assert.Equal(5, result); // 35 - 30 = 5
        }

        [Fact]
        public void CalculateVolumeCredits_ForComedy_ReturnsCorrectCredits()
        {
            var play = new Play("as-like", "As You Like It", 2670, PlayType.Comedy);
            var performance = new Performance("as-like", 35);

            var result = _calculator.CalculateVolumeCredits(play, performance);

            Assert.Equal(12, result); // (35 - 30) + (35 / 5) = 5 + 7 = 12
        }

        [Fact]
        public void PlayConstructor_ShouldClampLinesBetween1000And4000()
        {
            var playBelow = new Play("1", "Play Below", 500, PlayType.Tragedy);
            var playAbove = new Play("2", "Play Above", 5000, PlayType.Comedy);
            var playNormal = new Play("3", "Play Normal", 3000, PlayType.History);

            Assert.Equal(1000, playBelow.Lines);
            Assert.Equal(4000, playAbove.Lines);
            Assert.Equal(3000, playNormal.Lines);
        }
    }
}