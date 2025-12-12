using System;

namespace PokemonBattle
{
    public class Attaque
    {
        public string Nom { get; set; }
        public PokemonType Type { get; set; }
        public int Puissance { get; set; }
        public TypeAction Action { get; set; }
        public bool NecessiteChargement { get; set; } 

        public Attaque(string nom, PokemonType type, int puissance, TypeAction action, bool necessiteChargement = false)
        {
            Nom = nom;
            Type = type;
            Puissance = puissance;
            Action = action;
            NecessiteChargement = necessiteChargement;
        }
    }
}