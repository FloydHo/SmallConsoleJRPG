using SmallConsoleJRPG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmallConsoleJRPG
{

    internal class CombatSystem
    {
        private Random rand = new Random();
        private ItemDatabase db = new ItemDatabase();
        private bool alreadyInit = false;

        // Liste der Charaktere initialisieren
        private List<Character> characters = new List<Character>
        {
            new Character
            {
                name = "Eldric",
                level = 1,
                isDead = false,
                maxHP = 100,
                currentHP = 30,
                maxMP = 80,
                currentMP = 80,
                strength = 12,
                defense = 10,
                weaponType = "Sword",
                combatPosition = 0,
                inventory = new Inventory()
            },
            new Character
            {
                name = "Aliana",
                level = 1,
                isDead = false,
                maxHP = 40,
                currentHP = 5,
                maxMP = 200,
                currentMP = 200,
                strength = 5,
                defense = 2,
                weaponType = "Staff",
                combatPosition = 1,
                inventory = new Inventory()
            },
            new Character
            {
                name = "Rowan",
                level = 1,
                isDead = false,
                maxHP = 80,
                currentHP = 20,
                maxMP = 100,
                currentMP = 100,
                strength = 10,
                defense = 4 ,
                weaponType = "Sword",
                combatPosition = 2,
                inventory = new Inventory()
            }
        };


        // Liste der Gegner initialisieren

        public List<Enemy> enemys = new List<Enemy>();

        private void addEnemies(List<Enemy> eGroup)
        {
            enemys = eGroup;
        }

        public void SetupInventories()
        {
            characters[0].inventory.AddItem(db.getItem("smallHealPotion"));
            characters[0].inventory.AddItem(db.getItem("smallHealPotion"));
            characters[0].inventory.AddItem(db.getItem("hugeHealPotion"));
            characters[0].inventory.AddItem(db.getItem("HealPotion"));
            characters[0].inventory.AddItem(db.getItem("phoenixfeder"));
            characters[1].inventory.AddItem(db.getItem("smallHealPotion"));
        }


        //Positionen auf denen Pcs und NPCs gezeichnet werden können. 
        int[,] enemyPositionsSmall =
{
                    {68, 9},
                    {68, 14 },
                    {68, 19 }
                };

        int[,] charPositions = {
                    { 7, 10 },
                    { 12, 14 },
                    { 7, 18 }
                 };

        //Animationszeug
        string[] SP_playerSwordIdle = new string[] {
                        " │ o  ",
                        " ┼/|\\ ",
                        "  / \\ "
                    };

        string[] SP_playerStaffIdle = new string[] {
                        " ┼ o  ",
                        " │/|\\ ",
                        "  / \\ "
                    };

        string[] SP_CharacterDead = new string[]
        {
                "      ",
                "      ",
                " >‾/ o"
        };

        string[][] SP_CharacterUsePotion = {
                new string[]
            {
                "Ȫ",
                "/",
                " "
            },
            new string[]
            {
                " ",
                " ",
                "\\"
            },
            };

        string[][] SP_playerMeleeAttackSword = {
            // Pattern 1
            new string[]{
                "                ",
                "                ",
                "    │ o         ",
                "    ┼/|\\        ",
                "     / \\        "
            },
            // Pattern 2
            new string[] {
                "                ",
                "                ",
                " ───┼_o         ",
                "      |\\        ",
                "     / \\        "
            },
            // Pattern 3
            new string[] {
                "                ",
                " ───┼           ",
                "     \\o         ",
                "      |\\        ",
                "     / \\        "
            },
            // Pattern 4
            new string[] {
                "                ",
                "  ───┼          ",
                "     (o         ",
                "      |\\        ",
                "     / \\        "
            },
            // Pattern 5
            new string[] {
                "    ___         ",
                "  ´     `_      ",
                " '    o   `\\    ",
                "      \\┼───     ",
                "     / \\        "
            },
            // Pattern 6
            new string[] {
                "                ",
                "                ",
                "      o         ",
                "      \\┼───     ",
                "     / \\        "
            },
            // Pattern 7
            new string[] {
                "                ",
                "                ",
                "      o         ",
                "     /┼───      ",
                "     / \\        "
            },
            // Pattern 8
            new string[] {
                "                ",
                "                ",
                "    │ o         ",
                "    ┼/|\\        ",
                "     / \\        "
            }

        };

        string[][] SP_playerMeleeAttackStaff = {
            // Pattern 1 (16 5)
            new string[] {
                " ",
                "       ",
                "    ┼ o  ",
                "    │/|\\  ",
                "     / \\   "
            },
            // Pattern 2
            new string[] {
                "    ",
                "      ",
                " ─┼──_o   ",
                "      |\\  ",
                "     / \\  "
            },
            // Pattern 3
            new string[] {
                "       ",
                " ─┼──    ",
                "     \\o   ",
                "      |\\   ",
                "     / \\    "
            },
            // Pattern 4
            new string[] {
                "         ",
                "  ─┼──    ",
                "     (o    ",
                "      |\\  ",
                "     / \\  "
            },
            // Pattern 5
            new string[] {
                "    ___   ",
                "  ´     `_  ",
                " '    o   `\\ ",
                "      \\──┼─ ",
                "     / \\    "
            },
            // Pattern 6
            new string[] {
                "          ",
                "           ",
                "      o      ",
                "      \\──┼─ ",
                "     / \\    "
            },
            // Pattern 7
            new string[] {
                "        ",
                "         ",
                "      o    ",
                "     /──┼─  ",
                "     / \\   "
            },
            // Pattern 8
            new string[] {
                "     ",
                "       ",
                "    ┼ o   ",
                "    │/|\\  ",
                "     / \\  "
            }
        };

        string[][] SP_playerMoveStaff = {
            new string[] {
                " ┼ o  ",
                " │/|\\ ",
                "   |>  "
            },
            new string[] {
                " ┼ o  ",
                " │/|\\ ",
                "   >\\ "
            },
        };

        string[][] SP_playerMoveSword = {
            new string[] {
                " │ o  ",
                " ┼/|\\ ",
                "   |>  "
            },
            new string[] {
                " │ o  ",
                " ┼/|\\ ",
                "   >\\ "
            },
        };

        string[][] SP_playerVicotry = {
                   new string[] {
                        "    o  ",
                        "   /|\\ ",
                        "   / \\ "
                    },
                   new string[] {
                        "    o  ",
                        "    |  ",
                        "   / \\ "
                    },
                   new string[] {
                        "   \\o/ ",
                        "    |  ",
                        "   / \\ "
                    },

            };


        //Start vom Gameloop
        public void CombatStart(List<Enemy> eGroup)
        {
            addEnemies(eGroup);
            if (!alreadyInit) //Inventory soll nur einmal Initailiser werden da sonst Items verdoppelt werden.
            {
                SetupInventories();
                alreadyInit = true;
            }
            bool gameover = PrintFrame();
            if (gameover)
            {
                GameOver();
                Environment.Exit(0);
            }
        }

        //Sollte eig nur den Frame erstellen, jetzt sitzt der Gameloop hier drin, mal schauen wann ich den rausnehme
        bool PrintFrame()
        {
            // Place for Solo Character x11 y16
            // Place Second Character x7 y20


            int[] Char2 = { 11, 14 };
            int[] Char3 = { 7, 10 };


            Console.ForegroundColor = ConsoleColor.Cyan;

            // Hier wird stumpf der FensterFrame geschrieben, Hintergrund, Chars und Gegner haben eigene Methoden
            //Hintergrund ist deshalb wichtig eine eigene Methode zu haben, da diese sich in Zukunft ändern können soll.
            #region Frame
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("┌────────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("├──────────────────────────────────────────────────┬─────────────────────────────────────┤");
            Console.WriteLine("│                                                  │                                     │");
            Console.WriteLine("│                                                  │                                     │");
            Console.WriteLine("│                                                  │                                     │");
            Console.WriteLine("│                                                  │                                     │");
            Console.WriteLine("│                                                  │                                     │");
            Console.WriteLine("│                                                  │                                     │");
            Console.WriteLine("│                                                  │                                     │");
            Console.WriteLine("└──────────────────────────────────────────────────┴─────────────────────────────────────┘");
            #endregion

            Console.ForegroundColor = ConsoleColor.White;

            DrawCombatBackground();     //Erstellt den Hintergrund bevor Charactere gezeichnet werden.
            bool playerTurn = false;    //Wird mit false gestartet da er sonst nurnoch nach dem Ende des Zuges des Spielers auf false gesetzt wird, würde er mit true starten entsteht kann es passieren das ein Out of Bounce entsteht wenn der Enemy anfängt
            int menuSelect = 0;         //Variable die den ausgewählten Menüeintrag des Spielers speichert.
            //string[] party = { "Eldric", "Aliana" };
            string[] party = { "Eldric", "Aliana", "Rowan" };   //Temporär wird später durch Klasseneinträge ersetzt
            string[] enemyGroup = { "Spider 1", "Zombie", "Spider 2" };     //Temporär wird später durch Klasseneinträge ersetzt 

            List<string> combinedGroup = new List<string>(party.Concat(enemyGroup).ToArray());    //Verindung der beiden temporären String Arrays damit alle am Kampfbeteiligten in einer Liste sind und eine Reihenfolge gewürfelt werden kann.

            List<List<int>> enemiesDetails = new List<List<int>> { };

            //List<(string Name, int Initative)> party = new List<(string, int)>
            //{
            //    ("Eldric", 0),
            //    ("Aliana", 0),
            //    ("Rowan", 0)
            //};

            List<string> order = RollInitaitiv(combinedGroup); //Reihenfolge wer wann dran ist

            //Idle Charactere werden gezeichnet, wird später durch eine Methode ersetzt und verlegt
            for (int i = 0; i < party.Length; i++)
            {
                DrawCharacterIdle(charPositions[i, 0], charPositions[i, 1], party[0], true);
            }

            // Combat Loop startet hier, läuft so lange bis Gegner Tod sind, die Party Tod ist oder erfolgreich geflüchtet wurde, wird dann durch ein return; beendet
            // Der Kern des ganzen
            while (true)
            {
                order = CheckOrder(order, enemys);
                foreach (string turnOf in order)
                {
                    menuSelect = 0;
                    Character pc = characters.FirstOrDefault(e => e.name == turnOf);

                    DrawCombatBackground();
                    for (int i = 0; i < party.Length; i++)
                    {
                        DrawCharacterIdle(charPositions[i, 0], charPositions[i, 1], party[i], true);
                    }

                    for (int i = 0; i < enemys.Count; i++)
                    {
                        DrawEnemyIdle(enemyPositionsSmall);
                    }

                    // Vll Chars und Enemys in eine Liste werfen und Iterieren?
                    // Besser da Tote Gegner aussortiert werden können ? 

                    if (pc != null)
                    {
                        if (!pc.isDead)
                            playerTurn = true;
                        else
                            continue;
                    }


                    DrawMainMenu(party, enemys, turnOf); //Zeichnen des Menüs im unteren Teil des Bildschirmes.

                    if (!playerTurn)
                    {
                        EnemyAttack(party, turnOf, enemyPositionsSmall[Array.IndexOf(enemyGroup, turnOf), 0], enemyPositionsSmall[Array.IndexOf(enemyGroup, turnOf), 1]);
                    }


                    while (playerTurn)
                    {
                        Character characterTurn = characters.FirstOrDefault(e => e.name == turnOf);

                        DrawBattleMenu(menuSelect);                                     //DrawBattleMenu schreibt das AuswahlFenster für den Spieler wenn ein PC am Zug ist
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.UpArrow:
                                menuSelect = (menuSelect == 0) ? 4 : menuSelect - 1;
                                break;
                            case ConsoleKey.DownArrow:
                                menuSelect = (menuSelect == 4) ? 0 : menuSelect + 1;
                                break;
                            case ConsoleKey.Enter:
                                switch (menuSelect)
                                {
                                    case 0:

                                        DeleteBattleMenu();
                                        DrawMainMenu(party, enemys, turnOf);
                                        (string name, bool cancel) choosed = ChooseEnemy(enemyPositionsSmall);
                                        if (choosed.cancel) { break; }
                                        PlayerAttack(charPositions[Array.IndexOf(party, turnOf), 0], charPositions[Array.IndexOf(party, turnOf), 1], turnOf, choosed.name);
                                        playerTurn = false;
                                        break;
                                    case 1:
                                        //characters[0].UseItem(items[0]);
                                        MessageWindow("Noch keine Funktion");
                                        break;
                                    case 3:
                                        //characters[0].UseItem(items[0]);
                                        playerTurn = DrawItemWindow(characterTurn);
                                        DrawMainMenu(party, enemys, turnOf);
                                        break;
                                    case 4:
                                        if (TryRunning())
                                        {
                                            MessageWindow("Flucht ist erfolgreich");
                                            return false;
                                        }
                                        MessageWindow("Flucht war nicht erfolgreich");
                                        playerTurn = false;
                                        break;
                                }
                                ClearInputBuffer();
                                break;
                            default:
                                break;
                        }
                    }
                    if (!IsPartyAlive())
                    {
                        DrawMainMenu(party, enemys, turnOf);
                        for (int i = 0; i < party.Length; i++)

                        // vvvvvv Immer noch nicht cool und muss weg
                        {
                            DrawCharacterIdle(charPositions[i, 0], charPositions[i, 1], party[i], true);
                        }
                        return true;
                    }
                    else if (isEnemyGroupDead())
                    {
                        DrawPcVictory();
                        return false;
                    }
                    //check if party alive GAMEOVER Scren schreiben 

                }
                if (!IsPartyAlive())
                {
                    for (int i = 0; i < party.Length; i++)

                    // vvvvvv Immer noch nicht cool und muss weg
                    {
                        DrawCharacterIdle(charPositions[i, 0], charPositions[i, 1], party[i], true);
                    }
                    return true;
                }
            }
            //return true;
        }

        // Combat Loop endet hier ====================================================================================================================


        List<string> CheckOrder(List<string> order, List<Enemy> enemiesGroup)
        {
            List<string> list = new List<string>();
            foreach (string name in order)
            {
                foreach (var enemy in enemiesGroup)
                {
                    if (enemy.Name == name)
                    {
                        if (!enemy.IsDead)
                        {
                            list.Add(enemy.Name);
                        }
                    }
                }
                foreach (var pc in characters)
                {
                    if (pc.name == name)
                    {
                        list.Add(pc.name);
                    }
                }
            }
            return list;
        }



        public void GameOver()
        {
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("┌────────────────────────────────────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│          ▄████  ▄▄▄       ███▄ ▄███▓▓█████     ▒█████   ██▒   █▓▓█████  ██▀███         │");
            Console.WriteLine("│         ██▒ ▀█▒▒████▄    ▓██▒▀█▀ ██▒▓█   ▀    ▒██▒  ██▒▓██░   █▒▓█   ▀ ▓██ ▒ ██▒       │");
            Console.WriteLine("│        ▒██░▄▄▄░▒██  ▀█▄  ▓██    ▓██░▒███      ▒██░  ██▒ ▓██  █▒░▒███   ▓██ ░▄█ ▒       │");
            Console.WriteLine("│        ░▓█  ██▓░██▄▄▄▄██ ▒██    ▒██ ▒▓█  ▄    ▒██   ██░  ▒██ █░░▒▓█  ▄ ▒██▀▀█▄         │");
            Console.WriteLine("│        ░▒▓███▀▒ ▓█   ▓██▒▒██▒   ░██▒░▒████▒   ░ ████▓▒░   ▒▀█░  ░▒████▒░██▓ ▒██▒       │");
            Console.WriteLine("│         ░▒   ▒  ▒▒   ▓▒█░░ ▒░   ░  ░░░ ▒░ ░   ░ ▒░▒░▒░    ░ ▐░  ░░ ▒░ ░░ ▒▓ ░▒▓░       │");
            Console.WriteLine("│          ░   ░   ▒   ▒▒ ░░  ░      ░ ░ ░  ░     ░ ▒ ▒░    ░ ░░   ░ ░  ░  ░▒ ░ ▒░       │");
            Console.WriteLine("│        ░ ░   ░   ░   ▒   ░      ░      ░      ░ ░ ░ ▒       ░░     ░     ░░   ░        │");
            Console.WriteLine("│              ░       ░  ░       ░      ░  ░       ░ ░        ░     ░  ░   ░            │");
            Console.WriteLine("│                                                             ░                          │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("│                                                                                        │");
            Console.WriteLine("└────────────────────────────────────────────────────────────────────────────────────────┘");
        }

        bool IsPartyAlive()
        {
            bool isAlive = false;
            foreach (var partCharacter in characters)
            {
                if (!partCharacter.isDead)
                {
                    isAlive = true;
                }
            }
            return isAlive;
        }

        bool isEnemyGroupDead()
        {
            bool isDead = true;
            foreach (Enemy enemy in enemys)
            {
                if (!enemy.IsDead)
                {
                    isDead = false;
                }
            }
            return isDead;
        }

        (string name, bool cancel) ChooseEnemy(int[,] enemyPositions)
        {
            bool cancel = false;
            int enemySelect = 0;
            bool notDone = true;
            int enemyCount = enemys.Count - 1;

            foreach (Enemy enemy in enemys)
            {
                if (!enemy.IsDead)
                {
                    enemySelect = enemy.position;
                    break;
                }
            }


            while (notDone)
            {
                DrawEnemyIdle(enemyPositions, enemySelect);
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        enemySelect = (enemySelect == 0) ? enemyCount : enemySelect - 1;
                        while (true)
                        {
                            if (enemys[enemySelect].IsDead)
                            {
                                enemySelect--;
                                if (enemySelect < 0) enemySelect = enemyCount;
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        enemySelect = (enemySelect == enemyCount) ? 0 : enemySelect + 1;
                        while (true)
                        {
                            if (enemys[enemySelect].IsDead)
                            {
                                enemySelect++;
                                if (enemySelect > enemyCount) enemySelect = 0;
                            }
                            else
                            {
                                break;
                            }
                        }
                        break;
                    case ConsoleKey.Enter:
                        DrawEnemyIdle(enemyPositions);
                        notDone = false;
                        break;
                    case ConsoleKey.Backspace:
                        DrawEnemyIdle(enemyPositions);

                        cancel = true;
                        notDone = false;
                        break;
                }
            }
            return (enemys[enemySelect].Name, cancel);
        }

        void DrawEnemyIdle(int[,] enemyPositions, int? selectedEnemy = null)
        {
            List<string> list = new List<string>();
            foreach (Enemy enemy in enemys)
            {
                if (!enemy.IsDead)
                {
                    list.Add(enemy.Name);
                }
            }

            foreach (var opponent in list)
            {
                Enemy enemy = enemys.FirstOrDefault(e => e.Name == opponent);
                if (selectedEnemy.HasValue)
                {
                    if (selectedEnemy == enemy.position)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }
                }

                for (int i = 0; i < enemy.animation.idle.GetLength(0); i++)
                {
                    Console.SetCursorPosition(enemyPositions[enemy.position, 0], enemyPositions[enemy.position, 1] + i);
                    Console.Write(enemy.animation.idle[i]);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        void DrawSingelEnemyIdleDamage(Enemy enemy, string color = "White")
        {
            Console.SetCursorPosition(enemyPositionsSmall[enemy.position, 0] - 1, enemyPositionsSmall[enemy.position, 1]);
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < enemy.animation.idle.Length; i++)
            {
                Console.SetCursorPosition(enemyPositionsSmall[enemy.position, 0] + 1, enemyPositionsSmall[enemy.position, 1] + i);
                Console.Write(enemy.animation.idle[i]);
            }
            Thread.Sleep(100);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < enemy.animation.idle.Length; i++)
            {
                Console.SetCursorPosition(enemyPositionsSmall[enemy.position, 0], enemyPositionsSmall[enemy.position, 1] + i);
                Console.Write(enemy.animation.idle[i]);
            }

        }

        // ******************************************************************************************************************************************
        // *************************************************   Drawing vom Fenster und Menüs    *****************************************************
        // ******************************************************************************************************************************************

        void DrawCombatBackground(int envir = 0, int lines = 0)
        {
            string[][] backgrounds = {
                    new string[] {
                        "|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|___",
                        "___|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|",
                        "|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|___",
                        "___|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|",
                        "|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|___",
                        "___|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|_____|",
                        "                                                                                        ",
                        "                                                       _                ‾               ",
                        "                                   ‾                                                    ",
                        "                   ‾                                             _                      ",
                        "                                                                                        ",
                        "                                             ‾                                _         ",
                        "                          ‾                           -                                 ",
                        "                                                               ‾    _                   ",
                        "                                 -           _                                          ",
                        "        _             ‾                                                                 ",
                        "                                                                                        ",
                        "                                   -                             ‾                      ",
                        "                                                                                        ",
                        "                         _              _                                               ",
                        "                                                     _                      ‾           ",
                        "          ‾                                                                             ",
                        "                                                                                        "
                    },
                };
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int i = 0; i < backgrounds[0].Length; i++)
            {
                if (i < 6)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                if (lines > 0 && lines == i)
                {
                    break;
                }
                Console.SetCursorPosition(1, 1 + i);
                Console.Write(backgrounds[0][i]);
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="party"></param>
        /// <param name="enemyGroup"></param>
        /// <param name="turnOf"></param>
        void DrawMainMenu(string[] party, List<Enemy> enemyGroup, string turnOf)
        {
            //Draw Frame
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(0, 24);
            Console.WriteLine("├──────────────────────────────────────────────────┬─────────────────────────────────────┤");
            for (int i = 0; i < 7; i++)
            {
                Console.WriteLine("│                                                  │                                     │");
            }
            Console.Write("└──────────────────────────────────────────────────┴─────────────────────────────────────┘");
            Console.ForegroundColor = ConsoleColor.White;


            for (int i = 0; i < party.Length; i++)
            {
                Character pc = characters.FirstOrDefault(e => e.name == party[i]);
                Console.SetCursorPosition(3, 26 + (i * 2));
                Console.Write($"                ");
                if (party[i] == turnOf)
                {

                    Console.SetCursorPosition(3, 26 + (i * 2));
                    Console.Write($"> {party[i]}");
                }
                else
                {
                    Console.SetCursorPosition(3, 26 + (i * 2));
                    Console.Write(party[i]);
                }
                Console.SetCursorPosition(22, 26 + (i * 2));
                Console.Write($"HP: ");
                if (pc.currentHP < (pc.maxHP / 3))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (pc.currentHP < (pc.maxHP / 2))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.Write($"{pc.currentHP}");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"/{pc.maxHP}   ");
                Console.SetCursorPosition(40, 26 + (i * 2));
                Console.Write($"MP: {pc.currentMP}");
            }
            int writeSpace = 0;
            foreach (Enemy enemy in enemyGroup)
            {

                Console.SetCursorPosition(54, 26 + writeSpace);
                Console.Write(enemy.Name);
                Console.SetCursorPosition(70, 26 + writeSpace);
                Console.Write($"{enemy.CurrentHP}  ");
                writeSpace++;
            }
        }
        void DrawEnemyStats()
        {
            for (int i = 0; i < enemys.Count; i++)
            {
                Console.SetCursorPosition(54, 26 + i);
                Console.Write(enemys[i].Name);
                Console.SetCursorPosition(70, 26 + i);
                Console.Write($"{enemys[i].CurrentHP}  ");

            }
        }

        void MessageWindow(string message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(22, 3);
            Console.Write("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓"); //46    23       22
            Console.SetCursorPosition(22, 4);
            Console.Write("┃                                            ┃");
            Console.SetCursorPosition(22, 5);
            Console.Write("┃                                            ┃");
            Console.SetCursorPosition(22, 6);
            Console.Write("┃                                            ┃");
            Console.SetCursorPosition(22, 7);
            Console.Write("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
            int x = 45 - (message.Length / 2);
            Console.SetCursorPosition(x, 5);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message);
            Thread.Sleep(800);
            DrawCombatBackground(0, 7);


        }
        void DrawBattleMenu(int selected)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            int x = 51;
            int y = 24;
            Console.SetCursorPosition(x, y);
            //First Border
            Console.Write("╥════════════════╥");
            for (int i = 1; i < 8; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("║                ║");
            }
            Console.SetCursorPosition(x, y + 8);
            Console.Write("╨════════════════╨");
            Console.ForegroundColor = ConsoleColor.White;

            string[] entries = { "Angriff", "Technik", "Magie", "Item", "Flüchten" };

            for (int i = 0; i < entries.Length; i++)
            {
                if (i == selected)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                }
                Console.SetCursorPosition(x + 5, 26 + i);
                Console.Write(entries[i]);
                Console.ForegroundColor = ConsoleColor.White;
            }

        }
        bool DrawItemWindow(Character name)
        {
            int x = 40;
            int y = 24;

            int selectedx = 0;
            int selectedy = 0;
            int selectedItemIndex = 0;
            int zeile = 0;

            bool notFinished = true;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(x, y);

            //First Border

            Console.Write("╥════════════════════════════════════════════════╣");
            for (int i = 1; i < 8; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("║                                                ║");
            }
            Console.SetCursorPosition(x, y + 8);
            Console.Write("╨════════════════════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.White;

            // Auflistung der Items
            while (notFinished)
            {

                x = 47;
                y = 26;
                int count = 0;
                string selectedItem = null;

                foreach (var item in name.inventory.items)   // muss for i werden :(
                {
                    zeile = (count % 2) * 24;
                    if (selectedy == (int)(count / 2) && selectedx == count % 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        selectedItemIndex = count;
                        selectedItem = item.Key;
                    }

                    Console.SetCursorPosition(x + zeile, y + (int)(count / 2));
                    Console.Write($"{item.Value.item.name}");
                    Console.SetCursorPosition(x + zeile - 4, y + (int)(count / 2));
                    Console.Write($"{item.Value.quantity}x");
                    count++;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                int linesOfItems = (int)name.inventory.items.Count / 2;
                int selectabley = linesOfItems;


                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedy = (selectedy == 0) ? selectabley : selectedy - 1;
                        if (count % 2 == 1 && selectedItemIndex == 1) selectedx = 0;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedy = (selectedy == selectabley) ? 0 : selectedy + 1;
                        if (count % 2 == 1 && selectedItemIndex == count - 2) selectedx = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        selectedx = (selectedx == 1) ? 0 : selectedx + 1;
                        if (count % 2 == 1 && selectedItemIndex == count - 1) { selectedx = 1; selectedy = selectabley - 1; }
                        break;
                    case ConsoleKey.LeftArrow:
                        selectedx = (selectedx == 0) ? 1 : selectedx - 1;
                        if (count % 2 == 1 && selectedItemIndex == count - 1) { selectedx = 1; selectedy = selectabley - 1; }
                        break;
                    case ConsoleKey.Enter:
                        if (selectedItem != null)
                        {
                            var temp = SelectFriendlyCharacter();
                            if (temp.Item2)
                            {
                                temp.Item1.UseItem(db.getItem(selectedItem).item);
                                DrawUsePotion(name);
                                DrawDamageNumber(db.getItem(selectedItem).item.Get(), charPositions[temp.Item1.combatPosition, 0], charPositions[temp.Item1.combatPosition, 1], false);
                                name.inventory.RemoveItem(selectedItem);
                                return false;
                            }
                        }
                        break;
                    case ConsoleKey.Backspace:
                        return true;
                    default:
                        break;
                }
            }
            return true;
        }

        void DeleteBattleMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            int x = 51;
            int y = 24;
            Console.SetCursorPosition(x, y);
            //First Border
            Console.Write("┬─────────────────");
            for (int i = 1; i < 8; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("│                 ");
            }
            Console.SetCursorPosition(x, y + 8);
            Console.Write("┴─────────────────");
            Console.ForegroundColor = ConsoleColor.White;
        }

        (Character, bool) SelectFriendlyCharacter()
        {
            int selected = 0;
            Dictionary<int, (bool, Character)> party = new Dictionary<int, (bool, Character)>();
            foreach (Character character in characters)
            {
                party.Add(character.combatPosition, (character.isDead, character));
                DrawCharacterIdle(charPositions[character.combatPosition, 0], charPositions[character.combatPosition, 1], character.name, true, character.combatPosition == selected ? "green" : "white");
            }

            while (true)
            {
                foreach (Character character in characters)
                {
                    DrawCharacterIdle(charPositions[character.combatPosition, 0], charPositions[character.combatPosition, 1], character.name, true, character.combatPosition == selected ? "green" : "white");
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.UpArrow:
                        selected = (selected == 0) ? party.Count - 1 : selected - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selected = (selected == party.Count - 1) ? 0 : selected + 1;
                        break;
                    case ConsoleKey.Enter:
                        DrawCharacterIdle(charPositions[selected, 0], charPositions[selected, 1], party[selected].Item2.name, true, "white");
                        return (party[selected].Item2, true);
                    case ConsoleKey.Backspace:
                        DrawCharacterIdle(charPositions[selected, 0], charPositions[selected, 1], party[selected].Item2.name, true, "white");
                        return (party[selected].Item2, false);
                    default:
                        break;
                }
            }
            //return (party[0].Item2, false);
        }

        //**************************************************************************************************************************************************
        //****************************************************   Animations   ******************************************************

        void DrawCharacterIdle(int x, int y, string name, bool isPlayer, string color = "white")
        {
            switch (color)
            {
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

            }
            Character pc = characters.FirstOrDefault(e => e.name == name);
            string[] animIdle;
            switch (pc.weaponType)
            {
                case "Sword":
                    animIdle = SP_playerSwordIdle;
                    break;
                case "Staff":
                    animIdle = SP_playerStaffIdle;
                    break;
                default:
                    animIdle = SP_playerSwordIdle;
                    break;
            }

            if (isPlayer)
            {
                if (!pc.isDead)
                {
                    for (int i = 0; i < animIdle.Length; i++)
                    {
                        Console.SetCursorPosition(x, y + i);
                        Console.Write(animIdle[i]);
                    }
                }
                else
                {
                    for (int i = 0; i < SP_CharacterDead.Length; i++)
                    {
                        Console.SetCursorPosition(x, y + i);
                        Console.Write(SP_CharacterDead[i]);
                    }
                }
            }
            else
            {
                for (int i = 0; i < enemys[0].animation.idle.Length; i++)
                {
                    Console.SetCursorPosition(x, y + i);
                    Console.Write(enemys[0].animation.idle[i]);
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        void DrawSingelPcIdleDamage(string name, string color = "White")
        {
            Character pc = characters.FirstOrDefault(e => e.name == name);
            string[] animIdle;
            switch (pc.weaponType)
            {
                case "Sword":
                    animIdle = SP_playerSwordIdle;
                    break;
                case "Staff":
                    animIdle = SP_playerStaffIdle;
                    break;
                default:
                    animIdle = SP_playerSwordIdle;
                    break;
            }

            Console.SetCursorPosition(charPositions[pc.combatPosition, 0] - 1, charPositions[pc.combatPosition, 1]);
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < animIdle.Length; i++)
            {
                Console.SetCursorPosition(charPositions[pc.combatPosition, 0] - 1, charPositions[pc.combatPosition, 1] + i);
                Console.Write(animIdle[i]);
            }
            Thread.Sleep(100);
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 0; i < animIdle.Length; i++)
            {
                Console.SetCursorPosition(charPositions[pc.combatPosition, 0], charPositions[pc.combatPosition, 1] + i);
                Console.Write(animIdle[i]);
            }

        }

        void DrawPcVictory()
        {
            for (int i = 0; i < 10; i++)
            {
                foreach (Character pc in characters)
                {
                    if (!pc.isDead)
                    {
                        foreach (string[] sprite in SP_playerVicotry)
                        {
                            for (int j = 0; j < sprite.Length; j++)
                            {
                                Console.SetCursorPosition(charPositions[pc.combatPosition, 0] - 1, charPositions[pc.combatPosition, 1] + j);
                                Console.Write(sprite[j]);
                                Thread.Sleep(30);
                            }
                        }
                    }
                }
            }
        }

        void DrawUsePotion(Character name)
        {
            foreach (string[] sprite in SP_CharacterUsePotion)
            {
                for (int j = 0; j < sprite.Length; j++)
                {
                    Console.SetCursorPosition(charPositions[name.combatPosition, 0] + 4, charPositions[name.combatPosition, 1] - 1 + j);
                    Console.Write(sprite[j]);
                }
                Thread.Sleep(200);
            }
        }

        List<string> RollInitaitiv(List<string> party)
        {
            List<(string Name, int Initiative)> init_order = new List<(string, int)> { };
            foreach (string member in party)
            {
                init_order.Add((member, rand.Next(101)));
            }
            init_order.Sort((x, y) => y.Initiative.CompareTo(x.Initiative));

            List<string> sortedNames = new List<string>(init_order.Select(x => x.Name).ToArray());

            return sortedNames;
        }

        void PlayerAttack(int x, int y, string attacker, string enemy = "")
        {
            Character pc = characters.FirstOrDefault(e => e.name == attacker);
            string[][] animMove;
            string[][] animAttack;
            switch (pc.weaponType) //Hier evt Enums
            {
                case "Sword": 
                    animMove = SP_playerMoveSword;
                    animAttack = SP_playerMeleeAttackSword;
                    break;
                case "Staff":
                    animMove = SP_playerMoveStaff;
                    animAttack = SP_playerMeleeAttackStaff;
                    break;
                default:
                    animMove = SP_playerMoveSword;
                    animAttack = SP_playerMeleeAttackSword;
                    break;
            }

            int distance = 6;
            Console.SetCursorPosition(x, y);
            //Animation Move Forward
            for (int i = 0; i < distance; i++)
            {
                foreach (var sprite in animMove)
                {
                    x += 1;
                    for (int j = 0; j < sprite.Length; j++)
                    {
                        Console.SetCursorPosition(x, y + j);
                        Thread.Sleep(10);
                        Console.WriteLine(sprite[j]);
                    }
                }
            }
            //Animation Hit
            foreach (var sprite in animAttack)
            {
                for (int i = 0; i < sprite.Length; i++)
                {

                    Console.SetCursorPosition(x - 3, y - 2 + i);
                    Thread.Sleep(10);
                    Console.WriteLine(sprite[i]);
                }
            }

            //Damage Calc
            EnemyTakeDamage(attacker, enemy);

            //Animation Move Back 
            for (int i = 0; i < distance; i++)
            {
                foreach (var sprite in animMove)
                {
                    for (int j = 0; j < sprite.Length; j++)
                    {
                        Console.SetCursorPosition(x, y + j);
                        Thread.Sleep(10);
                        Console.WriteLine(sprite[j]);
                    }
                    x -= 1;
                }
            }
            DrawCharacterIdle(x, y, attacker, true);
        }

        void EnemyAttack(string[] party, string attacker, int x, int y)
        {
            Enemy enemy = enemys.FirstOrDefault(e => e.Name == attacker);
            if (!enemy.IsDead)
            {
                string target;
                while (true)
                {
                    target = party[rand.Next(party.Length)];
                    if (!CheckIfTargetIsDead(target))
                    {
                        break;
                    }
                }
                int distance = enemy.animation.movementDistance;

                //Animation Move Forward
                for (int i = 0; i < distance; i++)
                {
                    foreach (var sprite in enemy.animation.movement)
                    {
                        x -= 1;
                        for (int j = 0; j < sprite.Length; j++)
                        {
                            Console.SetCursorPosition(x, y + j);
                            Thread.Sleep(10);
                            Console.WriteLine(sprite[j]);
                        }
                        Thread.Sleep(10);
                    }
                }

                //AttackAnimaion

                foreach (var sprite in enemy.animation.attack)
                {
                    for (int j = 0; j < sprite.Length; j++)
                    {
                        Console.SetCursorPosition(x, y - 1 + j);
                        Thread.Sleep(10);
                        Console.WriteLine(sprite[j]);
                    }
                    Thread.Sleep(5);
                }

                PcTakeDamage(attacker, target);

                //Animatio Move Back 
                for (int i = 0; i < distance; i++)
                {
                    foreach (var sprite in enemy.animation.movement)
                    {
                        x += 1;
                        for (int j = 0; j < sprite.Length; j++)
                        {
                            Console.SetCursorPosition(x, y + j);
                            Thread.Sleep(10);
                            Console.WriteLine(sprite[j]);
                        }
                        Thread.Sleep(10);
                    }
                }
            }
        }

        bool CheckIfTargetIsDead(string name)
        {
            Character pc = characters.FirstOrDefault(e => e.name == name);
            return pc.isDead;
        }

        void EnemyTakeDamage(string attacker, string defender)
        {
            Character pc = characters.FirstOrDefault(e => e.name == attacker);
            Enemy enemy = enemys.FirstOrDefault(e => e.Name == defender);
            double randomValue = rand.NextDouble() * (1.25 - 0.9) + 0.9;
            int dmg = (int)((double)(pc.strength - enemy.Defense) * randomValue);
            if (dmg > 0)
            {
                enemy.CurrentHP -= dmg;
                if (enemy.CurrentHP < 0) enemy.CurrentHP = 0;
                DrawSingelEnemyIdleDamage(enemy);
                DrawEnemyStats();
                DrawDamageNumber(dmg, enemyPositionsSmall[enemy.position, 0], enemyPositionsSmall[enemy.position, 1]);
            }

            else if (dmg <= 0)
            {
                dmg = 0;
                DrawDamageNumber(dmg, enemyPositionsSmall[enemy.position, 0], enemyPositionsSmall[enemy.position, 1]);
            }

            if (enemy.CurrentHP <= 0)
            {
                enemy.IsDead = true;
                DeathEnemy(enemy);
                enemy.CurrentHP = 0;
            }
        }

        void DrawDamageNumber(int dmg, int x, int y, bool isDamage = true)
        {
            if (isDamage)
                Console.ForegroundColor = ConsoleColor.Red;
            else
                Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < 2; i++)
            {
                Console.SetCursorPosition(x + 2, y - 1 - i);
                Console.Write(dmg);
                Thread.Sleep(80);
                Console.SetCursorPosition(x + 2, y - 1 - i);
                Console.Write("    ");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        void DeathEnemy(Enemy enemy)
        {
            int x = enemyPositionsSmall[enemy.position, 0];
            int y = enemyPositionsSmall[enemy.position, 1];
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    default:
                        break;
                }
                for (int j = 0; j < enemy.animation.idle.Length; j++)
                {
                    Console.SetCursorPosition(x, y + j);
                    Thread.Sleep(10);
                    Console.WriteLine(enemy.animation.idle[j]);
                }
                Thread.Sleep(100);
            }
            Console.ForegroundColor = ConsoleColor.White;

        }

        void PcTakeDamage(string attacker, string defender)
        {
            Character pc = characters.FirstOrDefault(e => e.name == defender);
            Enemy enemy = enemys.FirstOrDefault(e => e.Name == attacker);
            double randomValue = rand.NextDouble() * (1.25 - 0.9) + 0.9;
            int dmg = (int)((double)(enemy.Strength - pc.defense) * randomValue);
            if (dmg > 0)
            {
                DrawSingelPcIdleDamage(defender);
                DrawDamageNumber(dmg, charPositions[pc.combatPosition, 0], charPositions[pc.combatPosition, 1]);
                pc.currentHP -= dmg;
            }
            else if (dmg <= 0)
            {
                dmg = 0;
                DrawDamageNumber(dmg, charPositions[pc.combatPosition, 0], charPositions[pc.combatPosition, 1]);
            }
            if (pc.currentHP <= 0)
            {
                pc.isDead = true;
                pc.currentHP = 0;
                DrawCharacterIdle(charPositions[pc.combatPosition, 0], charPositions[pc.combatPosition, 1], defender, true);
            }
        }

        bool TryRunning()
        {
            bool isBoss = false;
            int maxLevelEnemy = 0;
            int maxLevelCharacter = 0;
            foreach (Character getLevel in characters)
            {
                if (getLevel.level > maxLevelCharacter) maxLevelCharacter = getLevel.level;
            }
            foreach (Enemy getLevel in enemys)
            {
                if (getLevel.level > maxLevelEnemy) maxLevelEnemy = getLevel.level;
            }
            if (rand.Next(101) > 50)
            {
                return true;
            }

            return false;
        }

        void ClearInputBuffer()
        {
            // Leert den Eingabepuffer der Konsole, damit Tastenschläge nicht beim returnen ins Menü nachträglich ausgfeührt werden.
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true); // Liest und verwirft die Tasteneingaben
            }
        }

    }
}
