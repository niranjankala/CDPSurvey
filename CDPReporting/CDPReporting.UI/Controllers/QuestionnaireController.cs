using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDPReporting.UI.Models;

namespace CDPReporting.UI.Controllers
{
    public class QuestionnaireController : Controller
    {
        //
        // GET: /Questionnaire/                

        public ActionResult Index()
        {           
            
            var model = new QuestionModelResult();
            model.QuestionGroupList = QuestionStore.Instance.QuestionGroups.ToList();
                                                
            return View(model);
        }
	}
}