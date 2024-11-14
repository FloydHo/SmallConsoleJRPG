using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;
using SmallConsoleJRPG;

/*
 *  
 *  Idle Rendern verbessern, Schleifen aus dem Gameloop in Methoden
 *  Hardcode abändern, viel von den Chars iterieren
 *  
 *  
 *  ToDo: Combat Lsite mit Namen der Gegner ausbessern -> nutzen der Objekt List der Gegner.
 *         
 *          
 *
 * Wird super alles in Klassen umzuschreiben, vll bin ich ja masochist :')
 * 
 * 
 * 
 */



namespace SmallConsoleJRPG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(90, 35);
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CombatSystem combatSystem = new CombatSystem();

            //combatSystem.CombatStart();

            DemoLevel demoLevel = new DemoLevel();
            demoLevel.DrawMap();
        }
    }
}