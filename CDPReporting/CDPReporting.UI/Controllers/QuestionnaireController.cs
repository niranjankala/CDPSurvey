using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CDPReporting.UI.Models;
using CDPReporting.Business.Models;
using CDPReporting.Business.Services;
using log4net;

namespace CDPReporting.UI.Controllers
{
     [ActionAuthorize]
    public class QuestionnaireController : BaseController
    {
        private ILog _log;
        private QuestionnaireService oQuestionnaireService;

        public QuestionnaireController()
        {
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            oQuestionnaireService = new QuestionnaireService();
        }
        //
        // GET: /Questionnaire/                

        public ActionResult Index()
        {
            if (_log.IsInfoEnabled) _log.Info("Calling Index method of QuestionnaireController");
            //var model = new QuestionModelResult();
            //model.QuestionGroupList = oQuestionnaireService.GetQuestionList();                                                
            return View();
        }

        public ActionResult GetQuestionView(string questionViewId)
        {
             try
             {
                 if (_log.IsInfoEnabled) _log.Info("Calling Index method of GetQuestionView");
                 questionViewId = questionViewId.Insert(0,"_");
                 return PartialView(questionViewId);
             }
             catch(Exception ex)
             {
                 throw ex;
             }
        }

         public void SaveResponseTableType(List<QuestionResponseTableTypeModel> model)
        {
            if (_log.IsInfoEnabled) _log.Info("Calling Index method of SaveQuestionResponse");
            oQuestionnaireService.SaveResponseTableType(model);
        }

         public JsonResult GetQuestionData(string questionId)
         {
             List<QuestionResponseTableTypeModel> data = new List<QuestionResponseTableTypeModel>();
             for (int i = 1; i <= 10; i++)
             {
                 int j = 1;
                 data.Add(new QuestionResponseTableTypeModel()
                     {
                          GridIndexId =i+1, QuestionId = questionId, year = DateTime.Now.Year, 
                          GridCol1 = string.Format("Row {0}Col{1}", i, j++),
                          GridCol2 = string.Format("Row {0}Col{1}", i, j++),
                          GridCol3 = string.Format("Row {0}Col{1}", i, j++),
                          GridCol4 = string.Format("Row {0}Col{1}", i, j++)
                     });
             }
             return Json(new { data = data }, JsonRequestBehavior.AllowGet);
         }
   
     }
}