using System.Collections.Generic;

using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.Domain.Interfaces;

public interface IStatementPrinter
{
    string Print(Invoice invoice, Dictionary<string, Play> plays);
    string PrintText(Invoice invoice, Dictionary<string, Play> plays);
    string PrintXml(Invoice invoice, Dictionary<string, Play> plays);
}