using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaazaTV.View.Navigation
{

    public class MasterDetailsPageMenuItem
    {
        public MasterDetailsPageMenuItem()
        {
            TargetType = typeof(MasterDetailsPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public Type TargetType { get; set; }
    }
}