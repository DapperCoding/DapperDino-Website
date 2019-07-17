using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperDino.Areas.Admin.Models.ProductInstructions
{
    public class ProductInstructionsEditViewModel : DAL.Models.ProductInstructions
    {
        #region Constructors
        public ProductInstructionsEditViewModel(DAL.Models.ProductInstructions instructions)
        {
            this.Description = instructions.Description;
            this.Id = instructions.Id;
            this.Name = instructions.Name;
            this.Url = instructions.Url;
        }

        public ProductInstructionsEditViewModel()
        {

        }
        #endregion

        public int ProductId { get; set; }
    }
}
