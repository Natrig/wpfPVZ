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
using System.Windows.Threading;

namespace WPFPlantsVSZombie
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int live = 10;
        int income = 1;
        int suns = 3000;
        int toBigZombie = 0;
        int selectedPlantType = 0;
        List<Zombie> zList = new List<Zombie>();
        List<Plant> pList = new List<Plant>();
        List<Peas> psList = new List<Peas>();


        DispatcherTimer timer;
        DispatcherTimer zTimer;
        DispatcherTimer sTimer;
        DispatcherTimer gTimer;

        public MainWindow()
        {
            InitializeComponent();
            setField();

            // Подключение таймера дохода
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            // Подключение таймера зомби
            zTimer = new DispatcherTimer();
            zTimer.Tick += new EventHandler(zTimer_Tick);
            zTimer.Interval = new TimeSpan(0, 0, 3);
            zTimer.Start();

            // Подключение таймера выстрелов
            sTimer = new DispatcherTimer();
            sTimer.Tick += new EventHandler(sTimer_Tick);
            sTimer.Interval = new TimeSpan(0, 0, 3);
            sTimer.Start();

            // Таймер движения гороха
            gTimer = new DispatcherTimer();
            gTimer.Tick += new EventHandler(gTimer_Tick);
            gTimer.Interval = TimeSpan.FromMilliseconds(10);
            gTimer.Start();
        }


        // Основной таймер (1 сек)
        private void timer_Tick(object sender, EventArgs e)
        {
            suns += income;
            incomeLabel.Content = "Income per sec. : " + income;
            sunLabel.Content = "Suns: " + suns;
            liveLabel.Content = "Live: " + live;

            List<Zombie> deleteZList = new List<Zombie>();
            foreach(Zombie z in zList)
            {
                if (z.Hit(pList))
                {
                    z.hittedPlant.hp -= z.damage;
                    if (z.hittedPlant.hp <= 0)
                    {
                        FieldCanvas.Children.Remove(z.hittedPlant.plant);
                        pList.Remove(z.hittedPlant);
                        income -= z.hittedPlant.income;
                    }
                }
                else
                {
                    z.Move();
                    if (z.outField())
                    {
                        deleteZList.Add(z);
                    }
                }
            }

            foreach(Zombie z in deleteZList)
            {
                zList.Remove(z);
                FieldCanvas.Children.Remove(z.zombie);
                live--;
            }

            if (live <= 0 )
            {
                MessageBox.Show("You lose");
            }
        }

        // Таймер появления зомби
        private void zTimer_Tick(object sender, EventArgs e)
        {         
            Zombie z;
            if (toBigZombie < 10)
            {
                z = new Zombie(0);
                toBigZombie++;
            }
            else
            {
                z = new Zombie(1);
                toBigZombie = 0;
            }
            zList.Add(z);
            FieldCanvas.Children.Add(z.zombieImg());
        }

        // Таймер выстрелов
        private void sTimer_Tick(object sender, EventArgs e)
        {
            foreach(Plant p in pList)
            {
                switch(p.tag)
                {
                    case "peashooter":
                        if (p.isZombieOnLine(zList))
                        {
                            Peas Peas = new Peas(p.damage, p.curLeft, p.curTop);
                            psList.Add(Peas);
                            FieldCanvas.Children.Add(Peas.peasImg());
                        }
                        break;
                }
            }
        }

        // Таймер движения гороха
        private void gTimer_Tick(object sender, EventArgs e)
        {
            List<Peas> toDeletePeas = new List<Peas>();
            foreach(Peas peas in psList)
            {
                if (peas.Hit(zList))
                {
                    peas.hittedZombie.getDamage(peas.damage);                                   
                    if (peas.hittedZombie.hp == 0)
                    {
                        FieldCanvas.Children.Remove(peas.hittedZombie.zombie);
                        zList.Remove(peas.hittedZombie);
                        suns += 30;
                    }
                    FieldCanvas.Children.Remove(peas.peas);
                    toDeletePeas.Add(peas);
                }
                else
                {
                    if (peas.curLeft >= 1000)
                    {
                        toDeletePeas.Add(peas);
                        FieldCanvas.Children.Remove(peas.peas);
                    }
                    peas.Move();
                }
            }

            foreach (Peas peas in toDeletePeas)
                psList.Remove(peas);
        }

        private void setField()
        {
            int left = 0;
            int top = 200;
            int colorType = 0;
            for(int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Name = "Rect" + i + "x" + j;
                    rect.Height = 100;
                    rect.Width = 100;
                    rect.Stroke = Brushes.Black;
                    rect.MouseDown += Rect_MouseDown;
                    rect.Tag = i + "x" + j;
                    Canvas.SetLeft(rect, left);
                    Canvas.SetTop(rect, top);
               
                    if (colorType == 0)
                    {
                        rect.Fill = Brushes.Green;
                        colorType = 1;
                    }
                    else
                    {
                        rect.Fill = Brushes.LightGreen;
                        colorType = 0;
                    }
                    FieldCanvas.Children.Add(rect);

                    left += 100;                   
                }
                top += 100;
                left = 0;
            }
        }

        private void Rect_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            double left, top;
            left = Canvas.GetLeft(rect);
            top = Canvas.GetTop(rect);

            switch (selectedPlantType)
            {
                case 0:
                    if (suns >= 150)
                    {
                        Sunflower sf = new Sunflower();
                        FieldCanvas.Children.Add(sf.plantImg(left, top));
                        suns -= 150;
                        this.income++;
                        pList.Add(sf);
                    }
                    break;
                case 1:
                    if (suns >= 150)
                    {
                        Peashooter ps = new Peashooter();
                        FieldCanvas.Children.Add(ps.plantImg(left, top));
                        suns -= 150;
                        pList.Add(ps);
                    }
                    break;
                case 2:
                    if (suns >= 200)
                    {
                        Nutwall nw = new Nutwall();
                        FieldCanvas.Children.Add(nw.plantImg(left, top));
                        suns -= 200;
                        pList.Add(nw);
                    }
                    break;
            }
        }

        private void plantButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = (Image)sender;

            switch(img.Tag.ToString())
            {
                case "Sunflower": selectedPlantType = 0; break;
                case "Peashooter": selectedPlantType = 1; break;
                case "Nutwall": selectedPlantType = 2; break;
            }
        }
    }
}
