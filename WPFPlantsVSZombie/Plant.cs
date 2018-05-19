using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFPlantsVSZombie
{
    abstract class Plant
    {
        public int hp, damage, income;
        public double curLeft, curTop;
        public Image plant;
        public string tag;

        public Plant(int hp, int damage, int income, string tag)
        {
            this.hp = hp;
            this.damage = damage;
            this.income = income;
            this.tag = tag;
        }

        public abstract Image plantImg(double left, double top);

        public bool isZombieOnLine(List<Zombie> zList)
        {
            bool onLine = false;
            foreach (Zombie zl in zList)
            {
                if ((zl.curTop == this.curTop) && (zl.curLeft <= (this.curLeft + 600)) && !(zl.curLeft < this.curLeft))
                {
                    onLine = true;
                }
            }
            return onLine;
        }
    }
}
