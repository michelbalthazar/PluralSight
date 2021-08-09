using System.Collections.Generic;
using BethanysPieShop.Models;

namespace BethanysPieShop.ViewModels
{
    public class HomeviewModel
    {
        public IEnumerable<Pie> PiesOfTheWeek { get; set; }
    }
}
