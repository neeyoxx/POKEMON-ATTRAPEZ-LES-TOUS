using System;

namespace PokemonBattle
{
    public class Pokeball : IItem
    {
        public string Nom { get; set; }
        public double TauxCapture { get; set; }

        public Pokeball(double tauxCapture = 0.3)
        {
            Nom = "Pokéball";
            TauxCapture = tauxCapture;
        }

        public void Utiliser(Pokemon pokemon, Pokemon? ennemi = null)
        {
            if (ennemi == null)
            {
                Console.WriteLine("-> Impossible d'utiliser une Pokéball ici!");
                return;
            }

            Console.WriteLine($"-> {pokemon.Nom} lance une Pokéball sur {ennemi.Nom}!");

            Random rand = new Random();
            double chance = rand.NextDouble();

            if (chance < TauxCapture)
            {
                Console.WriteLine($"-> Succès! {ennemi.Nom} a été capturé!");
                ennemi.RecevoirDegats(ennemi.Pv);
            }
            else
            {
                int degatsPerte = ennemi.Pv / 4;
                ennemi.RecevoirDegats(degatsPerte);
                Console.WriteLine($"-> Échec! {ennemi.Nom} s'échappe et subit {degatsPerte} dégâts!");
            }
        }
    }
}