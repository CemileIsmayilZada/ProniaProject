using Core.Entities;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class SliderItemRepository : Repository<SlideItem>, ISliderItemRepository
    {
        public SliderItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}
