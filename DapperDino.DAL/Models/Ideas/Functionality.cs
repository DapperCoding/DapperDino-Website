using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DapperDino.DAL.Models.Ideas
{
    public class Functionality:IEntity
    {
        public int IdeaId { get; set; }
        [ForeignKey("IdeaId")]
        public Idea Idea { get; set; }

        public string Name { get; set; }
        public string Explanation { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Functionality Parent { get; set; }
        public IEnumerable<FunctionalityLink> Links { get; set; }
    }

    public class FunctionalityLink:LinkBase
    {
        public int FunctionalityId { get; set; }
        [ForeignKey("FunctionalityId")]
        public Functionality Functionality { get; set; }
    }

    public class FunctionalityModel : IEntity
    {
        public int FunctionalityId { get; set; }
        [ForeignKey("FunctionalityId")]
        public Functionality Functionality { get; set; }
        public string Explanation { get; set; }
        public int? InheritsFromId { get; set; }
        public FunctionalityModel InheritsFrom { get; set; }
        public IEnumerable<FunctionalityModelProperty> Properties { get; set; }
        public bool IsClass { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsInterface { get; set; }
        public bool IsStatic { get; set; }
        public Accessibility Accessibility { get; set; }
    }

    public class FunctionalityModelProperty : IEntity
    {
        public int FunctionalityModelId { get; set; }
        [ForeignKey("FunctionalityModelId")]
        public FunctionalityModel FunctionalityModel { get; set; }
        public int? FunctionalityModelTypeId { get; set; }
        public FunctionalityModel FunctionalityModelType { get; set; }
        public BaseType BaseType { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsStatic { get; set; }
        public Accessibility Accessibility { get; set; }
    }

    public enum Accessibility
    {
        Public = 0,
        Private = 1,
        Protected = 2
    }

    public enum BaseType
    {
        String = 0,
        Integer = 1,
        Double = 2,
        Class = 3,
        Interface = 4,
        Enum = 5
    }
}
