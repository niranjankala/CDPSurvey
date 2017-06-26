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
                if (_log.IsInfoEnabled) _log.Info("Calling Index method of GetQestionView");
               // Guid questionId = questionViewId.Replace("Question_", "").Replace("_", ".");
                //questionViewId = questionViewId.Insert(0, "_");
                Guid plantId = CurrentUser.PlantId ?? Guid.NewGuid();
                var model = oQuestionnaireService.GetQuestionResponse(questionViewId, plantId);
                return PartialView("_Question_CC0_3", model);
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
                Guid plantId = CurrentUser.PlantId ?? Guid.NewGuid();
                oQuestionnaireService.SaveQuestionResponse(response, plantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveResponseTableType(List<QuestionResponseTableTypeModel> model, Guid questionId, int selectedYear)
        {
            if (_log.IsInfoEnabled) _log.Info("Calling Index method of SaveQuestionResponse");
            Guid plantId = CurrentUser.PlantId ?? Guid.NewGuid();
            oQuestionnaireService.SaveResponseTableType(model, questionId, plantId, selectedYear);
        }

        public JsonResult GetQuestionData(Guid questionId, int year)
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

            Guid plantId = CurrentUser.PlantId ?? Guid.NewGuid();

            data = oQuestionnaireService.GetTableTypeResponse(questionId, plantId, year);

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