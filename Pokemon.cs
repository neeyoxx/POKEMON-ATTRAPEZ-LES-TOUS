using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonBattle
{
    public class Pokemon
    {
        // On définit le maximum de PV ici pour pouvoir le réutiliser ailleurs (ex: Potion)
        public const int MaxPv = 100;

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

        // Nouvelle méthode centralisée pour le soin
        public void Soigner(int montant)
        {
            Pv += montant;
            if (Pv > MaxPv) 
            {
                Pv = MaxPv;
            }
        }

        // Nouvelle méthode pour centraliser la gestion des dégâts
        public void RecevoirDegats(int montant)
        {
            Pv -= montant;
            if (Pv < 0)
            {
                Pv = 0;
            }
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
            
            // Vérification si l'attaque existe (si c'est une struct par défaut, Nom sera null)
            if (string.IsNullOrEmpty(attaqueChoisie.Nom))
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
                
                // Utilisation de la nouvelle méthode Soigner
                this.Soigner(montantSoin);

                Console.WriteLine($"-> {Nom} utilise {attaqueChoisie.Nom} et se soigne de {montantSoin} PV.");
            }
            else
            {
                // Calcul des dégâts
                double multiplicateur = CalculerMultiplicateurDegats(this.Type, cible.Type);
                int degatsBruts = this.PuissanceAttaque + attaqueChoisie.Puissance;
                int degatsFinaux = (int)(degatsBruts * multiplicateur);

                // Application des dégâts via la méthode dédiée
                cible.RecevoirDegats(degatsFinaux);

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

                // Gestion du Vampirisme
                if (attaqueChoisie.Action == TypeAction.Vampirisme)
                {
                    int montantSoin = degatsFinaux / 2;
                    this.Soigner(montantSoin); // Réutilisation de Soigner
                    Console.WriteLine($"-> {Nom} récupère {montantSoin} PV grâce à l'effet de vampirisme !");
                }
            }
        }
    }
}