using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperDino.DAL;
using DapperDino.Models;
using DapperDino.Models.FaqViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DapperDino.Controllers
{
    [Route("Faq")]
    public class FaqController : Controller
    {
        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructor(s)

        public FaqController(ApplicationDbContext context)
        {
            _context = context;

        }

        #endregion

        #region Public Methods

        [Route("Index")]
        public IActionResult Index()
        {
            var viewModel = new IndexViewModel()
            {
                FrequentlyAskedQuestions = _context.FrequentlyAskedQuestions.Include(x => x.ResourceLink).Select(x =>
                      new FrequentlyAskedQuestionViewModel()
                      {
                          Answer = x.Answer,
                          Question = x.Question,
                          Description = x.Description,
                          Id = x.Id,
                          ResourceLink = new ResourceLinkViewModel()
                          {
                              Link = x.ResourceLink.Link,
                              DisplayName = x.ResourceLink.DisplayName,
                              Id = x.ResourceLinkId
                          }
                      }).ToList()
            };

            return View(viewModel);
        }

        #endregion

        
    }
}