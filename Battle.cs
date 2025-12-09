using System;
using System.Collections.Generic;

namespace PokemonBattle
{
    public class Battle
    {
        public Pokemon PokemonJoueur { get; set; }
        public Pokemon PokemonEnnemi { get; set; }
        public Inventory Inventaire { get; set; }
        private int Tour { get; set; }

        public Battle(Pokemon pokemonJoueur, Pokemon pokemonEnnemi, Inventory inventaire)
        {
            PokemonJoueur = pokemonJoueur;
            PokemonEnnemi = pokemonEnnemi;
            Inventaire = inventaire;
            Tour = 0;
        }

        public void Start()
        {
            Console.WriteLine($"\n========== COMBAT ==========");
            Console.WriteLine($"{PokemonJoueur.Nom} VS {PokemonEnnemi.Nom}");
            Console.WriteLine("============================\n");

            while (PokemonJoueur.EstVivant && PokemonEnnemi.EstVivant)
            {
                ExecuteTour();
            }

            AfficherResultat();
        }

        private void ExecuteTour()
        {
            Tour++;
            Console.WriteLine($"\n--- TOUR {Tour} ---");
            AfficherEtat();

            int choix = AfficherMenuJoueur();

            switch (choix)
            {
                case 1:
                    int indexAttaque = ChoisirAttaque();
                    if (indexAttaque >= 0 && indexAttaque < PokemonJoueur.Attaques.Count)
                    {
                        string nomAttaque = PokemonJoueur.Attaques[indexAttaque].Nom;
                        PokemonJoueur.Attaquer(PokemonEnnemi, nomAttaque);
                    }
                    break;

                case 2:
                    if (!UtiliserObjet())
                        return;
                    break;

                case 3:
                    Console.WriteLine($"-> {PokemonJoueur.Nom} s'enfuit du combat!");
                    PokemonJoueur.Pv = 0;
                    return;
            }

            if (PokemonEnnemi.EstVivant)
            {
                Console.WriteLine($"\n-> {PokemonEnnemi.Nom} attaque!");
                Random rand = new Random();
                int indexAttaqueEnnemi = rand.Next(PokemonEnnemi.Attaques.Count);
                string nomAttaqueEnnemi = PokemonEnnemi.Attaques[indexAttaqueEnnemi].Nom;
                PokemonEnnemi.Attaquer(PokemonJoueur, nomAttaqueEnnemi);
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }

        private int AfficherMenuJoueur()
        {
            Console.WriteLine($"\nQue faire ?");
            Console.WriteLine("1. Attaquer");
            Console.WriteLine("2. Utiliser un objet");
            Console.WriteLine("3. Fuir");
            Console.Write("Choix (1-3): ");

            int choix = 0;
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > 3)
            {
                Console.Write("Choix invalide. Réessayez: ");
            }
            return choix;
        }

        private int ChoisirAttaque()
        {
            Console.WriteLine("\nAttaques disponibles:");
            for (int i = 0; i < PokemonJoueur.Attaques.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {PokemonJoueur.Attaques[i].Nom} (Puissance: {PokemonJoueur.Attaques[i].Puissance})");
            }
            Console.Write("Choisir une attaque (1-" + PokemonJoueur.Attaques.Count + "): ");

            int choix = 0;
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > PokemonJoueur.Attaques.Count)
            {
                Console.Write("Choix invalide. Réessayez: ");
            }
            return choix - 1;
        }

        private bool UtiliserObjet()
        {
            Inventaire.DisplayInventory();
            
            if (Inventaire.Items.Count == 0)
            {
                Console.WriteLine("-> Inventaire vide!");
                return false;
            }

            // Afficher les items avec numéros
            var groupedItems = new Dictionary<string, int>();
            var itemList = new List<string>();
            
            foreach (var item in Inventaire.Items)
            {
                if (!groupedItems.ContainsKey(item.Nom))
                {
                    groupedItems[item.Nom] = 0;
                    itemList.Add(item.Nom);
                }
                groupedItems[item.Nom]++;
            }

            Console.WriteLine("Choisissez un objet:");
            for (int i = 0; i < itemList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {itemList[i]} x{groupedItems[itemList[i]]}");
            }
            Console.WriteLine($"{itemList.Count + 1}. Annuler");

            Console.Write("Choix: ");
            int choix = 0;
            
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > itemList.Count + 1)
            {
                Console.Write("Choix invalide. Réessayez: ");
            }

            if (choix == itemList.Count + 1)
            {
                return false;
            }

            string nomObjetChoisi = itemList[choix - 1];
            IItem? objet = Inventaire.GetItemByName(nomObjetChoisi);
            
            if (objet == null)
            {
                Console.WriteLine("-> Objet non trouvé!");
                return false;
            }

            objet.Utiliser(PokemonJoueur, PokemonEnnemi);
            Inventaire.RemoveItem(objet);
            return true;
        }

        private void AfficherEtat()
        {
            Console.WriteLine($"{PokemonJoueur.Nom}: {PokemonJoueur.Pv}/{Pokemon.MaxPv} PV");
            Console.WriteLine($"{PokemonEnnemi.Nom}: {PokemonEnnemi.Pv}/{Pokemon.MaxPv} PV");
        }

        private void AfficherResultat()
        {
            Console.WriteLine("\n========== RÉSULTAT ==========");
            if (PokemonJoueur.EstVivant)
            {
                Console.WriteLine($"Victoire! {PokemonJoueur.Nom} a gagné!");
            }
            else
            {
                Console.WriteLine($"Défaite! {PokemonEnnemi.Nom} a gagné!");
            }
            Console.WriteLine("==============================\n");
        }
    }
}