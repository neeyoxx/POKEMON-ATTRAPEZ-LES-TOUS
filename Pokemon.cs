using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonBattle
{
    public class Pokemon
    {
        public string Nom { get; set; }
        public TypePokemon Type { get; set; }
        public int Pv { get; set; } 
        public int PuissanceAttaque { get; set; }
        public readonly List<Attaque> Attaques;

        public bool EstVivant
        {
            get { return Pv > 0; }
        }

        public Pokemon(string nom, TypePokemon type, int pv, int puissanceAttaque, List<Attaque> attaques)
        {
            Nom = nom;
            Type = type;
            Pv = pv;
            PuissanceAttaque = puissanceAttaque;
            Attaques = attaques;
        }

        private double CalculerMultiplicateurDegats(TypePokemon typeAttaque, TypePokemon typeCible)
        {
            if (typeAttaque == TypePokemon.Electrique && typeCible == TypePokemon.Eau) return 2.0;
            if (typeAttaque == TypePokemon.Electrique && typeCible == TypePokemon.Plante) return 0.5;

            if (typeAttaque == TypePokemon.Plante && typeCible == TypePokemon.Eau) return 2.0;
            if (typeAttaque == TypePokemon.Plante && typeCible == TypePokemon.Feu) return 0.5;

            if (typeAttaque == TypePokemon.Eau && typeCible == TypePokemon.Feu) return 2.0;
            if (typeAttaque == TypePokemon.Eau && typeCible == TypePokemon.Plante) return 0.5;

            if (typeAttaque == TypePokemon.Feu && typeCible == TypePokemon.Plante) return 2.0;
            if (typeAttaque == TypePokemon.Feu && typeCible == TypePokemon.Eau) return 0.5;

            return 1.0;
        }

        public void Attaquer(Pokemon cible, string nomAttaque)
        {
            Attaque attaqueChoisie = Attaques.FirstOrDefault(a => a.Nom == nomAttaque);
            
            if (attaqueChoisie.Nom == null)
            {
                Console.WriteLine($"-> {Nom} ne connaît pas l'attaque {nomAttaque} !");
                return;
            }

            if (!EstVivant)
            {
                Console.WriteLine($"-> {Nom} essaie d'attaquer, mais est déjà K.O. !");
                return;
            }

            if (attaqueChoisie.Action == TypeAction.Soin)
            {
                int montantSoin = attaqueChoisie.Puissance;
                Pv += montantSoin;
                if (Pv > 100) Pv = 100;
                Console.WriteLine($"-> {Nom} utilise {attaqueChoisie.Nom} et se soigne de {montantSoin} PV.");
            }
            else
            {
                double multiplicateur = CalculerMultiplicateurDegats(this.Type, cible.Type);
                int degatsBruts = this.PuissanceAttaque + attaqueChoisie.Puissance;
                int degatsFinaux = (int)(degatsBruts * multiplicateur);

                cible.Pv -= degatsFinaux;

                if (cible.Pv < 0)
                {
                    cible.Pv = 0;
                }

                string message = $"-> {Nom} utilise {attaqueChoisie.Nom} et inflige {degatsFinaux} dégâts à {cible.Nom}";

                if (multiplicateur == 2.0)
                {
                    message += ". C'est **Super Efficace** !";
                }
                else if (multiplicateur == 0.5)
                {
                    message += ". Ce n'est **Pas très efficace** !";
                }
                else
                {
                    message += ".";
                }
                
                Console.WriteLine(message);

                if (attaqueChoisie.Action == TypeAction.Vampirisme)
                {
                    int montantSoin = degatsFinaux / 2;
                    Pv += montantSoin;
                    Console.WriteLine($"-> {Nom} récupère {montantSoin} PV grâce à l'effet de vampirisme !");
                }
            }
        }
    }
}