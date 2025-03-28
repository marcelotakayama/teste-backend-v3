using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.Domain.Interfaces
{
    public interface IStatementCalculator
    {
        int CalculateAmount(Play play, Performance perf);
        int CalculateVolumeCredits(Play play, Performance perf);
    }
}
