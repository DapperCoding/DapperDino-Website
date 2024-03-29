﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models
{
    public class Product:IEntity
    {
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double SalePercentage { get; set; } = 0;
        public bool IsActive { get; set; }

        public int? ProductInstructionsId { get; set; }
        [ForeignKey("ProductInstructionsId")]
        public virtual ProductInstructions Instructions { get; set; }

        public List<ProductImage> ProductImages { get; set; }
        public List<ProductProductCategory> Categories { get; set; }
        public List<ProductProductEdition> Editions { get; set; }
    }

    public class ProductImage : IEntity
    {
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public string Alt { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsHeaderImage { get; set; }
    }

    public class ProductInstructions : IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }

    public class ProductProductCategory
    {
        public int ProductId { get; set; }
        public int ProductCategoryId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }

    public class ProductProductEdition
    {
        public int ProductId { get; set; }
        public int ProductEditionId { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductEdition ProductEdition { get; set; }
    }

    public class ProductCategory:IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual ProductCategory Parent { get; set; }

        public virtual List<ProductProductCategory> Categories { get; set; }
    }

    public class ProductEdition:IEntity
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string ExtraInfo { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public virtual List<ProductProductEdition> Group { get; set; }
    }
}
