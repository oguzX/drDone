using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.Models
{
    public class Category
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual IList<Product> Product { get; set; } 
    }
    public class CategoryMap : ClassMapping<Category>
    {
        public CategoryMap()
        {
            Table("category");
            Id(x => x.Id, x => x.Generator(Generators.Identity));

            Property(x => x.Name, x => x.NotNullable(true));
            
            Bag(x => x.Product, x =>
            {
                x.Key(y => y.Column("category_id"));
                x.Table("product_category");
            }, x=>x.ManyToMany(y=>y.Column("product_id")));
        }
    }
}