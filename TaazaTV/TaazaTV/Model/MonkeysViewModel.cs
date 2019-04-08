using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using TaazaTV.Helper;
using Xamarin.Forms;


namespace TaazaTV.Model
{
    class MonkeysViewModel
    {
        public ObservableCollection<BreaingNews> breaingNews { get; set; }

        public MonkeysViewModel()
        {
            breaingNews = new ObservableCollection<BreaingNews>
            {
                new BreaingNews
            {
                ImageUrl = "https://img.youtube.com/vi/DIHgTO_DtK0/hqdefault.jpg",
                Name = "Breaking News 1"
            },
                new BreaingNews
            {
                ImageUrl = "http://www.mckvie.edu.in/site/assets/files/1462/taaza_for_youtube.664x0-is.jpg",
                Name = "Breaking News 2"
                },
            new BreaingNews
            {
                ImageUrl = "http://www.qaumiektamanch.org/gallery/milestones/img013.jpg",
                Name = "Breaking News 3"

            }
            };
        }

    }
    public class BreaingNews
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Type TargetType { get; set; }
    }   
}
