using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTrafficAnalyser.Domain.Models.Exercise
{
    public class PushUps
    {
        public virtual int ID { get; set; }
        public virtual int Repetitions { get; set; }
        public virtual DateTime Time { get; set; }
    }
}
