﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Snake201806.Model
{
    /// <summary>
    /// A játékmenet logikáját tartalmazza
    /// 
    /// Ez egy osztálydefiníció, ami leírja, hogy ha létrehozok egy példányt ebből az osztályból, 
    /// akkor hogyan is kell minden példánynak működnie.
    /// Ez olyan, mint egy tervrajz.
    /// </summary>
    class Arena
    {
        private MainWindow View;
        private Snake snake;
        private DispatcherTimer pendulum;
        private bool isStarted;

        /// <summary>
        /// Konstruktorfüggvény, ő hozza létre az osztály egy-egy példányát.
        /// </summary>
        /// <param name="view">az ablak, ami létrehozta az Arena példányát</param>
        public Arena(MainWindow view)
        {
            //hivatkozva az osztálypéldányra, amiben vagyunk
            //így is el tudjuk érni az osztálypéldány osztályszintű változóját
            this.View = view;

            //A játék kezdetén megjelenítjük a játékszabályokat
            //Az osztályon belül a thid használata nem kötelező
            View.GamePlayBorder.Visibility = System.Windows.Visibility.Visible;

            snake = new Snake(10,10);

            pendulum = new DispatcherTimer(TimeSpan.FromMilliseconds(500),DispatcherPriority.Normal, 
                                            ItsTimeForDisplay, Application.Current.Dispatcher);

            isStarted = false;
        }

        private void ItsTimeForDisplay(object sender, EventArgs e)
        {

            if (!isStarted)
            { // ha nem indult el a játék, akkor ne csináljunk semmit
                return;
            }

            //meg kell jegyezni, hogy a kígyó feje hol van
            //ez egy rossz kód, mivel az eredeti példányra mutató kód referenciáját 
            //veszi át, így az eredeti példányra mutat. Ha azt változtatom, akkor ez is változik
            //var currentPosition = snake.HeadPosition;

            //új példányt hozunk létre, így elváasztjuk a jelenlegi példányra mutató referenciától
            var currentPosition = new ArenaPosition(snake.HeadPosition.RowPosition, snake.HeadPosition.ColumnPosition);

            //ki kell számolni a következő pozíciót a fej iránya alapján
            switch (snake.HeadDirection)
            {
                case SnakeHeadDirectionEnum.Up:
                    snake.HeadPosition.RowPosition = snake.HeadPosition.RowPosition - 1;
                    break;
                case SnakeHeadDirectionEnum.Down:
                    snake.HeadPosition.RowPosition = snake.HeadPosition.RowPosition + 1;
                    break;
                case SnakeHeadDirectionEnum.Left:
                    snake.HeadPosition.ColumnPosition = snake.HeadPosition.ColumnPosition - 1;
                    break;
                case SnakeHeadDirectionEnum.Right:
                    snake.HeadPosition.ColumnPosition = snake.HeadPosition.ColumnPosition + 1;
                    break;
                case SnakeHeadDirectionEnum.InPlace:
                    break;
                default:
                    break;
            }

            //ki kell rajzolni a következő pozícióra a kígyó fejét
            //Kígyófej megjelenítése Circle ikonnal
            //A grid az általa tartalmazott elemeket egy gyűjteményen keresztül teszi elérhetővé
            //ez a gyűjtemény a Children
            //a gyűjtemény egy felsorolás, ahol az első elm a 0. indexő, a második az 1. indexű, és így tovább.
            //a 10. sor 10. elemét tehát így tudjuk elkérni a gyűjteménytől
            var cell = View.ArenaGrid.Children[snake.HeadPosition.RowPosition * 20 + snake.HeadPosition.ColumnPosition];
            //viszont ez egy általános IUElement típust ad vissza, bármi, ami belekerül a gridbe
            //ilyen elemként kerül bele
            var image = (FontAwesome.WPF.ImageAwesome)cell;
            //és már el tudom érni az ikon tulajdonságot
            image.Icon = FontAwesome.WPF.FontAwesomeIcon.Circle;

            //el kell tüntetni a korábbi pozícióról.
            cell = View.ArenaGrid.Children[currentPosition.RowPosition * 20 + currentPosition.ColumnPosition];
            image = (FontAwesome.WPF.ImageAwesome)cell;
            image.Icon = FontAwesome.WPF.FontAwesomeIcon.SquareOutline;
        }

        internal void KeyDown(KeyEventArgs e)
        {
            //a játék megkezdéséhez a négy nyílbillentyű valamelyikét kell leütni.
            switch (e.Key)
            {
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                    if (!isStarted)
                    { //még nem fut, indítjuk
                        StartNewGame();
                    }

                    //Le kell kezelni a billentyűleütést
                    switch (e.Key)
                    {
                        case Key.Left:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Left;
                            break;
                        case Key.Up:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Up;
                            break;
                        case Key.Right:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Right;
                            break;
                        case Key.Down:
                            snake.HeadDirection = SnakeHeadDirectionEnum.Down;
                            break;
                    }
                    Console.WriteLine(e.Key);
                    break;
            }
        }

        private void StartNewGame()
        {
            //elindítjuk a játékot: eltüntetjük a játékszabályokat
            View.GamePlayBorder.Visibility = System.Windows.Visibility.Hidden;
            View.NumberOfMealsTextBlock.Visibility = System.Windows.Visibility.Visible;
            View.ArenaGrid.Visibility = System.Windows.Visibility.Visible;
            isStarted = true;
        }
    }
}
