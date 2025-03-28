using System;

using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;

namespace TheatricalPlayersRefactoringKata.Domain.Services
{
    public class StatementCalculator : IStatementCalculator
    {
        public int CalculateAmount(Play play, Performance perf)
        {
            var baseAmount = play.Lines * 10;
            return play.Type switch
            {
                PlayType.Tragedy => CalculateTragedyAmount(baseAmount, perf.Audience),
                PlayType.Comedy => CalculateComedyAmount(baseAmount, perf.Audience),
                PlayType.History => CalculateHistoryAmount(baseAmount, perf.Audience),
                _ => throw new Exception($"Unknown play type: {play.Type}")
            };
        }

        public int CalculateVolumeCredits(Play play, Performance perf)
        {
            var volumeCredits = Math.Max(perf.Audience - 30, 0);
            if (play.Type == PlayType.Comedy)
            {
                volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
            }
            return volumeCredits;
        }

        private int CalculateTragedyAmount(int baseAmount, int audience) =>
            audience > 30 ? baseAmount + 1000 * (audience - 30) : baseAmount;

        private int CalculateComedyAmount(int baseAmount, int audience)
        {
            var amount = baseAmount + 300 * audience;
            if (audience > 20) amount += 10000 + 500 * (audience - 20);
            return amount;
        }

        private int CalculateHistoryAmount(int baseAmount, int audience) =>
            CalculateTragedyAmount(baseAmount, audience) + CalculateComedyAmount(baseAmount, audience);
    }
}
