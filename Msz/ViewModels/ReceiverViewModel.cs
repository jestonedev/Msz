using Msz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Msz.ViewModels
{
    public class ReceiverViewModel
    {
        public Receiver Receiver { get; set; }
        public List<Models.Msz> Mszs { get; set; }
        public List<Category> Categories { get; set; }
        public List<Gender> Genders { get; set; }
        public List<KinshipRelation> KinshipRelations { get; set; }
        public List<AssigmentForm> AssigmentForms { get; set; }
    }
}
