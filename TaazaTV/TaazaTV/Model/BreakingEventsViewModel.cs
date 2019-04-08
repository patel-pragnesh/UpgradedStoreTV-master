using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace TaazaTV.Model
{
    class BreakingEventsViewModel
    {
        public ObservableCollection<BreaingEvents> breaingEvents { get; set; }
        public BreakingEventsViewModel()
        {

            breaingEvents = new ObservableCollection<BreaingEvents>
            {
                new BreaingEvents
            {
                ImageUrl = "https://img.youtube.com/vi/DIHgTO_DtK0/hqdefault.jpg",
                Name = "Breaking News 1"
            },
                new BreaingEvents
            {
                ImageUrl = "http://www.mckvie.edu.in/site/assets/files/1462/taaza_for_youtube.664x0-is.jpg",
                Name = "Breaking News 2"
            },
            new BreaingEvents
            {
                ImageUrl = "http://www.qaumiektamanch.org/gallery/milestones/img013.jpg",
                Name = "Breaking News 3"
            }
            };
        }
    }
    public class BreaingEvents
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Type TargetType { get; set; }
    }
}
