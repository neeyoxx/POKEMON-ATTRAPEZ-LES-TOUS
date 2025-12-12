using System;
using System.Collections.Generic;

namespace PokemonBattle
{
    public class Battle
    {
        public Pokemon PokemonJoueur { get; set; }
        public Pokemon PokemonEnnemi { get; set; }
        public Player Joueur { get; set; }
        private int Tour { get; set; }
        private string? DernierObjetUtilise { get; set; }

        public Battle(Pokemon pokemonJoueur, Pokemon pokemonEnnemi, Player joueur)
        {
            PokemonJoueur = pokemonJoueur;
            PokemonEnnemi = pokemonEnnemi;
            Joueur = joueur;
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

            if (PokemonJoueur.AttaqueEnPreparation != null)
            {
                Console.WriteLine($"-> {PokemonJoueur.Nom} finit de charger son attaque !");

                PokemonJoueur.Attaquer(PokemonEnnemi, ""); 
            }
            else
            {

                bool actionEffectuee = false;
                while (!actionEffectuee)
                {
                    int choix = AfficherMenuJoueur();
                    switch (choix)
                    {
                        case 1:
                            int indexAttaque = ChoisirAttaque();
                            if (indexAttaque >= 0 && indexAttaque < PokemonJoueur.Attaques.Count)
                            {
                                string nomAttaque = PokemonJoueur.Attaques[indexAttaque].Nom;
                                PokemonJoueur.Attaquer(PokemonEnnemi, nomAttaque);
                                actionEffectuee = true;
                            }
                            break;
                        case 2:
                            if (UtiliserObjet() && DernierObjetUtilise != "Potion") actionEffectuee = true;
                            break;
                        case 3:
                            Console.WriteLine($"-> {PokemonJoueur.Nom} s'enfuit !");
                            PokemonJoueur.Pv = 0;
                            return;
                    }
                }
            }


            if (PokemonEnnemi.EstVivant)
            {
                Console.WriteLine($"\n-> Tour de {PokemonEnnemi.Nom}...");
                
                if (PokemonEnnemi.AttaqueEnPreparation != null)
                {
                    PokemonEnnemi.Attaquer(PokemonJoueur, "");
                }
                else
                {
                    Random rand = new Random();
                    int indexAttaqueEnnemi = rand.Next(PokemonEnnemi.Attaques.Count);
                    string nomAttaqueEnnemi = PokemonEnnemi.Attaques[indexAttaqueEnnemi].Nom;
                    PokemonEnnemi.Attaquer(PokemonJoueur, nomAttaqueEnnemi);
                }
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
            Joueur.AfficherArgent();
            Console.Write("Choix (1-3): ");
            int choix = 0;
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > 3) Console.Write("Choix invalide: ");
            return choix;
        }

        private int ChoisirAttaque()
        {
            Console.WriteLine("\nAttaques disponibles:");
            for (int i = 0; i < PokemonJoueur.Attaques.Count; i++)
            {
                string info = PokemonJoueur.Attaques[i].NecessiteChargement ? " (2 Tours)" : "";
                Console.WriteLine($"{i + 1}. {PokemonJoueur.Attaques[i].Nom} (Puissance: {PokemonJoueur.Attaques[i].Puissance}){info}");
            }
            Console.Write("Choisir (1-" + PokemonJoueur.Attaques.Count + "): ");
            int choix = 0;
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > PokemonJoueur.Attaques.Count) Console.Write("Invalide: ");
            return choix - 1;
        }

        private bool UtiliserObjet()
        {
            Joueur.Inventaire.DisplayInventory();
            if (Joueur.Argent == 0) { Console.WriteLine("Pas d'argent."); return false; }
            var itemList = new List<string>();
            int index = 1;
            foreach (var nom in Joueur.Inventaire.Items.Keys) {
                int cout = ObtenirCoutObjet(nom);
                Console.WriteLine($"{index}. {nom} x{Joueur.Inventaire.Items[nom]} - {cout}€");
                itemList.Add(nom); index++;
            }
            Console.WriteLine($"{itemList.Count + 1}. Annuler");
            Console.Write("Choix: ");
            int choix = 0;
            while (!int.TryParse(Console.ReadLine(), out choix) || choix < 1 || choix > itemList.Count + 1) Console.Write("Invalide: ");
            if (choix == itemList.Count + 1) return false;

            string nomObjet = itemList[choix - 1];
            int cost = ObtenirCoutObjet(nomObjet);

            if (nomObjet == "Potion") {
                if (Joueur.Argent < cost) { Console.WriteLine("Argent insuffisant!"); return false; }
                if (PokemonJoueur.Pv == Pokemon.MaxPv) { Console.WriteLine("PV Max!"); return false; }
                PokemonJoueur.Soigner(20);
                DernierObjetUtilise = "Potion";
            }
            else if (nomObjet == "Pokéball") {
                if (Joueur.Argent < cost) { Console.WriteLine("Argent insuffisant!"); return false; }
                Random rand = new Random();
                if (rand.NextDouble() < 0.4) { Console.WriteLine("Capturé!"); PokemonEnnemi.RecevoirDegats(PokemonEnnemi.Pv); }
                else { int dmg = PokemonEnnemi.Pv/4; PokemonEnnemi.RecevoirDegats(dmg); Console.WriteLine($"Échec! Dégâts: {dmg}"); }
                DernierObjetUtilise = "Pokéball";
            }
            Joueur.DepenseArgent(cost);
            Joueur.Inventaire.RemoveItem(nomObjet, 0);
            return true;
        }

        private int ObtenirCoutObjet(string nom) => nom switch { "Potion" => 50, "Pokéball" => 100, _ => 0 };
        private void AfficherEtat() { Console.WriteLine($"{PokemonJoueur.Nom}: {PokemonJoueur.Pv} PV"); Console.WriteLine($"{PokemonEnnemi.Nom}: {PokemonEnnemi.Pv} PV"); }
        private void AfficherResultat() { 
            Console.WriteLine("\n=== RÉSULTAT ==="); 
            if (PokemonJoueur.EstVivant) { Console.WriteLine("Victoire!"); Joueur.GagneArgent(300); } 
            else Console.WriteLine("Défaite!"); 
        }
    }
}