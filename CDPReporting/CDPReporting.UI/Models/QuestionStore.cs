using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.UI.Models
{
    public class QuestionStore
    {
        private static volatile QuestionStore instance;
        private static object syncRoot = new Object();

        private QuestionStore() {

            questions = new List<Question>();
            questionGroups = new List<Models.QuestionGroup>();
            CreateObjects();
        }

        public static QuestionStore Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new QuestionStore();
                    }
                }

                return instance;
            }
        }

        List<Question> questions;
        private List<QuestionGroup> questionGroups;

        public List<QuestionGroup> QuestionGroups
        {
            get { return questionGroups; }
            
        }


        public List<Question> Questions
        {
            get { return questions; }
        }
        void CreateObjects()
        {
            QuestionGroup group1 = new QuestionGroup();
            group1.QuestionGroupId = "CC0";
            group1.QuestionGroupName = "CC0. Introduction";
            group1.Questions.Add(new Question(QuestionType.Simple) { QuestionId = "CC01", QuestionGroupId = "CC0", QuestionText = "CC0.1. Please give a general description and introduction to your organization [maximum 5000 characters]." });
            group1.Questions.Add(new Question(QuestionType.DateRange) { QuestionId = "CC02", QuestionGroupId = "CC0", QuestionText = "CC0.2. Please state the start and end date of the year for which you are reporting data." });
            group1.Questions.Add(AddQuestionWithOptions(groupId:group1.QuestionGroupId, questionId:"CC03", questionText: "Please select the currency in which you would like to submit your response. All financial information contained in the response.", options:"Yes;No"));
            group1.Questions.Add(new Question(QuestionType.Option) { QuestionId = "CC04", QuestionGroupId = "CC0", QuestionText = "CC0.4: 	Please select if you wish to complete a shorter information request [SME questionnaire only]" });

            group1 = new QuestionGroup();
            group1.QuestionGroupId = "CC1";
            group1.QuestionGroupName = "CC1. Governance";
            Question question = new Question(QuestionType.Option) { QuestionId = "CC11", QuestionGroupId = "CC1", QuestionText = "Where is the highest level of direct responsibility for climate change within your organization?" };
            question.Options.AddRange(new List<string>()
            {"Board or individual/sub-set of the Board or other committee appointed by the Board;",
            @"Senior Manager/Officer;",
            @"Other Manager/Officer",
            @"No individual or committee with overall responsibility for climate change"});
            group1.Questions.Add(question);
            group1.Questions.Add(new Question(QuestionType.DateRange) { QuestionId = "CC11a", QuestionGroupId = "CC1", QuestionText = "CC0.2. Please state the start and end date of the year for which you are reporting data." });
            group1.Questions.Add(new Question(QuestionType.Option) { QuestionId = "CC12", QuestionGroupId = "CC1", QuestionText = "CC0.3. Please select the currency in which you would like to submit your response. All financial information contained in the response." });
            group1.Questions.Add(new Question(QuestionType.Option) { QuestionId = "CC12a", QuestionGroupId = "CC1", QuestionText = "CC0.4: 	Please select if you wish to complete a shorter information request [SME questionnaire only]" });

            questions.AddRange(group1.Questions);
        }

        private Question AddQuestionWithOptions(string groupId, string questionId, string questionText, string options)
        {
            throw new NotImplementedException();
        }
    }
}