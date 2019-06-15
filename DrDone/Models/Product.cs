using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;

namespace DrDone.Models
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual User User { get; set; }

        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual string DescriptionShort { get { return Description.Length<100 ? Description : Description.Substring(0,100)+" ..."; }}
        public virtual string Image { get; set; }
        public virtual int Price { get; set; }
        public virtual int Status { get; set; }

        public virtual IList<Category> Category { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual DateTime? DeletedAt { get; set; }

        public virtual bool IsDeleted { get { return DeletedAt != null; } }

        public Product()
        {
            Category = new List<Category>();
        }
    }

    public class ProductMap : ClassMapping<Product>
    {
        public ProductMap() {
            Table("product");
            ManyToOne(x => x.User, x =>
            {
                x.Column("user_id");
                x.NotNullable(true);
            });
            Id(x => x.Id, x => x.Generator(Generators.Identity));
            Property(x => x.Title, x => x.NotNullable(true));
            Property(x => x.Description, x => x.NotNullable(true));
            Property(x => x.Price, x => x.NotNullable(true));
            Property(x => x.Status, x => x.NotNullable(true));
            Property(x => x.Image, x => {
                x.Column("image");
                x.NotNullable(true);
                }
            );

            Property(x => x.CreatedAt, x => {
                x.Column("created_at");
                x.NotNullable(true);
            });

            Property(x => x.UpdatedAt, x => x.Column("updated_at"));
            Property(x => x.DeletedAt, x => x.Column("deleted_at"));

            Bag(x => x.Category, x =>
             {
                 x.Key(y => y.Column("product_id"));
                 x.Table("product_category");
             }, x=>x.ManyToMany(y=>y.Column("category_id")));

        }
    }
}