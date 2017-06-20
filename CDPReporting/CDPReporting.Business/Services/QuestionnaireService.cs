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
                List<GroupQuestionModel> groupQuestionList = new List<GroupQuestionModel>();
                List<GroupQuestion> dbGroupQuestionList = _context.GroupQuestions.ToList();
                List<SubGroupQuestion> dbSubGroupQuestionList = _context.SubGroupQuestions.ToList();
                List<Question> dbQuestionList = _context.Questions.ToList();
                List<TableInformation> dbTableType = _context.TableInformations.ToList();
                groupQuestionList = (from groupquestion in dbGroupQuestionList
                                     select new GroupQuestionModel
                                     {
                                         QuestionGroupId = groupquestion.QuestionGroupId,
                                         QuestionGroupName = groupquestion.QuestionGroupText,
                                         SubGroupQuestion =  (from subgroup in dbSubGroupQuestionList where subgroup.QuestionGroupId == groupquestion.QuestionGroupId
                                                                  select new SubGroupQuestionModel
                                                                  {
                                                                      SubQuestionGroupId = subgroup.QuestionSubGroupId,
                                                                      SubQuestionGroupName = subgroup.QuestionSubGroupText,
                                                                      Question = (from question in dbQuestionList
                                                                                  where question.QuestionGroupId == groupquestion.QuestionGroupId && 
                                                                                  question.QuestionSubGroupId == subgroup.QuestionSubGroupId
                                                                                      select new QuestionModel
                                                                                      {
                                                                                          QuestionId = question.QuestionId,
                                                                                          QuestionName = question.QuestionText,
                                                                                          TableType = dbTableType.FirstOrDefault(m=>m.TableId == question.TableId).TableType
                                                                                      }).ToList()
                                                                  }).ToList()
                                     }).ToList();
                return groupQuestionList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to save question response.
        /// </summary>
        public void SaveResponseTableType(List<QuestionResponseTableTypeModel> modelData)
        {
            try
            {
                foreach(var data in modelData)
                {
                    TableTypeQuestion response = new TableTypeQuestion();
                    //response.UserId = 
                    response.Year = DateTime.Now.Year;
                    response.QuestionId = data.QuestionId;
                    response.GridColumn1 = data.GridCol1;
                    response.GridColumn2 = data.GridCol2;
                    response.GridColumn3 = data.GridCol3;
                    response.GridColumn4 = data.GridCol4;
                    response.GridColumn5 = data.GridCol5;
                    response.GridColumn6 = data.GridCol6;
                    response.GridColumn7 = data.GridCol7;
                    response.GridColumn8 = data.GridCol8;
                    response.GridColumn9 = data.GridCol9;
                    response.GridColumn10 = data.GridCol10;
                    _context.TableTypeQuestions.AddObject(response);
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
            Guid questionTableId = _context.Questions.First(q => q.QuestionId == questionId).TableId;
            string contextName = _context.TableInformations.First(table => table.TableId == questionTableId).TableType;
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
                    GridDescriptiveTable userAnswer = _context.GridDescriptiveTables.FirstOrDefault(ans => ans.UserId == userId &&
                        ans.QuestionId == questionId);
                    if (userAnswer != null)
                    {
                        result = userAnswer.Comment;
                    }
                    break;

                case "GDPGrid":
                    TableTypeQuestion gridResponse = _context.TableTypeQuestions.FirstOrDefault(ans => ans.UserId == userId &&
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

        public void SaveQuestionResponse(QuestionResponseModel response, Guid userId )
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
                GridDescriptiveTable data = _context.GridDescriptiveTables.FirstOrDefault(ans => ans.UserId == userId &&
                        ans.QuestionId == response.QuestionId);
                if(data != null)
                {
                    data.Comment = response.Value.ToString();                  
                }
                else
                {
                    data = new GridDescriptiveTable();
                    data.DescriptionId = Guid.NewGuid();
                    data.UserId = userId;
                    data.Year = response.Year;
                    data.QuestionId = response.QuestionId;
                    data.Comment = Convert.ToString(response.Value);
                    _context.GridDescriptiveTables.AddObject(data);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }    
}
