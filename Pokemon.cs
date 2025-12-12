using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonBattle
{
    public class Pokemon
    {
        public const int MaxPv = 100;

        public string Nom { get; set; }
        public PokemonType Type { get; set; }
        public int Pv { get; set; } 
        public int PuissanceAttaque { get; set; }
        public readonly List<Attaque> Attaques;

        public Attaque? AttaqueEnPreparation { get; set; }

        public bool EstVivant
        {
            get { return Pv > 0; }
        }

        public Pokemon(string nom, PokemonType type, int pv, int puissanceAttaque, List<Attaque> attaques)
        {
            Nom = nom;
            Type = type;
            Pv = pv;
            PuissanceAttaque = puissanceAttaque;
            Attaques = attaques;
            AttaqueEnPreparation = null;
        }

        public void Soigner(int montant)
        {
            Pv += montant;
            if (Pv > MaxPv) Pv = MaxPv;
        }

        public void RecevoirDegats(int montant)
        {
            Pv -= montant;
            if (Pv < 0) Pv = 0;
        }

        private double CalculerMultiplicateurDegats(PokemonType typeAttaque, PokemonType typeCible)
        {

            if (typeAttaque == PokemonType.Electrique && typeCible == PokemonType.Eau) return 2.0;
            if (typeAttaque == PokemonType.Electrique && typeCible == PokemonType.Plante) return 0.5;
            if (typeAttaque == PokemonType.Plante && typeCible == PokemonType.Eau) return 2.0;
            if (typeAttaque == PokemonType.Plante && typeCible == PokemonType.Feu) return 0.5;
            if (typeAttaque == PokemonType.Eau && typeCible == PokemonType.Feu) return 2.0;
            if (typeAttaque == PokemonType.Eau && typeCible == PokemonType.Plante) return 0.5;
            if (typeAttaque == PokemonType.Feu && typeCible == PokemonType.Plante) return 2.0;
            if (typeAttaque == PokemonType.Feu && typeCible == PokemonType.Eau) return 0.5;
            return 1.0;
        }

 
        public void Attaquer(Pokemon cible, string nomAttaque = "")
        {
            Attaque? attaqueChoisie;

  
            if (AttaqueEnPreparation != null)
            {
                attaqueChoisie = AttaqueEnPreparation;
                Console.WriteLine($"-> {Nom} libère sa puissance accumulée !");
                AttaqueEnPreparation = null; 
            }
   
            else
            {
                attaqueChoisie = Attaques.FirstOrDefault(a => a.Nom == nomAttaque);
                
                if (attaqueChoisie == null) return;

                if (!EstVivant)
                {
                    Console.WriteLine($"-> {Nom} est K.O. et ne peut pas attaquer.");
                    return;
                }

     
                if (attaqueChoisie.NecessiteChargement)
                {
                    AttaqueEnPreparation = attaqueChoisie;
                    
                    if (Nom == "Pikachu")
                    {
                        Console.WriteLine($"-> {Nom} commence à charger de l'électricité... Les étincelles volent !");
                    }
                    else if (Nom == "Salamèche")
                    {
                        Console.WriteLine($"-> {Nom} commence à bouillir ! Sa queue s'enflamme intensément !");
                    }
                    else
                    {
                        Console.WriteLine($"-> {Nom} accumule de l'énergie pour {attaqueChoisie.Nom} !");
                    }
                    return; 
                }
            }

            if (attaqueChoisie.Action == TypeAction.Soin)
            {
                int montantSoin = attaqueChoisie.Puissance;
                this.Soigner(montantSoin);
                Console.WriteLine($"-> {Nom} utilise {attaqueChoisie.Nom} et se soigne de {montantSoin} PV.");
            }
            else
            {
                double multiplicateur = CalculerMultiplicateurDegats(attaqueChoisie.Type, cible.Type);
                int degatsBruts = this.PuissanceAttaque + attaqueChoisie.Puissance;
                int degatsFinaux = (int)(degatsBruts * multiplicateur);

                if (multiplicateur == 2.0) Console.WriteLine($"-> {Nom} utilise {attaqueChoisie.Nom}! C'est super efficace!");
                else if (multiplicateur == 0.5) Console.WriteLine($"-> {Nom} utilise {attaqueChoisie.Nom}... Pas très efficace...");
                else Console.WriteLine($"-> {Nom} utilise {attaqueChoisie.Nom}!");

                if (attaqueChoisie.Action == TypeAction.Vampirisme)
                {
                    int montantSoinVampire = degatsFinaux / 2;
                    this.Soigner(montantSoinVampire);
                    Console.WriteLine($"-> {Nom} absorbe {montantSoinVampire} PV!");
                }

                cible.RecevoirDegats(degatsFinaux);
                Console.WriteLine($"-> {cible.Nom} subit {degatsFinaux} dégâts! (PV: {cible.Pv}/{MaxPv})");
            }
        }
    }
}