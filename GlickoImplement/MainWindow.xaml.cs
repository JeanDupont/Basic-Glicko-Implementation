using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GlickoImplement
{
    /// This app is used to implement Glicko (version 1) specification
    /// Learn more here : 
    /// http://www.glicko.net/glicko/glicko.pdf
    /// http://www.glicko.net/
    /// 
    /// Glicko is a ranking system
    ///
    /// Be careful :
    /// According to Mark Glickman, "The Glicko system works best when the number of games in a rating period is moderate, say an average of 5-10 games per player in a rating period"
    /// Note that in this implementation we only do it game by game which means we don't have Rating Period and we don't update Rating Deviation with T over time


    public static class GameOutcome
    {
        public const double WIN = 1;
        public const double DRAW = 0.5;
        public const double LOSE = 0;
    }

    public class player
    {
        /// <summary>
        /// Current rating of the player
        /// </summary>
        public double R;

        /// <summary>
        /// Current Rating Deviation of the player
        /// </summary>
        public double RD;

        public void match(player two, double gameOutcome)
        {
            double R = this.R;
            double RD = this.RD;
            double R2 = two.R;
            double RD2 = two.RD;
            double outcome = gameOutcome;

            // Constant Q
            double q = 0.0057565;

            // Several lines for readability
            double TripleSquareQ = 3 * q * q;
            double SquarePi = Math.PI * Math.PI;
            double SquareRD = RD * RD;
            // G func
            double g = 1 / (Math.Sqrt(1 + TripleSquareQ * SquareRD / SquarePi));

            // E(s|r, rj, RDj ) func on only one match
            double X = 1 / (1 + Math.Pow(10,
                (-((g * (R - R2)) / 400))));

            // d² variable
            double d = 1 / (Math.Pow(q, 2) * Math.Pow(g, 2) * X * (1 - X));

            // Several lines for readability
            double second = q / ((1 / Math.Pow(RD, 2)) + (1 / d));
            // New rating of this.player
            double newR = R + second * (g * (outcome - X));

            // New rating deviation of this.player
            double newRD = Math.Sqrt(Math.Pow(1 / Math.Pow(RD, 2) + (1 / d), -1));

            // Updating this.player variables
            this.R = newR;
            // Mark Glickman says it is recommended to put a minimum limit for RD
            // When the RD gets too low, the player's rating doesn't change enough
            this.RD = newRD < 50 ? 50 : newRD;

            // Player TWO is not updated !!
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // I didn't put this in the view for the sake of lazyness
            player one = new player()
            {
                R = 1200,
                RD = 60
            };
            player two = new player()
            {
                R = 1200,
                RD = 60
            };
            
            one.match(two, GameOutcome.WIN);
            Console.WriteLine("New rating is : " + one.R);
            Console.WriteLine("New RD is : " + one.RD);

           InitializeComponent();
        }
    }
}
