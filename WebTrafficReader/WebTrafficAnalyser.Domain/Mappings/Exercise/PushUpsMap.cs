using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTrafficAnalyser.Domain.Models.Exercise;

namespace WebTrafficAnalyser.Domain.Mappings.Exercise
{
    public class PushUpsMap: ClassMapping<PushUps>
    {
        public PushUpsMap()
        {
            Id(x => x.ID, m => m.Generator(Generators.Native));
            Property(x => x.Repetitions);
            Property(x => x.Time);
        }
    }
}
