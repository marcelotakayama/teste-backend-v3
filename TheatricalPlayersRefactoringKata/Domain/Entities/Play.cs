using System;

using TheatricalPlayersRefactoringKata.Domain.Enums;

namespace TheatricalPlayersRefactoringKata.Domain.Entities
{
    public class Play
    {
        public string Id { get; }
        public string Name { get; }
        public int Lines { get; }
        public PlayType Type { get; }

        public Play(string id, string name, int lines, PlayType type)
        {
            Id = id;
            Name = name;
            // Força o lines a ficar entre 1000 e 4000 (Obrigatório)
            Lines = Math.Clamp(lines, 1000, 4000); 
            Type = type;
        }
    }
}