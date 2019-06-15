using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrDone.Models
{
    public class Image
    {
        public virtual int Id { get; set; }
        public virtual string Url { get; set; }
    }
    //public class ImageMap : ClassMapping<Image>
    //{
    //    public ImageMap()
    //    {
    //        Table("image");
    //        Id(x => x.Id, x => x.Generator(Generators.Identity));
    //        Property(x => x.Url, x => x.NotNullable(true));

    //        Bag(x => x.Products, x =>
    //          {
    //              x.Key(y => y.Column("images_id"));
    //              x.Table("product_image");
    //          }, x => x.ManyToMany(y => y.Column("product_id")));
    //    }
    //}
}