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

         public ActionResult GetQuestion()
        {
             try
             {

                 return View();
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
	}
}