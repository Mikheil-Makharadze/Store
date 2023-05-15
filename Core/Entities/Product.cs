﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        //Relationships
        public int ProducerId { get; set; }
        public Producer Producer { get; set; }

        public ICollection<Category> Categories { get; set; }

    }
}
