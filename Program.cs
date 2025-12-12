using System;
using System.Collections.Generic;

namespace PokemonBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Player joueur = new Player("Dresseur", 1000);


            List<Attaque> attaquesPikachu = new List<Attaque>
            {
                new Attaque("Éclair", PokemonType.Electrique, 20, TypeAction.Degats),
                new Attaque("Soin", PokemonType.Electrique, 20, TypeAction.Soin),
                new Attaque("Queue de Fer", PokemonType.Acier, 30, TypeAction.Degats),
                new Attaque("Fatal-Foudre", PokemonType.Electrique, 100, TypeAction.Degats, true) 
            };

            List<Attaque> attaquesFlammèche = new List<Attaque>
            {
                new Attaque("Flammèche", PokemonType.Feu, 25, TypeAction.Degats),
                new Attaque("Vampirisme", PokemonType.Feu, 20, TypeAction.Vampirisme),
                new Attaque("Nitro-Charge", PokemonType.Feu, 35, TypeAction.Degats),
                new Attaque("Déflagration", PokemonType.Feu, 100, TypeAction.Degats, true)
            };

            Pokemon pikachu = new Pokemon("Pikachu", PokemonType.Electrique, 220, 15, attaquesPikachu);
            Pokemon salamèche = new Pokemon("Salamèche", PokemonType.Feu, 220, 18, attaquesFlammèche);

            joueur.Inventaire.AddItem("Potion", 3);
            joueur.Inventaire.AddItem("Pokéball", 5);

            Console.WriteLine($"Bienvenue {joueur.Nom}!");
            joueur.AfficherArgent();
            Console.WriteLine("Appuyez sur une touche pour commencer le combat...");
            Console.ReadKey();

            Battle battle = new Battle(pikachu, salamèche, joueur);
            battle.Start();

            Console.WriteLine($"\nArgent final: {joueur.Argent}€");
        }
    }
}