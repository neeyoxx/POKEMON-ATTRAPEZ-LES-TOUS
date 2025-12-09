namespace PokemonBattle
{
public struct Attaque
    {
        public string Nom { get; }
        public TypeAction Action { get; }
        public int Puissance { get; }

        public Attaque(string nom, TypeAction action, int puissance)
        {
            Nom = nom;
            Action = action;
            Puissance = puissance;
        }
    }
}