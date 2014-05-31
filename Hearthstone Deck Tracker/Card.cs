﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Hearthstone_Deck_Tracker
{
    public struct Card
    {
        public string Id;
        public string PlayerClass;
        public string Rarity;
        public string Type { get; set; }
        private int? _count;
        public string Name { get; set; }
        public int Cost { get; set; }
        public int InHandCount;

        public int Height
        {
            get { return (int) (OverlayWindow.Scaling*35); }
        }
        public int PlayerWindowHeight
        {
            get { return (int)(PlayerWindow.Scaling * 35); }
        }

        public int OpponentWindowHeight
        {
            get { return (int)(OpponentWindow.Scaling * 35); }
        }


        public string GetPlayerClass
        {
            get { return PlayerClass ?? "Neutral"; }
        }

        public int Count
        {
            get { return _count ?? 1; }
            set { _count = value; }
        }

        public SolidColorBrush ColorPlayer
        {
            get
            {
                return Hearthstone.IsUsingPremade
                    ?new SolidColorBrush((InHandCount > 0 && Hearthstone.HighlightCardsInHand) ? Colors.GreenYellow :  (_count != 0) ? Colors.White : Colors.Gray)
                           : ColorEnemy;
            }
        }

       
        public SolidColorBrush ColorEnemy
        {
            get { return new SolidColorBrush(Colors.White); }
        }

        public ImageBrush Background
        {
            get
            {
                if (Id == null || Name == null)
                {
                    return new ImageBrush();
                }
                try
                {
                    string cardFileName = Name.ToLower().Replace(' ', '-').Replace(":", "").Replace("'", "-").Replace(".", "").Replace("!", "") + ".png";

                    //if (!File.Exists("Images/" + cardFileName))
                    //{
                    //    return new ImageBrush();
                    //}
                    
                   //card graphic
                    var group = new DrawingGroup();

                    if (File.Exists("Images/" + cardFileName))
                    {
                        group.Children.Add(
                            new ImageDrawing(new BitmapImage(new Uri("Images/" + cardFileName, UriKind.Relative)),
                                             new Rect(104, 0, 110, 35)));
                    }

                    //frame
                    group.Children.Add(
                        new ImageDrawing(
                            new BitmapImage(
                                new Uri(
                                    (Rarity == "Legendary")
                                        ? "Images/frame_legendary.png"
                                        : ((_count >= 2)
                                               ? "Images/frame_2.png"
                                               : "Images/frame_1.png"),
                                    UriKind.Relative)),
                            new Rect(0, 0, 218, 35)));

                    //dark overlay
                    if (_count == 0)
                    {
                        group.Children.Add(
                            new ImageDrawing(new BitmapImage(new Uri("Images/dark.png", UriKind.Relative)),
                                             new Rect(0, 0, 218, 35)));
                    }

                    var brush = new ImageBrush();
                    brush.ImageSource = new DrawingImage(group);
                    return brush;
                }
                catch (Exception)
                {
                    return new ImageBrush();
                }
            }
        }

        public override string ToString()
        {
            return Name + " (" + Count + ")";
        }

        public override bool Equals(object card)
        {
            if (!(card is Card))
                return false;
            var c = (Card) card;
            return c.Name == Name;
        }
    }
}