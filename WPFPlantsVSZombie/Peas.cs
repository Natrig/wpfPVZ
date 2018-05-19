using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
namespace WPFPlantsVSZombie
{
    class Peas
    {
        public int damage, movespeed;
        public Image peas;
        public double curLeft, curTop, plantLeft, plantTop;
        public Zombie hittedZombie;

        public Peas(int damage, double left, double top)
        {
            this.damage = damage;
            this.plantLeft = left;
            this.plantTop = top;
            this.curLeft = left + 83;
            this.curTop = top + 7;
            this.movespeed = 10;
        }

        public Image peasImg()
        {
            BitmapImage image = new BitmapImage(new Uri("D:\\ProjectMVS\\WPFPlantsVSZombie\\WPFPlantsVSZombie\\bin\\Debug\\Images\\Peas.png", UriKind.Absolute));

            peas = new Image();
            peas.Source = image;
            peas.Height = 30;
            peas.Width = 30;

            Canvas.SetLeft(peas, curLeft);
            Canvas.SetTop(peas, curTop);

            return peas;
        }

        public void Move()
        {
            curLeft += movespeed;
            Canvas.SetLeft(peas, curLeft);
        }       

        public bool Hit(List<Zombie> zList)
        {
            bool hit = false;
            foreach (Zombie zl in zList)
            {
                if ((zl.curTop == this.plantTop) && (zl.curLeft <= (this.curLeft + movespeed)) && !(zl.curLeft < this.curLeft))
                {
                    hit = true;
                    hittedZombie = zl;
                }
            }
            return hit;
        }
    }
}
