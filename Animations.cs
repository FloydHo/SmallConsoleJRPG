using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallConsoleJRPG
{

    //Sollte nochmal in seperate Files ausgelagert werden und PC Animationen gehören auch hier hin

    public abstract class Animations
    {
        public string[] idle { get; set; }
        public string[][] movement { get; set; }
        public int movementDistance { get; set; }
        public string[][] attack { get; set; }


        public abstract void InitializeAnimations();

    }

    public class SpiderSmall : Animations
    {
        public SpiderSmall()
        {
            InitializeAnimations();
        }

        public override void InitializeAnimations()
        {
            idle = new string[] {
            "   ,-, ",
            " /∞/\\\\ "
            };

            movement = new string[][]{
                new string[] {
                "   ,-, ",
                " (∞/<\\ "
                },
                new string[] {
                "   ,-, ",
                " <∞|/> "
                },
                new string[] {
                "   ,-, ",
                " /∞(|) "
                },
            };

            movementDistance = 2;

            attack = new string[][]{
                new string[] {
                "",
                "   ,-, ",
                " /∞/<\\ "
                },
                new string[] {
                "",
                " _ ,-,",
                "  ∞/|\\ "
                },
                new string[] {
                "",
                " \\ ,-, ",
                "  ∞`|\\ "
                },
                new string[] {
                "",
                " ( \\-, ",
                "  ∞ |\\ "
                },
                new string[] {
                "   ,. ",
                "  / /-,",
                " ( '\\",
                "  `'"
                },
                new string[] {
                "       ",
                "   ,-, ",
                " /∞/<\\ ",
                "       "
                },
            };
        }
    }

    public class Zombie : Animations
    {
        public Zombie()
        {
            InitializeAnimations();
        }

        public override void InitializeAnimations()
        {
            idle = new string[] {
                " {)  ",
                " //\\ ",
                " / \\ "
            };

            movement = new string[][]
            {
                    new string[] {
                        "  _{) ",
                        " ''‾) ",
                        "   /v "
                    },
                    new string[] {
                        "  _{) ",
                        " ''‾) ",
                        "   <| "
                    },
            };

            movementDistance = 3;

            attack = new string[][]{
                new string[] {
                    "     ",
                    " {)  ",
                    " //\\ ",
                    " / \\ "
                },
            };
        }
    }
}