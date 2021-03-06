﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WPFPlantsVSZombie
{
    class Sunflower : Plant
    {
        public Sunflower() : base(1, 0, 1, "sunflower") { }

        public override Image plantImg(double left, double top)
        {
            BitmapImage image = new BitmapImage(new Uri("D:\\ProjectMVS\\WPFPlantsVSZombie\\WPFPlantsVSZombie\\bin\\Debug\\Images\\Sunflower.png", UriKind.Absolute));
            plant = new Image();

            plant.Source = image;
            plant.Height = 100;
            plant.Width = 100;

            Canvas.SetLeft(plant, left);
            curLeft = left;
            Canvas.SetTop(plant, top);
            curTop = top;
            return plant;
        }
    }
}
