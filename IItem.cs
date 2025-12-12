using System;

namespace PokemonBattle
{
    public interface IItem
    {
        string Nom { get; set; }
        
        void Utiliser(Pokemon pokemon, Pokemon? ennemi = null);
    }
}