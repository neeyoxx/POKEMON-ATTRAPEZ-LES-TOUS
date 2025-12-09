using System;

namespace PokemonBattle
{
    // Interface définissant le contrat pour tous les objets
    public interface IItem
    {
        string Nom { get; set; }
        
        // La méthode renvoie 'true' si l'objet a été utilisé avec succès, 'false' sinon.
        void Utiliser(Pokemon pokemon, Pokemon? ennemi = null);
    }
}