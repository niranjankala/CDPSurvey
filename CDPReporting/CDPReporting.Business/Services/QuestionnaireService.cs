﻿using System;
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
                List<CDPTableInformation> dbTableType = _context.CDPTableInformations.ToList();
                questionGroups = (from groupquestion in dbGroupQuestionList
                                  select new GroupQuestionModel
                                  {
                                      QuestionGroupId = groupquestion.QuestionGroupId,
                                      QuestionGroupName = groupquestion.QuestionGroupText,
                                      SubGroupQuestion = (from subgroup in dbSubGroupQuestionList
                                                          where subgroup.QuestionGroupId == groupquestion.QuestionGroupId
                                                          select new SubGroupQuestionModel
                                                          {
                                                              SubQuestionGroupId = subgroup.QuestionSubGroupId,
                                                              SubQuestionGroupName = subgroup.QuestionSubGroupText,

                                                              Question = (from question in dbQuestionList
                                                                          where question.QuestionGroupId == groupquestion.QuestionGroupId &&
                                                                          question.QuestionSubGroupId == subgroup.QuestionSubGroupId
                                                                          orderby question.QuestionOrder
                                                                          select new QuestionModel
                                                                          {
                                                                              Id = question.QId,
                                                                              QuestionId = question.QuestionId,
                                                                              QuestionText = question.QuestionText,
                                                                              QuestionOrder = question.QuestionOrder ?? 1,
                                                                              GroupText = question.GroupText,
                                                                              SubGroupText = question.SubGroupText,

                                                                              TableType = dbTableType.FirstOrDefault(m => m.TableId == question.TableId).TableType
                                                                          }).ToList()
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
        public void SaveResponseTableType(List<QuestionResponseTableTypeModel> modelData, string questionId, Guid userId)
        {
            try
            {
                List<CDPTableTypeQuestion> responseList = _context.CDPTableTypeQuestions.Where(answer => answer.UserId == userId && answer.QuestionId == questionId).ToList();
                List<CDPTableTypeQuestion> responseToRemove = new List<CDPTableTypeQuestion>();

                if (modelData == null || modelData.Count == 0)
                    responseToRemove = responseList.Where(m => m.QuestionId == questionId && m.UserId == userId).ToList();
                else
                    responseToRemove = responseList.Where(x => !modelData.Any(m => m.GridIndexId == x.GridIndex)).ToList();
                if (modelData != null)
                {
                    foreach (var data in modelData)
                    {
                        //response = _context.CDPTableTypeQuestions.First(answer => answer.GridIndex == data.GridIndexId);

                        bool responseToAdd = responseList.Where(m => m.GridIndex == data.GridIndexId).Any();

                        if (!responseToAdd)
                        {

                            CDPTableTypeQuestion response = new CDPTableTypeQuestion()
                            {
                                GridIndex = Guid.NewGuid(),
                                UserId = userId,
                                Year = DateTime.Now.Year,
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
                            _context.CDPTableTypeQuestions.AddObject(response);
                            _context.SaveChanges();

                        }
                    }
                }

                foreach (var data in responseToRemove)
                {
                    CDPTableTypeQuestion response = _context.CDPTableTypeQuestions.First(m => m.GridIndex == data.GridIndex);
                    _context.CDPTableTypeQuestions.DeleteObject(response);
                    _context.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public QuestionResponseModel GetQuestionResponse(string questionId, Guid userId)
        {
            Guid questionTableId = _context.CDPQuestions.First(q => q.QuestionId == questionId).TableId;
            string contextName = _context.CDPTableInformations.First(table => table.TableId == questionTableId).TableType;
            QuestionResponseModel result = new QuestionResponseModel();
            result.Value = GetQuestionAnswerDetails(userId, questionId, contextName);
            result.QuestionType = QuestionType.Simple;
            result.QuestionId = questionId;
            result.Year = DateTime.Now.Year;
            return result;
        }

        private object GetQuestionAnswerDetails(Guid userId, string questionId, string contextName)
        {
            object result = null;
            switch (contextName)
            {
                case "GridDescriptiveTable":
                    CDPGridDescriptive userAnswer = _context.CDPGridDescriptives.FirstOrDefault(ans => ans.UserId == userId &&
                        ans.QuestionId == questionId);
                    if (userAnswer != null)
                    {
                        result = userAnswer.Comment;
                    }
                    break;

                case "GDPGrid":
                    CDPTableTypeQuestion gridResponse = _context.CDPTableTypeQuestions.FirstOrDefault(ans => ans.UserId == userId &&
                        ans.QuestionId == questionId);
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

        public void SaveQuestionResponse(QuestionResponseModel response, Guid userId)
        {
            try
            {
                switch (response.QuestionType)
                {
                    case QuestionType.Simple:
                        SaveSimpleQuestion(response, userId);
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
                        break;
                    case QuestionType.Date:
                        break;
                    case QuestionType.Boolean:
                        break;
                    case QuestionType.CDPGrid:

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

        private void SaveSimpleQuestion(QuestionResponseModel response, Guid userId)
        {
            try
            {
                CDPGridDescriptive data = _context.CDPGridDescriptives.FirstOrDefault(ans => ans.UserId == userId &&
                        ans.QuestionId == response.QuestionId);
                if (data != null)
                {
                    data.Comment = response.Value.ToString();
                }
                else
                {
                    data = new CDPGridDescriptive();
                    data.DescriptionId = Guid.NewGuid();
                    data.UserId = userId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.Comment = Convert.ToString(response.Value);
                    _context.CDPGridDescriptives.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<QuestionResponseTableTypeModel> GetTableTypeResponse(string questionId, Guid userId)
        {
            try
            {
                List<QuestionResponseTableTypeModel> responseList = new List<QuestionResponseTableTypeModel>();
                List<CDPTableTypeQuestion> dbResponseList = _context.CDPTableTypeQuestions.Where(m => m.UserId == userId && m.QuestionId == questionId).ToList();
                responseList = (from list in dbResponseList
                                select new QuestionResponseTableTypeModel
                                {
                                    GridIndexId = list.GridIndex,
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
