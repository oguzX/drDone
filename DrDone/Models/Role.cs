using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.Models
{
    public class Role
    {
        public virtual int id { get; set; }
        public virtual String Name{ get; set; }
    }
    public class RoleMap: ClassMapping<Role>
    {
        public RoleMap()
        {
            Table("roles");
            Id(i => i.id, x => x.Generator(Generators.Identity));
            Property(x => x.Name, x => x.NotNullable(true));
        }
    }
}