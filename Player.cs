using System;

namespace PokemonBattle
{
    public class Player
    {
        public string Nom { get; set; }
        public int Argent { get; set; }
        public Inventory Inventaire { get; set; }

        public Player(string nom, int argentDeDepart)
        {
            Nom = nom;
            Argent = argentDeDepart;
            Inventaire = new Inventory();
        }

        public void DepenseArgent(int montant)
        {
            Argent -= montant;
            if (Argent < 0) Argent = 0;
        }

        public void GagneArgent(int montant)
        {
            Argent += montant;
            Console.WriteLine($"-> +{montant}€ ! Argent total: {Argent}€");
        }

        public void AfficherArgent()
        {
            Console.WriteLine($"Argent: {Argent}€");
        }
    }
}