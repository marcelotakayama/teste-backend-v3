using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Services;
using TheatricalPlayersRefactoringKata.Domain.Interfaces;

namespace TheatricalPlayersRefactoringKata.Domain.Formatters
{
    public class TextStatementFormatter : StatementFormatter, IStatementFormatter
    {
        public string Format(Invoice invoice, Dictionary<string, Play> plays, int totalAmount, int volumeCredits)
        {
            var result = new StringBuilder();
            result.AppendLine($"Statement for {invoice.Customer}");

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayId];
                var thisAmount = CalculateAmount(play, perf);
                result.AppendLine($"  {play.Name}: {FormatCurrency(thisAmount)} ({perf.Audience} seats)");
            }

            result.AppendLine($"Amount owed is {FormatCurrency(totalAmount)}");
            result.AppendLine($"You earned {volumeCredits} credits");
            return result.ToString();
        }
    }

    public class XmlStatementFormatter : StatementFormatter, IStatementFormatter
    {
        public string Format(Invoice invoice, Dictionary<string, Play> plays, int totalAmount, int volumeCredits)
        {
            var statementXml = new XElement("Statement",
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                new XElement("Customer", invoice.Customer),
                new XElement("Items")
            );

            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayId];
                var thisAmount = CalculateAmount(play, perf) / 100.0;
                var thisCredits = CalculateVolumeCredits(play, perf);

                statementXml.Element("Items")?.Add(
                    new XElement("Item",
                        new XElement("AmountOwed", thisAmount.ToString("0.##", CultureInfo.InvariantCulture)),
                        new XElement("EarnedCredits", thisCredits),
                        new XElement("Seats", perf.Audience)
                    )
                );
            }

            statementXml.Add(
                new XElement("AmountOwed", (totalAmount / 100.0).ToString("0.##", CultureInfo.InvariantCulture)),
                new XElement("EarnedCredits", volumeCredits)
            );

            return new XDocument(new XDeclaration("1.0", "utf-8", null), statementXml).ToString();
        }
    }

    public abstract class StatementFormatter
    {
        protected string FormatCurrency(int amount)
        {
            var nfi = new CultureInfo("en-US", false).NumberFormat;
            nfi.CurrencySymbol = "$";
            nfi.CurrencyDecimalSeparator = ".";
            nfi.CurrencyGroupSeparator = ",";

            return (amount / 100.0).ToString("C", nfi);
        }

        protected int CalculateAmount(Play play, Performance perf)
        {
            var calculator = new StatementCalculator();
            return calculator.CalculateAmount(play, perf);
        }

        protected int CalculateVolumeCredits(Play play, Performance perf)
        {
            var calculator = new StatementCalculator();
            return calculator.CalculateVolumeCredits(play, perf);
        }
    }
}