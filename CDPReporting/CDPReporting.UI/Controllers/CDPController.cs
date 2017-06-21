using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDPReporting.UI.Models;
using CDPReporting.Business.Models;

namespace CDPReporting.UI.Controllers
{
    public class CDPController : Controller
    {
        //
        // GET: /Questionnaire/                

        public ActionResult Index()
        {           
            
            var model = new CDPQuestionModelResult();
            model.QuestionGroupList = QuestionStore.Instance.QuestionGroups.ToList();
                                                
            return View(model);
        }
	}
}