using System;

namespace PokemonBattle
{
    public class Potion : IItem
    {
        public string Nom { get; set; }
        public int MontantSoin { get; set; }

        public Potion(int montantSoin = 20)
        {
            Nom = $"Potion (+{montantSoin} PV)";
            MontantSoin = montantSoin;
        }

        public void Utiliser(Pokemon pokemon, Pokemon? ennemi = null)
        {
            if (pokemon.Pv == Pokemon.MaxPv)
            {
                Console.WriteLine($"-> {pokemon.Nom} a déjà la santé au maximum!");
                return;
            }

            pokemon.Soigner(MontantSoin);
            Console.WriteLine($"-> {pokemon.Nom} utilise Potion et récupère {MontantSoin} PV!");
        }
    }
}