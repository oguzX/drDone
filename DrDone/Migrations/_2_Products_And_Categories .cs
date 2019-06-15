using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace DrDone.Migrations
{
    [Migration(2)]
    public class _1_Products_And_Categories : Migration
    {

        public override void Down()
        {
            Delete.Table("products");
            Delete.Table("category");
            Delete.Table("product_category");
        }

        public override void Up()
        {
            Create.Table("products")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("user_id").AsInt32().ForeignKey("users", "id")
                .WithColumn("title").AsString(128)
                .WithColumn("description").AsString(128)
                .WithColumn("image").AsString(128)
                .WithColumn("created_at").AsDateTime()
                .WithColumn("updated_at").AsDateTime().Nullable()
                .WithColumn("deleted_at").AsDateTime().Nullable()
                .WithColumn("image").AsString(45).Nullable()
                .WithColumn("price").AsInt32().Nullable()
                .WithColumn("status").AsInt32().Nullable();

            Create.Table("category")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString(128);

            Create.Table("product_category")
                .WithColumn("category_id").AsInt32().ForeignKey("category", "id").OnDelete(Rule.Cascade)
                .WithColumn("product_id").AsInt32().ForeignKey("product", "id").OnDelete(Rule.Cascade);

        }
    }
}