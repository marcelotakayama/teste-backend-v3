using System;
using System.Collections.Generic;

using ApprovalTests;
using ApprovalTests.Reporters;

using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Services;

using Xunit;

namespace TheatricalPlayersRefactoringKata.Tests;

public class StatementPrinterTests
{
    [Fact]
    [UseReporter(typeof(DiffReporter))]
    public void TestStatementExampleLegacy()
    {
        var plays = new Dictionary<string, Play>();
        plays.Add("hamlet", new Play("hamlet", "Hamlet", 4024, PlayType.Tragedy));
        plays.Add("as-like", new Play("as-like", "As You Like It", 2670, PlayType.Comedy));
        plays.Add("othello", new Play("othello", "Othello", 3560, PlayType.Tragedy));

        Invoice invoice = new Invoice(
            "BigCo",
            new List<Performance>
            {
                new Performance("hamlet", 55),
                new Performance("as-like", 35),
                new Performance("othello", 40),
            }
        );

        var calculator = new StatementCalculator();
        var statementService = new StatementPrinter(calculator);

        var result = statementService.Print(invoice, plays);

        Approvals.Verify(result);
    }

    [Fact]
    [UseReporter(typeof(DiffReporter))]
    public void TestTextStatementExample()
    {
        var plays = new Dictionary<string, Play>();
        plays.Add("hamlet", new Play("hamlet", "Hamlet", 4024, PlayType.Tragedy));
        plays.Add("as-like", new Play("as-like", "As You Like It", 2670, PlayType.Comedy));
        plays.Add("othello", new Play("othello", "Othello", 3560, PlayType.Tragedy));
        plays.Add("henry-v", new Play("henry-v", "Henry V", 3227, PlayType.History));
        plays.Add("john", new Play("john", "King John", 2648, PlayType.History));
        plays.Add("richard-iii", new Play("richard-iii", "Richard III", 3718, PlayType.History));

        Invoice invoice = new Invoice(
            "BigCo",
            new List<Performance>
            {
                new Performance("hamlet", 55),
                new Performance("as-like", 35),
                new Performance("othello", 40),
                new Performance("henry-v", 20),
                new Performance("john", 39),
                new Performance("henry-v", 20)
            }
        );

        var calculator = new StatementCalculator();
        var statementService = new StatementPrinter(calculator);

        var result = statementService.Print(invoice, plays);

        Approvals.Verify(result);
    }

    [Fact]
    [UseReporter(typeof(DiffReporter))]
    public void TestXmlStatementExample()
    {
        var plays = new Dictionary<string, Play>
    {
        { "hamlet", new Play("hamlet", "Hamlet", 4024, PlayType.Tragedy) },
        { "as-like", new Play("as-like", "As You Like It", 2670, PlayType.Comedy) },
        { "othello", new Play("othello", "Othello", 3560, PlayType.Tragedy) },
        { "henry-v", new Play("henry-v", "Henry V", 3227, PlayType.History) },
        { "john", new Play("john", "King John", 2648, PlayType.History) },
        { "richard-iii", new Play("richard-iii", "Richard III", 3718, PlayType.History) }
    };

        Invoice invoice = new Invoice(
            "BigCo",
            new List<Performance>
            {
            new Performance("hamlet", 55),
            new Performance("as-like", 35),
            new Performance("othello", 40),
            new Performance("henry-v", 20),
            new Performance("john", 39),
            new Performance("henry-v", 20)
            }
        );

        var calculator = new StatementCalculator();
        var statementService = new StatementPrinter(calculator);

        var result = statementService.PrintXml(invoice, plays);

        Approvals.Verify(result);
    }

}