using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDPReporting.Data.Entity;
using log4net;
using CDPReporting.Business.Models;
using CDPReporting.Data.Entity;
using System.Transactions;

namespace CDPReporting.Business.Services
{
    public class QuestionnaireService
    {
        private SMR_KMS_DB_DevEntities _context;
        private ILog _log;
        public QuestionnaireService()
        {
            _context = new SMR_KMS_DB_DevEntities();
            _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        /// <summary>
        /// Method to get list of Questions.
        /// </summary>
        public List<GroupQuestionModel> GetQuestionList(Guid questionnaireId)
        {
            try
            {
                List<GroupQuestionModel> questionGroups = new List<GroupQuestionModel>();
                List<CDPQuestionGroup> dbGroupQuestionList = _context.CDPQuestionGroups.Where(m => m.QuestionnaireId == questionnaireId).ToList();
                List<CDPQuestionSubGroup> dbSubGroupQuestionList = _context.CDPQuestionSubGroups.ToList();
                List<CDPQuestion> dbQuestionList = _context.CDPQuestions.ToList();
                List<CDPQuestionType> questionTypes = _context.CDPQuestionTypes.ToList();
                questionGroups = (from questionGroup in (dbGroupQuestionList.OrderBy(g => g.Order).ToList())
                                  select new GroupQuestionModel
                                  {
                                      QuestionGroupId = questionGroup.QuestionGroupId,
                                      QuestionGroupName = questionGroup.QuestionGroupText,
                                      Questions = (from question in dbQuestionList.Where(q => q.QuestionGroupId == questionGroup.QGroupId).OrderBy(q => q.QuestionOrder)
                                                   join qType in questionTypes on question.QuestionType equals qType.Id
                                                   select new QuestionModel
                                                   {
                                                       Id = question.QId,
                                                       QuestionId = question.QuestionId,
                                                       Title = question.Title,
                                                       QuestionText = question.QuestionText,
                                                       QuestionOrder = question.QuestionOrder,
                                                       GroupText = questionGroup.QuestionGroupText,
                                                       IsApplicable = question.IsApplicable,

                                                       //SubGroupText = question.SubGroupText,
                                                       QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), qType.Type)
                                                   }).ToList()
                                  }).ToList();





                return questionGroups;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<QuestionnaireQuestions> GetQuestionnaireOptionList()
        {
            try
            {
                List<QuestionnaireQuestions> questionnaireList = new List<QuestionnaireQuestions>();
                //CDPQuestionnaire dbQuestionnaireList = _context.CDPQuestionnaires.FirstOrDefault(m=>m.Id == questionnaireId);
                questionnaireList = (from list in _context.CDPQuestionnaires
                                     select new QuestionnaireQuestions
                                         {
                                             Id = list.Id,
                                             Name = list.Name
                                         }).ToList();
                return questionnaireList;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Method to save question response.
        /// </summary>
        public void SaveResponseTableType(List<QuestionResponseTableTypeModel> modelData, Guid questionId, Guid userPlantId, int selectedYear)
        {
            using (var transaction = new TransactionScope())
            {

                try
                {
                    List<CDPTabularAnswer> responseList = _context.CDPTabularAnswers.Where(answer =>
                        answer.PlantId == userPlantId && answer.QuestionId == questionId && answer.Year == selectedYear).ToList();

                    foreach (var data in responseList)
                    {
                        CDPTabularAnswer response = _context.CDPTabularAnswers.First(m => m.Id == data.Id);
                        _context.CDPTabularAnswers.DeleteObject(response);
                    }

                    if (modelData != null)
                    {
                        foreach (var data in modelData)
                        {
                            CDPTabularAnswer response = new CDPTabularAnswer()
                            {
                                Id = Guid.NewGuid(),
                                PlantId = userPlantId,
                                Year = selectedYear,
                                QuestionId = questionId,
                                Answer = data.Answer

                            };
                            _context.CDPTabularAnswers.AddObject(response);
                        }
                    }

                    _context.SaveChanges();

                    transaction.Complete();
                }

                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw ex;
                }
            }
        }

        public QuestionResponseModel GetQuestionResponse(Guid questionId, Guid userPlantId, int selectedYear)
        {
            CDPQuestion question = _context.CDPQuestions.FirstOrDefault(q => q.QId == questionId);
            CDPQuestionType questionType = _context.CDPQuestionTypes.FirstOrDefault(type => type.Id == question.QuestionType);

            //CDPQuestionValidation validate = _context.CDPQuestionValidations.FirstOrDefault(m => m.QuestionId == questionId);
            //CDPValidationType validationType = _context.CDPValidationTypes.FirstOrDefault(m => m.Id == validate.ValidationId);

            QuestionResponseModel result = new QuestionResponseModel();
            result.QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), questionType.Type);
            result.CDPId = question.QuestionId;
            result.Caption = question.Title;
            result.QuestionText = question.QuestionText;
            result.Value = GetQuestionAnswerDetails(userPlantId, questionId, result.QuestionType, selectedYear);
            if (questionType.Type != "Simple")
                result.OptionList = GetOptionList(questionId);
            result.QuestionId = questionId;
            result.Year = DateTime.Now.Year;
            result.IsAnswerResponseAllowed = question.IsApplicable ?? true;
            result.Validations = (from validation in _context.CDPQuestionValidations.ToList()
                                  join type in _context.CDPValidationTypes.ToList() on validation.ValidationId equals type.Id
                                  where (validation.QuestionId == questionId)
                                  select new Validation
                                  {
                                      ValidationValue = validation.Value,
                                      ValidationType = ParseValidationType(type.ValidationType)
                                  }).ToList();

            Validation defaultValueResutlt = null;
            if (result.Validations != null && (result.Value == null || string.IsNullOrEmpty(Convert.ToString(result.Value))))
            {
                defaultValueResutlt = result.Validations.FirstOrDefault(v => v.ValidationType == ValidationType.DefaultValue);
                if (defaultValueResutlt != null)
                    result.Value = defaultValueResutlt.ValidationValue;
            }
            return result;
        }

        private ValidationType ParseValidationType(string validationType)
        {
            ValidationType result = ValidationType.None;
            if (!string.IsNullOrWhiteSpace(validationType) && Enum.IsDefined(typeof(ValidationType), validationType))
                result = (ValidationType)Enum.Parse(typeof(ValidationType), validationType);
            return result;
        }

        private Options GetOptionList(Guid questionId)
        {
            try
            {
                Options optionList = new Options();
                CDPQuestionOption dbOptionList = _context.CDPQuestionOptions.FirstOrDefault(m => m.QuestionId == questionId);
                if (dbOptionList != null)
                {
                    optionList.OptionId = dbOptionList.OptionId;
                    optionList.QuestionId = dbOptionList.QuestionId;
                    optionList.OptionCSVText = dbOptionList.OptionsCSVText.Split('|').ToList();
                    optionList.OtherOptions = dbOptionList.OptionOthersText;
                }
                return optionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object GetQuestionAnswerDetails(Guid userPlantId, Guid questionId, QuestionType contextName, int selectionYear)
        {
            object result = null;
            switch (contextName)
            {
                case QuestionType.Simple:
                    CDPSimpleChoiceAnswer userSimpleAnswer = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (userSimpleAnswer != null)
                    {
                        result = userSimpleAnswer.AnswerValue;
                    }
                    break;
                case QuestionType.List:
                    break;
                case QuestionType.DropDown:
                    CDPSimpleChoiceAnswer userDropDownAnswer = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (userDropDownAnswer != null)
                    {
                        result = userDropDownAnswer.AnswerValue;
                    }
                    break;
                case QuestionType.DropDownList:
                    CDPSimpleChoiceAnswer userAnswer = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (userAnswer != null)
                    {
                        result = userAnswer.AnswerValue;
                    }
                    break;
                case QuestionType.Option:
                    CDPSimpleChoiceAnswer userOptionAnswer = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (userOptionAnswer != null)
                    {
                        result = userOptionAnswer.AnswerValue;
                    }
                    break;
                case QuestionType.MultipleSelectList:
                    CDPSimpleChoiceAnswer userMultipleSelectListAnswer = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (userMultipleSelectListAnswer != null)
                    {
                        result = userMultipleSelectListAnswer.AnswerValue;
                    }
                    break;
                case QuestionType.DateRange:
                    CDPDateRangeAnswer dateRangeAns = _context.CDPDateRangeAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId && ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (dateRangeAns != null)
                    {
                        result = new DateRange() { StartDate = dateRangeAns.StartDate, EndDate = dateRangeAns.EndDate };
                    }
                    break;
                case QuestionType.Date:
                    break;
                case QuestionType.Boolean:
                    CDPSimpleChoiceAnswer userBooleanAnswer = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (userBooleanAnswer != null)
                    {
                        result = userBooleanAnswer.AnswerValue;
                    }
                    break;
                case QuestionType.CDPGridResultList:
                    //var gridResponse = _context.CDPTabularAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                    //    ans.QuestionId == questionId && ans.Year == selectionYear);
                    // List<string> gridResponse
                    var Answers = from tablans in _context.CDPTabularAnswers
                                  where tablans.PlantId == userPlantId && tablans.QuestionId == questionId && tablans.Year == selectionYear
                                  orderby tablans.Id
                                  select tablans;
                    //).ToList();
                    List<CDPTabularAnswer> gridResponse = new List<CDPTabularAnswer>();
                    foreach (var item in Answers)
                    {
                        CDPTabularAnswer oCDPTabularAnswer = new CDPTabularAnswer();
                        oCDPTabularAnswer.Answer = item.Answer;
                        oCDPTabularAnswer.Id = item.Id;
                        oCDPTabularAnswer.PlantId = item.PlantId;
                        oCDPTabularAnswer.QuestionId = item.QuestionId;
                        oCDPTabularAnswer.Year = item.Year;
                        gridResponse.Add(oCDPTabularAnswer);
                    }
                    if (gridResponse != null)
                    {
                        result = gridResponse;
                    }
                    break;

                case QuestionType.CDPGrid:
                    //var gridResponse = _context.CDPTabularAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                    //    ans.QuestionId == questionId && ans.Year == selectionYear);
                    // List<string> gridResponse
                    var gridAnswers = from tablans in _context.CDPTabularAnswers
                                      where tablans.PlantId == userPlantId && tablans.QuestionId == questionId && tablans.Year == selectionYear
                                      orderby tablans.Id
                                      select tablans;
                    //).ToList();
                    List<CDPTabularAnswer> cdpgridResponse = new List<CDPTabularAnswer>();
                    foreach (var item in gridAnswers)
                    {
                        CDPTabularAnswer oCDPTabularAnswer = new CDPTabularAnswer();
                        oCDPTabularAnswer.Answer = item.Answer;
                        oCDPTabularAnswer.Id = item.Id;
                        oCDPTabularAnswer.PlantId = item.PlantId;
                        oCDPTabularAnswer.QuestionId = item.QuestionId;
                        oCDPTabularAnswer.Year = item.Year;
                        cdpgridResponse.Add(oCDPTabularAnswer);
                    }
                    if (cdpgridResponse != null)
                    {
                        result = cdpgridResponse;
                    }

                    break;

                default:
                    break;
            }
            return result;
        }

        public void SaveQuestionResponse(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                switch (response.QuestionType)
                {
                    case QuestionType.Simple:
                        SaveSimpleQuestion(response, userPlantId);
                        break;
                    case QuestionType.List:
                        break;
                    case QuestionType.DropDown:
                        SaveSimpleQuestion(response, userPlantId);
                        break;
                    case QuestionType.DropDownList:
                        SaveDropDown(response, userPlantId);
                        break;
                    case QuestionType.Option:
                        SaveOptionQuestion(response, userPlantId);
                        break;
                    case QuestionType.OptionList:
                        break;
                    case QuestionType.DateRange:
                        SaveDateRangeQuestion(response, userPlantId);
                        break;
                    case QuestionType.Date:
                        break;
                    case QuestionType.Boolean:
                        SaveBooleanQuestion(response, userPlantId);
                        break;
                    case QuestionType.CDPGrid:
                        break;
                    case QuestionType.MultipleSelectList:
                        SaveMultipleSelectListQuestion(response, userPlantId);
                        break;
                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void SaveOptionQuestion(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                CDPSimpleChoiceAnswer data = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == response.QuestionId && ans.Year == response.Year);
                if (data != null)
                {
                    data.AnswerValue = response.Value.ToString();
                }
                else
                {
                    data = new CDPSimpleChoiceAnswer();
                    data.AnswerId = Guid.NewGuid();
                    data.PlantId = userPlantId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.AnswerValue = Convert.ToString(response.Value);
                    _context.CDPSimpleChoiceAnswers.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveMultipleSelectListQuestion(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                CDPSimpleChoiceAnswer data = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == response.QuestionId && ans.Year == response.Year);
                if (data != null)
                {
                    data.AnswerValue = response.Value.ToString();
                }
                else
                {
                    data = new CDPSimpleChoiceAnswer();
                    data.AnswerId = Guid.NewGuid();
                    data.PlantId = userPlantId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.AnswerValue = Convert.ToString(response.Value);
                    _context.CDPSimpleChoiceAnswers.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveDropDown(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                CDPSimpleChoiceAnswer data = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == response.QuestionId && ans.Year == response.Year);
                if (data != null)
                {
                    data.AnswerValue = Convert.ToString(response.Value);
                }
                else
                {
                    data = new CDPSimpleChoiceAnswer();
                    data.AnswerId = Guid.NewGuid();
                    data.PlantId = userPlantId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.AnswerValue = Convert.ToString(response.Value);
                    _context.CDPSimpleChoiceAnswers.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveBooleanQuestion(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                CDPSimpleChoiceAnswer data = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == response.QuestionId && ans.Year == response.Year);
                if (data != null)
                {
                    data.AnswerValue = response.Value.ToString();
                }
                else
                {
                    data = new CDPSimpleChoiceAnswer();
                    data.AnswerId = Guid.NewGuid();
                    data.PlantId = userPlantId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.AnswerValue = Convert.ToString(response.Value);
                    _context.CDPSimpleChoiceAnswers.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveDropDownList(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                CDPSimpleChoiceAnswer data = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == response.QuestionId && ans.Year == response.Year);
                if (data != null)
                {
                    data.AnswerValue = response.Value.ToString();
                }
                else
                {
                    data = new CDPSimpleChoiceAnswer();
                    data.AnswerId = Guid.NewGuid();
                    data.PlantId = userPlantId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.AnswerValue = Convert.ToString(response.Value);
                    _context.CDPSimpleChoiceAnswers.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveDateRangeQuestion(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                CDPDateRangeAnswer data = _context.CDPDateRangeAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId.Equals(response.QuestionId) && ans.Year == response.Year);
                string[] date = response.Value.ToString().Split('s');

                if (data != null)
                {
                    data.StartDate = Convert.ToDateTime(date[0]);
                    data.EndDate = Convert.ToDateTime(date[1]);
                }
                else
                {
                    data = new CDPDateRangeAnswer();
                    data.AnswerId = Guid.NewGuid();
                    data.PlantId = userPlantId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.StartDate = Convert.ToDateTime(date[0]);
                    data.EndDate = Convert.ToDateTime(date[1]);
                    _context.CDPDateRangeAnswers.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveSimpleQuestion(QuestionResponseModel response, Guid userPlantId)
        {
            try
            {
                CDPSimpleChoiceAnswer data = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == response.QuestionId && ans.Year == response.Year);
                if (data != null)
                {
                    data.AnswerValue = response.Value.ToString();
                }
                else
                {
                    data = new CDPSimpleChoiceAnswer();
                    data.AnswerId = Guid.NewGuid();
                    data.PlantId = userPlantId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.AnswerValue = Convert.ToString(response.Value);
                    _context.CDPSimpleChoiceAnswers.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<QuestionResponseTableTypeModel> GetTableTypeResponse(Guid questionId, Guid plantId, int selectedYear)
        //{
        //    try
        //    {
        //        List<QuestionResponseTableTypeModel> responseList = new List<QuestionResponseTableTypeModel>();
        //        List<CDPTabularQuestionAnswer> dbResponseList = _context.CDPTabularQuestionAnswers.Where(m => m.PlantId == plantId && m.QuestionId == questionId && m.Year == selectedYear).ToList();
        //        responseList = (from list in dbResponseList
        //                        select new QuestionResponseTableTypeModel
        //                        {
        //                            GridIndexId = list.Id,
        //                            QuestionId = list.QuestionId,
        //                            GridCol1 = list.GridColumn1,
        //                            GridCol2 = list.GridColumn2,
        //                            GridCol3 = list.GridColumn3,
        //                            GridCol4 = list.GridColumn4,
        //                            GridCol5 = list.GridColumn5,
        //                        }).ToList();
        //        return responseList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public JsondataQuestions GetQuestionIdForJson(Guid questionId)
        {
            JsondataQuestions oJsondataQuestions = new JsondataQuestions();
            var response = _context.CDPQuestions.Select(p => p).Where(p => p.QId == questionId).FirstOrDefault();
            oJsondataQuestions.QId = response.QId;
            oJsondataQuestions.QuestionId = response.QuestionId;

            return oJsondataQuestions;
        }
    }
}
