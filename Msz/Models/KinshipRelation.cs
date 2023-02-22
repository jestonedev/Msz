﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.Models
{
    public class KinshipRelation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string KinshipTypeCode { get; set; }

        [Required]
        public string NameRelations { get; set; }
    }
}