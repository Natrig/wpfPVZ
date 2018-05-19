using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPFPlantsVSZombie
{
    class Zombie
    {
        public int hp, damage, movespeed;
        public double curLeft, curTop;
        public Image zombie;
        public List<double> Tops = new List<double>();
        public Plant hittedPlant;
        public string imgType;

        public Zombie(int type)
        {
            switch(type) 
            {
                case 0:
                    this.hp = 7;
                    this.movespeed = 25;
                    this.damage = 1;
                    this.imgType = "Zombie";
                    break;
                case 1:
                    this.hp = 14;
                    this.movespeed = 15;
                    this.damage = 2;
                    this.imgType = "BigZombie";
                    break;
            }
            this.curLeft = 1000;
            double topPosition = 200;
            for (int i = 0; i < 5; i++)
            {
                Tops.Add(topPosition);
                topPosition += 100;
            }
            Random Rand = new Random();
            int listPos = Rand.Next(Tops.Count);
            this.curTop = Tops[listPos];
        }

        public Image zombieImg()
        {
            BitmapImage image = new BitmapImage(new Uri("D:\\ProjectMVS\\WPFPlantsVSZombie\\WPFPlantsVSZombie\\bin\\Debug\\Images\\" + imgType + ".png", UriKind.Absolute));

            zombie = new Image();
            zombie.Source = image;
            zombie.Height = 100;
            zombie.Width = 100;

            Canvas.SetLeft(zombie, curLeft);
            Canvas.SetTop(zombie, curTop);
            Canvas.SetZIndex(zombie, 2);

            return zombie;
        }

        public void getDamage(int damage)
        {
            this.hp -= damage;
            string damageImgType = imgType + "Damaged";
            BitmapImage image = new BitmapImage(new Uri("D:\\ProjectMVS\\WPFPlantsVSZombie\\WPFPlantsVSZombie\\bin\\Debug\\Images\\" + damageImgType + ".png", UriKind.Absolute));
            zombie.Source = image;

            DispatcherTimer gTimer = new DispatcherTimer();
            gTimer.Tick += new EventHandler(gTimer_Tick);
            gTimer.Interval = TimeSpan.FromMilliseconds(100);
            gTimer.Start();
        }

        public void standartImg()
        {
            BitmapImage image = new BitmapImage(new Uri("D:\\ProjectMVS\\WPFPlantsVSZombie\\WPFPlantsVSZombie\\bin\\Debug\\Images\\" + imgType + ".png", UriKind.Absolute));
            zombie.Source = image;
        }

        private void gTimer_Tick(object sender, EventArgs e)
        {
            this.standartImg();
        }

        public void Move()
        {
            curLeft -= this.movespeed;
            Canvas.SetLeft(zombie, curLeft);
        }

        public bool outField()
        {
            bool outField = false;
            if (this.curLeft <= 0)
                outField = true;
            return outField;
        }

        public bool Hit(List<Plant> pList)
        {
            bool hit = false;
            foreach (Plant pl in pList)
            {
                if ((pl.curTop == this.curTop) && (pl.curLeft >= (this.curLeft - 60) && !(pl.curLeft > this.curLeft)))
                {
                    hit = true;
                    hittedPlant = pl;
                }
            }
            return hit;
        }
    }
}
