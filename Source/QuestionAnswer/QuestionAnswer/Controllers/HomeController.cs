using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionAnswer.Models;
using QuestionAnswer.Utilities;

namespace QuestionAnswer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog _logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            _logger.Info("########################################################################################################################");
            return View();
        }

        /// <summary>
        /// API Get List Tag
        /// </summary>
        /// <returns>List Tag</returns>
        [HttpGet]
        [Route("api/getlisttag")]
        public IActionResult GetListTag()
        {
            try
            {
                List<TagModel> lstResult = new Repository.TagRepository().GetListTag();
                if (lstResult != null && lstResult.Count > 0)
                {
                    return Ok(lstResult);
                }
                else
                {
                    return Ok(null);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(UtilFunctions.GetDetailsException(ex));
                return StatusCode(StatusCodes.Status500InternalServerError, "Request falied from getlisttag. Please try again.");
            }
        }
    }
}
