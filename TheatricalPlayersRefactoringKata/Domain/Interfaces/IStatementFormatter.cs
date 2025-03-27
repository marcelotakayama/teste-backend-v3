using System.Collections.Generic;
using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.Domain.Interfaces;

public interface IStatementFormatter
{
    string Format(Invoice invoice, Dictionary<string, Play> plays, int totalAmount, int volumeCredits);
}