using System;
using System.Collections.Generic;

namespace PokemonBattle
{
    public class Inventory
    {
        public List<IItem> Items { get; set; }

        public Inventory()
        {
            Items = new List<IItem>();
        }

        public void AddItem(IItem item, int quantity = 1)
        {
            for (int i = 0; i < quantity; i++)
            {
                Items.Add(item);
            }
            Console.WriteLine($"-> {item.Nom} ajouté à l'inventaire ({quantity}x)");
        }

        public void RemoveItem(IItem item)
        {
            if (Items.Contains(item))
            {
                Items.Remove(item);
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

            var groupedItems = new Dictionary<string, int>();
            foreach (var item in Items)
            {
                if (groupedItems.ContainsKey(item.Nom))
                    groupedItems[item.Nom]++;
                else
                    groupedItems[item.Nom] = 1;
            }

            foreach (var kvp in groupedItems)
            {
                Console.WriteLine($"- {kvp.Key} x{kvp.Value}");
            }
            Console.WriteLine("==================\n");
        }

        public IItem? GetItemByName(string nom)
        {
            return Items.Find(i => i.Nom == nom);
        }
    }
}