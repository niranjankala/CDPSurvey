using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDPReporting.Data.Entity;
using log4net;
using CDPReporting.Business.Models;

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
        public List<GroupQuestionModel> GetQuestionList()
        {
            try
            {
                List<GroupQuestionModel> questionGroups = new List<GroupQuestionModel>();
                List<CDPQuestionGroup> dbGroupQuestionList = _context.CDPQuestionGroups.ToList();
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
                                                       QuestionText = question.QuestionText,
                                                       QuestionOrder = question.QuestionOrder,
                                                       GroupText = questionGroup.QuestionGroupText,
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
        /// <summary>
        /// Method to save question response.
        /// </summary>
        public void SaveResponseTableType(List<QuestionResponseTableTypeModel> modelData, Guid questionId, Guid userPlantId, int selectedYear)
        {
            try
            {
                List<CDPTabularQuestionAnswer> responseList = _context.CDPTabularQuestionAnswers.Where(answer => answer.PlantId == userPlantId && answer.QuestionId == questionId && answer.Year == selectedYear).ToList();
                List<CDPTabularQuestionAnswer> responseToRemove = new List<CDPTabularQuestionAnswer>();

                if (modelData == null || modelData.Count == 0)
                    responseToRemove = responseList.Where(m => m.QuestionId == questionId && m.PlantId == userPlantId && m.Year == selectedYear).ToList();
                else
                    responseToRemove = responseList.Where(x => !modelData.Any(m => m.GridIndexId == x.Id)).ToList();
                if (modelData != null)
                {
                    foreach (var data in modelData)
                    {
                        //response = _context.CDPTableTypeQuestions.First(answer => answer.GridIndex == data.GridIndexId);

                        bool responseToAdd = responseList.Any(m => m.Id == data.GridIndexId);

                        if (!responseToAdd)
                        {

                            CDPTabularQuestionAnswer response = new CDPTabularQuestionAnswer()
                            {
                                Id = Guid.NewGuid(),
                                PlantId = userPlantId,
                                Year = selectedYear,
                                QuestionId = questionId,
                                GridColumn1 = data.GridCol1,
                                GridColumn2 = data.GridCol2,
                                GridColumn3 = data.GridCol3,
                                GridColumn4 = data.GridCol4,
                                GridColumn5 = data.GridCol5,
                                GridColumn6 = data.GridCol6,
                                GridColumn7 = data.GridCol7,
                                GridColumn8 = data.GridCol8,
                                GridColumn9 = data.GridCol9,
                                GridColumn10 = data.GridCol10
                            };
                            _context.CDPTabularQuestionAnswers.AddObject(response);
                            _context.SaveChanges();

                        }
                    }
                }

                foreach (var data in responseToRemove)
                {
                    CDPTabularQuestionAnswer response = _context.CDPTabularQuestionAnswers.First(m => m.Id == data.Id);
                    _context.CDPTabularQuestionAnswers.DeleteObject(response);
                    _context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuestionResponseModel GetQuestionResponse(Guid questionId, Guid userPlantId, int selectedYear)
        {
            CDPQuestion question = _context.CDPQuestions.FirstOrDefault(q => q.QId == questionId);
            CDPQuestionType questionType = _context.CDPQuestionTypes.FirstOrDefault(type => type.Id == question.QuestionType);
            QuestionResponseModel result = new QuestionResponseModel();
            result.QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), questionType.Type);
            result.Caption = question.Title;
            result.QuestionText = question.QuestionText;
            result.Value = GetQuestionAnswerDetails(userPlantId, questionId, result.QuestionType, selectedYear);
            if (questionType.Type != "Simple")
                result.OptionList = GetOptionList(questionId);
            result.QuestionId = questionId;
            result.Year = DateTime.Now.Year;
            return result;
        }

        private Options GetOptionList(Guid questionId)
        {
            try
            {
                Options optionList = new Options();
                CDPQuestionOption dbOptionList = _context.CDPQuestionOptions.FirstOrDefault(m=>m.QuestionId == questionId);
                if (dbOptionList != null)
                {
                    optionList.OptionId = dbOptionList.OptionId;
                    optionList.QuestionId = dbOptionList.QuestionId;
                    optionList.OptionCSVText = dbOptionList.OptionsCSVText.Split(',').ToList();
                    optionList.OtherOptions = dbOptionList.OptionOthersText;
                }
                return optionList;
            }
            catch(Exception ex)
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
                case QuestionType.MultipleSelectList:
                    CDPSimpleChoiceAnswer userAnswer = _context.CDPSimpleChoiceAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (userAnswer != null)
                    {
                        result = userAnswer.AnswerValue;
                    }
                    break;
                case QuestionType.List:
                    break;
                case QuestionType.DropDown:
                    break;
                case QuestionType.DropDownList:
                    break;
                case QuestionType.Option:
                    break;
                case QuestionType.OptionList:
                    break;
                case QuestionType.DateRange:
                    CDPDateRangeAnswer dateRangeAns = _context.CDPDateRangeAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId && ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (dateRangeAns != null)
                    {
                        result = string.Format("{0}s{1}", dateRangeAns.StartDate, dateRangeAns.EndDate);
                    }
                    break;
                case QuestionType.Date:
                    break;
                case QuestionType.Boolean:
                    break;
                case QuestionType.CDPGrid:
                    CDPTabularQuestionAnswer gridResponse = _context.CDPTabularQuestionAnswers.FirstOrDefault(ans => ans.PlantId == userPlantId &&
                        ans.QuestionId == questionId && ans.Year == selectionYear);
                    if (gridResponse != null)
                    {
                        result = gridResponse.GridColumn1;
                        result = gridResponse.GridColumn2;
                        result = gridResponse.GridColumn3;
                        result = gridResponse.GridColumn4;
                        result = gridResponse.GridColumn5;
                        result = gridResponse.GridColumn6;
                        result = gridResponse.GridColumn7;
                        result = gridResponse.GridColumn8;
                        result = gridResponse.GridColumn9;
                        result = gridResponse.GridColumn10;
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
                        break;
                    case QuestionType.DropDownList:
                        SaveDropDownList(response, userPlantId);
                        break;
                    case QuestionType.Option:
                        break;
                    case QuestionType.OptionList:
                        break;
                    case QuestionType.DateRange:
                        SaveDateRangeQuestion(response, userPlantId);
                        break;
                    case QuestionType.Date:
                        break;
                    case QuestionType.Boolean:
                        break;
                    case QuestionType.CDPGrid:
                        break;
                    case QuestionType.MultipleSelectList:                        
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

        public List<QuestionResponseTableTypeModel> GetTableTypeResponse(Guid questionId, Guid plantId, int selectedYear)
        {
            try
            {
                List<QuestionResponseTableTypeModel> responseList = new List<QuestionResponseTableTypeModel>();
                List<CDPTabularQuestionAnswer> dbResponseList = _context.CDPTabularQuestionAnswers.Where(m => m.PlantId == plantId && m.QuestionId == questionId && m.Year == selectedYear).ToList();
                responseList = (from list in dbResponseList
                                select new QuestionResponseTableTypeModel
                                {
                                    GridIndexId = list.Id,
                                    QuestionId = list.QuestionId,
                                    GridCol1 = list.GridColumn1,
                                    GridCol2 = list.GridColumn2,
                                    GridCol3 = list.GridColumn3,
                                    GridCol4 = list.GridColumn4,
                                    GridCol5 = list.GridColumn5,
                                }).ToList();
                return responseList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
