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
            List<GroupQuestionModel> oSidePanelQuestions = new List<GroupQuestionModel>();
            if (_log.IsInfoEnabled) _log.Info("Calling Index method of QuestionnaireController");


            oSidePanelQuestions = oQuestionnaireService.GetQuestionList();

            // return View("_ViewQuestionList", oSidePanelQuestions);


            //var model = new QuestionModelResult();
            //  var model.QuestionGroupList = oQuestionnaireService.GetQuestionList();                                                
            return View(oSidePanelQuestions);
        }

        public ActionResult GetQuestionView(Guid questionViewId)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling Index method of GetQuestionView");
               // Guid questionId = questionViewId.Replace("Question_", "").Replace("_", ".");
                //questionViewId = questionViewId.Insert(0, "_");
                var model = oQuestionnaireService.GetQuestionResponse(questionViewId, CurrentUser.UserId);
                return PartialView(questionViewId.ToString(), model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public void SaveQuestionResponse(Guid questionId, string answer, string questionType, int year)
        {
            try
            {
                if (_log.IsInfoEnabled) _log.Info("Calling Index method of GetQuestionView");
                QuestionResponseModel response = new QuestionResponseModel();
                response.QuestionId = questionId;
                response.Value = answer;
                response.Year = year;
                response.QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), questionType);
                oQuestionnaireService.SaveQuestionResponse(response, CurrentUser.UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveResponseTableType(List<QuestionResponseTableTypeModel> model, Guid questionId)
        {
            if (_log.IsInfoEnabled) _log.Info("Calling Index method of SaveQuestionResponse");
            oQuestionnaireService.SaveResponseTableType(model, questionId, CurrentUser.UserId);
        }

        public JsonResult GetQuestionData(Guid questionId)
        {
            List<QuestionResponseTableTypeModel> data = new List<QuestionResponseTableTypeModel>();
            //for (int i = 1; i <= 10; i++)
            //{
            //    int j = 1;
            //    data.Add(new QuestionResponseTableTypeModel()
            //        {
            //             GridIndexId =i+1, QuestionId = questionId, year = DateTime.Now.Year, 
            //             GridCol1 = string.Format("Row {0}Col{1}", i, j++),
            //             GridCol2 = string.Format("Row {0}Col{1}", i, j++),
            //             GridCol3 = string.Format("Row {0}Col{1}", i, j++),
            //             GridCol4 = string.Format("Row {0}Col{1}", i, j++)
            //        });
            //}

            data = oQuestionnaireService.GetTableTypeResponse(questionId, CurrentUser.UserId);

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetAllSidePanelQuestions()
        //{
        //    List<GroupQuestionModel> oSidePanelQuestions = new List<GroupQuestionModel>();

        //    oSidePanelQuestions = oQuestionnaireService.GetQuestionList();

        //    return View("_ViewQuestionList", oSidePanelQuestions);
        //}

    }
}