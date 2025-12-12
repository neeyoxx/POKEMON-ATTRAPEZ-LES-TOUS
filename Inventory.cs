using System;
using System.Collections.Generic;

namespace PokemonBattle
{
    public class Inventory
    {
        public Dictionary<string, int> Items { get; set; }

        public Inventory()
        {
            Items = new Dictionary<string, int>();
        }
        public void AddItem(string nomObjet, int quantity = 1)
        {
            if (Items.ContainsKey(nomObjet))
            {
                Items[nomObjet] += quantity;
            }
            else
            {
                Items[nomObjet] = quantity;
            }
            Console.WriteLine($"-> {nomObjet} x{quantity} ajouté(s) à l'inventaire.");
        }

        public void RemoveItem(string nomObjet, int quantity = 1)
        {
            if (Items.ContainsKey(nomObjet))
            {
                if (quantity <= 0) quantity = 1; 
                
                Items[nomObjet] -= quantity;
                if (Items[nomObjet] <= 0)
                {
                    Items.Remove(nomObjet);
                }
            }
        }

        public void DisplayInventory()
        {
            Console.WriteLine("\n=== INVENTAIRE ===");
            if (Items.Count == 0)
            {
                Console.WriteLine("Inventaire vide!");
                return;
            }

            int index = 1;
            foreach (var kvp in Items)
            {
                Console.WriteLine($"{index}. {kvp.Key} x{kvp.Value}");
                index++;
            }
            Console.WriteLine("==================\n");
        }
    }
}