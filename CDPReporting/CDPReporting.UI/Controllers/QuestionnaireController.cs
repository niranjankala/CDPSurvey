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
            QuestionGroup group1 = new QuestionGroup();
            group1.QuestionGroupId = "CC0";
            group1.QuestionGroupName = "CC0. Introduction page";
            group1.Questions.Add(new Question { QuestionId = "CC01", QuestionGroupId = "CC0",  Type = QuestionType.Simple, QuestionText = "CC0.1. Please give a general description and introduction to your organization [maximum 5000 characters]."});
            group1.Questions.Add(new Question { QuestionId = "CC02", QuestionGroupId = "CC0", Type = QuestionType.DateRange, QuestionText = "CC0.2. Please state the start and end date of the year for which you are reporting data."});
            group1.Questions.Add(new Question { QuestionId = "CC03", QuestionGroupId = "CC0", Type = QuestionType.Option, QuestionText = "CC0.3. Please select the currency in which you would like to submit your response. All financial information contained in the response."});
            group1.Questions.Add(new Question { QuestionId = "CC04", QuestionGroupId = "CC0", Type = QuestionType.Option, QuestionText = "CC0.4: 	Please select if you wish to complete a shorter information request [SME questionnaire only]" });
            
            //var model = new QuestionModelResult();
            //model.QuestionGroupList = group1;
                                                
            return View(group1);
        }
	}
}