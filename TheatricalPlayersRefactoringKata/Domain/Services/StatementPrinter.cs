using System.Collections.Generic;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Formatters;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;
using TheatricalPlayersRefactoringKata.Domain.Services;

public class StatementPrinter : IStatementPrinter
{
    private readonly IStatementFormatter _textFormatter;
    private readonly IStatementFormatter _xmlFormatter;
    private readonly StatementCalculator _calculator;

    public StatementPrinter(StatementCalculator calculator)
    {
        _calculator = calculator;
        _textFormatter = new TextStatementFormatter();
        _xmlFormatter = new XmlStatementFormatter();
    }

    public string Print(Invoice invoice, Dictionary<string, Play> plays)
    {
        return PrintText(invoice, plays);
    }

    public string PrintText(Invoice invoice, Dictionary<string, Play> plays)
    {
        var (totalAmount, volumeCredits) = CalculateTotals(invoice, plays);
        return _textFormatter.Format(invoice, plays, totalAmount, volumeCredits);
    }

    public string PrintXml(Invoice invoice, Dictionary<string, Play> plays)
    {
        var (totalAmount, volumeCredits) = CalculateTotals(invoice, plays);
        return _xmlFormatter.Format(invoice, plays, totalAmount, volumeCredits);
    }

    private (int totalAmount, int volumeCredits) CalculateTotals(Invoice invoice, Dictionary<string, Play> plays)
    {
        int totalAmount = 0;
        int volumeCredits = 0;

        foreach (var perf in invoice.Performances)
        {
            var play = plays[perf.PlayId];
            totalAmount += _calculator.CalculateAmount(play, perf);
            volumeCredits += _calculator.CalculateVolumeCredits(play, perf);
        }

        return (totalAmount, volumeCredits);
    }
}