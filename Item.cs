using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SmallConsoleJRPG;

namespace SmallConsoleJRPG
{
    public abstract class Item
    {
        public string name { get; set; }
        public string type { get; set; }

        public Item(string name)
        {
            this.name = name;
        }

        public abstract void Use(Character target);

        public abstract int Get();
    }

    public class ItemDatabase
    {
        Dictionary<string, Item> databankUsableItems = new Dictionary<string, Item>();

        // Initialisiere die Items in der Datenbank
        public ItemDatabase()
        {
            AddItem("smallHealPotion", new HealingPotion("Kleiner Heiltrank", 20));
            AddItem("HealPotion", new HealingPotion("Heiltrank", 50));
            AddItem("hugeHealPotion", new HealingPotion("Großer Heiltrank", 100));
            AddItem("mediumHealPotion", new HealingPotion("Medium Heiltrank", 40));
            AddItem("phoenixfeder", new Revival("Phönixfeder"));
        }

        public void AddItem(string itemID, Item item)
        {
            if (!databankUsableItems.ContainsKey(itemID))
            {
                databankUsableItems.Add(itemID, item);
            }
        }

        public (string key, Item item) getItem(string key)
        {
            return (key, databankUsableItems[key]);
        }

    }

    //========================================================   Vorlagen Heal Items    =================================================================

    public class HealingPotion : Item
    {
        public int HealAmount { get; set; }

        public HealingPotion(string name, int healAmount) : base(name)
        {
            HealAmount = healAmount;
            type = "heal";
        }

        public override void Use(Character target)
        {
            if (!target.isDead)
            {
                int healedAmount = Math.Min(HealAmount, target.maxHP - target.currentHP);
                target.currentHP += healedAmount;
            }
        }

        public override int Get()
        {
            return HealAmount;
        }
    }

    public class Revival : Item
    {
        public int HealAmount { get; set; }

        public Revival(string name) : base(name)
        {
            HealAmount = 0;
            type = "heal";
        }

        public override void Use(Character target)
        {
            if (target.isDead)
            {
                int healedAmount = (int)target.maxHP / 4;
                target.currentHP += healedAmount;
                target.isDead = false;
            }
        }

        public override int Get()
        {
            return HealAmount;
        }
    }
}
