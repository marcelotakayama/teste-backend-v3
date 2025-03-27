using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

using TheatricalPlayersRefactoringKata;

public class StatementPrinter
{
    public string Print(Invoice invoice, Dictionary<string, Play> plays)
    {
        return PrintText(invoice, plays);
    }

    public string PrintText(Invoice invoice, Dictionary<string, Play> plays)
    {
        var totalAmount = 0;
        var volumeCredits = 0;
        var result = $"Statement for {invoice.Customer}\n";

        foreach (var perf in invoice.Performances)
        {
            var play = plays[perf.PlayId];
            var thisAmount = CalculateAmount(play, perf);
            volumeCredits += CalculateVolumeCredits(play, perf);

            result += $"  {play.Name}: {FormatCurrency(thisAmount)} ({perf.Audience} seats)\n";
            totalAmount += thisAmount;
        }

        result += $"Amount owed is {FormatCurrency(totalAmount)}\n";
        result += $"You earned {volumeCredits} credits\n";

        return result;
    }

    public string PrintXml(Invoice invoice, Dictionary<string, Play> plays)
    {
        var totalAmount = 0;
        var volumeCredits = 0;

        var statementXml = new XElement("statement",
            new XElement("customer", invoice.Customer),
            new XElement("performances")
        );

        foreach (var perf in invoice.Performances)
        {
            var play = plays[perf.PlayId];
            var thisAmount = CalculateAmount(play, perf);
            volumeCredits += CalculateVolumeCredits(play, perf);

            statementXml.Element("performances")?.Add(
                new XElement("performance",
                    new XElement("play", play.Name),
                    new XElement("amount", thisAmount / 100),
                    new XElement("audience", perf.Audience)
                )
            );

            totalAmount += thisAmount;
        }

        statementXml.Add(
            new XElement("totalAmount", totalAmount / 100),
            new XElement("credits", volumeCredits)
        );

        return statementXml.ToString();
    }

    private int CalculateAmount(Play play, Performance perf)
    {
        var lines = play.Lines;
        if (lines < 1000) lines = 1000;
        if (lines > 4000) lines = 4000;
        var thisAmount = lines * 10;

        switch (play.Type)
        {
            case "tragedy":
                if (perf.Audience > 30)
                    thisAmount += 1000 * (perf.Audience - 30);
                break;
            case "comedy":
                if (perf.Audience > 20)
                    thisAmount += 10000 + 500 * (perf.Audience - 20);
                thisAmount += 300 * perf.Audience;
                break;
            case "history":
                var tragedyAmount = lines * 10;
                var comedyAmount = lines * 10;
                if (perf.Audience > 30)
                    tragedyAmount += 1000 * (perf.Audience - 30);
                if (perf.Audience > 20)
                    comedyAmount += 10000 + 500 * (perf.Audience - 20);
                comedyAmount += 300 * perf.Audience;
                thisAmount = tragedyAmount + comedyAmount;
                break;
            default:
                throw new Exception("unknown type: " + play.Type);
        }

        return thisAmount;
    }

    private int CalculateVolumeCredits(Play play, Performance perf)
    {
        var volumeCredits = Math.Max(perf.Audience - 30, 0);
        if (play.Type == "comedy")
            volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);
        return volumeCredits;
    }

    private string FormatCurrency(int amount)
    {
        return string.Format(CultureInfo.InvariantCulture, "${0:N2}", amount / 100.0);
    }
}
