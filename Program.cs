using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace PokemonBattle
{
    class Program
    {
        static string ChoisirAttaque(Pokemon pokemon)
        {
            Console.WriteLine($"\nQuel coup {pokemon.Nom} doit-il utiliser ? (PV actuels : {pokemon.Pv})");
            
            for (int i = 0; i < pokemon.Attaques.Count; i++)
            {
                Attaque att = pokemon.Attaques[i];
                
                Console.WriteLine($"[{i + 1}] {att.Nom}"); 
            }
            
            int choix = -1;
            bool choixValide = false;

            while (!choixValide)
            {
                Console.Write("Entrez le numéro de l'attaque : ");
                string entree = Console.ReadLine();

                if (int.TryParse(entree, out choix))
                {
                    if (choix >= 1 && choix <= pokemon.Attaques.Count)
                    {
                        choixValide = true;
                    }
                    else
                    {
                        Console.WriteLine("Choix invalide. Veuillez entrer un numéro de la liste.");
                    }
                }
                else
                {
                    Console.WriteLine("Saisie invalide. Veuillez entrer un nombre.");
                }
            }
            
            return pokemon.Attaques[choix - 1].Nom;
        }

        static void Main(string[] args)
        {
            // Créer les attaques pour Pikachu
            List<Attaque> attaquesPikachu = new List<Attaque>
            {
                new Attaque("Éclair", PokemonType.Electrique, 20, TypeAction.Degats),
                new Attaque("Soin", PokemonType.Electrique, 15, TypeAction.Soin)
            };

            // Créer les attaques pour Salamèche
            List<Attaque> attaquesFlammèche = new List<Attaque>
            {
                new Attaque("Flammèche", PokemonType.Feu, 25, TypeAction.Degats),
                new Attaque("Vampirisme", PokemonType.Feu, 20, TypeAction.Vampirisme)
            };

            // Créer les Pokémon
            Pokemon pikachu = new Pokemon("Pikachu", PokemonType.Electrique, 100, 15, attaquesPikachu);
            Pokemon salamèche = new Pokemon("Salamèche", PokemonType.Feu, 100, 18, attaquesFlammèche);

            // Créer l'inventaire du joueur
            Inventory inventaire = new Inventory();
            inventaire.AddItem(new Potion(20), 3);
            inventaire.AddItem(new Pokeball(0.4), 5);

            // Lancer le combat
            Battle battle = new Battle(pikachu, salamèche, inventaire);
            battle.Start();
        }
    }
}