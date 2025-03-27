using System;
using System.Collections.Generic;

using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Formatters;
using TheatricalPlayersRefactoringKata.Domain.Services;

using Xunit;

namespace TheatricalPlayersRefactoringKata.Tests
{
    public class StatementFormatterTests
    {
        private readonly Dictionary<string, Play> _plays;
        private readonly Invoice _invoice;
        private readonly StatementCalculator _calculator;

        public StatementFormatterTests()
        {
            _calculator = new StatementCalculator();

            _plays = new Dictionary<string, Play>
            {
                { "hamlet", new Play("hamlet", "Hamlet", 3950, PlayType.Tragedy) },
                { "as-like", new Play("as-like", "As You Like It", 2670, PlayType.Comedy) }
            };

            _invoice = new Invoice(
                "BigCo",
                new List<Performance>
                {
                    new Performance("hamlet", 30),
                    new Performance("as-like", 20)
                });
        }

        [Fact]
        public void TextFormatter_ShouldFormatCorrectly()
        {
            var formatter = new TextStatementFormatter();
            var totalAmount = _calculator.CalculateAmount(_plays["hamlet"], _invoice.Performances[0]) +
                            _calculator.CalculateAmount(_plays["as-like"], _invoice.Performances[1]);
            var volumeCredits = _calculator.CalculateVolumeCredits(_plays["hamlet"], _invoice.Performances[0]) +
                              _calculator.CalculateVolumeCredits(_plays["as-like"], _invoice.Performances[1]);

            var result = formatter.Format(_invoice, _plays, totalAmount, volumeCredits);

            Assert.Contains("Statement for BigCo", result);
            Assert.Contains("Hamlet: $395.00 (30 seats)", result);
            Assert.Contains("As You Like It: $", result);
            Assert.Contains("Amount owed is $", result);
            Assert.Contains("You earned", result);
        }

        [Fact]
        public void XmlFormatter_ShouldGenerateValidXml()
        {
            var formatter = new XmlStatementFormatter();
            var totalAmount = _calculator.CalculateAmount(_plays["hamlet"], _invoice.Performances[0]) +
                            _calculator.CalculateAmount(_plays["as-like"], _invoice.Performances[1]);
            var volumeCredits = _calculator.CalculateVolumeCredits(_plays["hamlet"], _invoice.Performances[0]) +
                              _calculator.CalculateVolumeCredits(_plays["as-like"], _invoice.Performances[1]);

            var result = formatter.Format(_invoice, _plays, totalAmount, volumeCredits);

            Assert.Contains("<Statement", result);
            Assert.Contains("<Customer>BigCo</Customer>", result);
            Assert.Contains("<Items>", result);
            Assert.Contains("<AmountOwed>", result);
            Assert.Contains("<EarnedCredits>", result);
        }

        [Fact]
        public void FormatCurrency_ShouldFormatCorrectly()
        {
            var formatter = new TextStatementFormatter();
            var testAmount = 3950;

            var result = formatter.Format(_invoice, _plays, testAmount, 0);

            Assert.Contains("$39.50", result);
        }
    }
}