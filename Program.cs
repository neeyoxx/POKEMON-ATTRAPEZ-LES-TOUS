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
  
            List<Attaque> attaquesPikachu = new List<Attaque>
            {
                new Attaque("Tonnerre", TypeAction.Degats, 20),
                new Attaque("Eclair", TypeAction.Degats, 10),
                new Attaque("Plénitude", TypeAction.Soin, 30),
                new Attaque("Vampirisme-Eclair", TypeAction.Vampirisme, 15)
            };

            List<Attaque> attaquesCarapuce = new List<Attaque>
            {
                new Attaque("Pistolet à O", TypeAction.Degats, 18),
                new Attaque("Hydrocanon", TypeAction.Degats, 25),
                new Attaque("Repos", TypeAction.Soin, 40),
                new Attaque("Vampirisme-Eau", TypeAction.Vampirisme, 12)
            };

   
            Pokemon monPokemon = new Pokemon("Pikachu", TypePokemon.Electrique, 100, 15, attaquesPikachu);
            Pokemon pokemonAdverse = new Pokemon("Carapuce", TypePokemon.Eau, 100, 10, attaquesCarapuce);

            int tour = 1;

            Console.WriteLine($"--- Début du combat ---");
            Console.WriteLine($"-> {monPokemon.Nom} de type {monPokemon.Type} est entré dans l'arène.");
            Console.WriteLine($"-> {pokemonAdverse.Nom} de type {pokemonAdverse.Type} est entré dans l'arène.");
            Console.WriteLine("------------------------------------------");

            while (monPokemon.EstVivant && pokemonAdverse.EstVivant)
            {
                Console.WriteLine($"\n*** TOUR {tour} ***");

                if (monPokemon.EstVivant)
                {
                    string nomAttaquePikachu = ChoisirAttaque(monPokemon);
                    
                    monPokemon.Attaquer(pokemonAdverse, nomAttaquePikachu);

                    if (pokemonAdverse.EstVivant)
                    {
                        Console.WriteLine($"-> {pokemonAdverse.Nom} a {pokemonAdverse.Pv} PV.");
                    }
                    else
                    {
                        Console.WriteLine($"-> {pokemonAdverse.Nom} est tombé K.O. !");
                        break;
                    }
                }

                
                if (pokemonAdverse.EstVivant)
                {
                    string nomAttaqueCarapuce;
                    if (pokemonAdverse.Pv < 40) 
                        nomAttaqueCarapuce = "Repos";
                    else if (monPokemon.Type == TypePokemon.Electrique)
                        nomAttaqueCarapuce = "Pistolet à O";
                    else
                        nomAttaqueCarapuce = "Hydrocanon";

                    pokemonAdverse.Attaquer(monPokemon, nomAttaqueCarapuce);
                    
                    if (monPokemon.EstVivant)
                    {
                        Console.WriteLine($"-> {monPokemon.Nom} a {monPokemon.Pv} PV.");
                    }
                    else
                    {
                        Console.WriteLine($"-> {monPokemon.Nom} est tombé K.O. !");
                    }
                }

                tour++;
                Console.WriteLine("------------------------------------------");
                Thread.Sleep(1500);
            }

            Console.WriteLine($"\n=== RÉSULTAT DU COMBAT ===");
            if (monPokemon.EstVivant)
            {
                Console.WriteLine($"-> {monPokemon.Nom} a gagné ! Félicitations, vous avez vaincu {pokemonAdverse.Nom}.");
            }
            else if (pokemonAdverse.EstVivant)
            {
                Console.WriteLine($"-> {pokemonAdverse.Nom} a gagné ! Votre {monPokemon.Nom} est K.O.");
            }
            else
            {
                Console.WriteLine("Match nul ! Les deux Pokémon sont tombés K.O. en même temps.");
            }
        }
    }
}