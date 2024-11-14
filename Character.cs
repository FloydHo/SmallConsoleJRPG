using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallConsoleJRPG
{
    public class Character
    {
        public string name { get; set; }
        public int level { get; set; }
        public bool isDead { get; set; }
        public int maxHP { get; set; }
        public int currentHP { get; set; }
        public int maxMP { get; set; }
        public int currentMP { get; set; }
        public int strength { get; set; }
        public int defense { get; set; }
        public string weaponType { get; set; }
        public int combatPosition { set; get; }
        public List<string> items { get; set; }

        public Inventory inventory = new Inventory();

        public void UseItem(Item item)
        {
            item.Use(this);
        }
    }

    public class Inventory
    {
        public Dictionary<string, (Item item, int quantity)> items = new Dictionary<string, (Item, int)>();

        public void AddItem((string key, Item item) item, int quantity = 1)
        {
            if (items.ContainsKey(item.key))
            {
                items[item.key] = (item.item, items[item.key].quantity + quantity);
            }
            else
            {
                items[item.key] = (item.item, quantity);
            }
        }

        public void RemoveItem(string itemID)
        {
            if (items.ContainsKey(itemID))
            {
                if (items[itemID].quantity > 1)
                {
                    items[itemID] = (items[itemID].item, items[itemID].quantity - 1);
                }
                else
                {
                    items.Remove(itemID);
                }
            }
            else
            {
            }
        }

        public void ShowInventory()
        {
            foreach (var entry in items)
            {
                // reuturn Mist
            }
        }

    }



}
