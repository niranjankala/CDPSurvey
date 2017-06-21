using CDPReporting.Business.Models;
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

        private QuestionStore()
        {

            questions = new List<CDPQuestion>();
            questionGroups = new List<Models.CDPQuestionGroup>();
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

        List<CDPQuestion> questions;
        private List<CDPQuestionGroup> questionGroups;

        public List<CDPQuestionGroup> QuestionGroups
        {
            get { return questionGroups; }

        }


        public List<CDPQuestion> Questions
        {
            get { return questions; }
        }
        void CreateObjects()
        {
            //CDPQuestionGroup group1 = new CDPQuestionGroup();
            //group1.QuestionGroupId = "CC0";
            //group1.QuestionGroupName = "CC0. Introduction";
            //group1.Questions.Add(new CDPQuestion(QuestionType.Simple) { QuestionId = "CC01", QuestionGroupId = "CC0", QuestionText = "CC0.1. Please give a general description and introduction to your organization [maximum 5000 characters]." });
            //group1.Questions.Add(new CDPQuestion(QuestionType.DateRange) { QuestionId = "CC02", QuestionGroupId = "CC0", QuestionText = "CC0.2. Please state the start and end date of the year for which you are reporting data." });

            //group1.Questions.Add(AddQuestionWithOptions(QuestionType.DropDownList, groupId: group1.QuestionGroupId, questionId: "CC03", questionText: "Country list configuration", options: "IND;USA;UK"));

            //group1.Questions.Add(AddQuestionWithOptions(QuestionType.DropDown, groupId: group1.QuestionGroupId, questionId: "CC04", questionText: "Please select the currency in which you would like to submit your response. All financial information contained in the response.", options: "USD;INR;EUR"));
            //group1.Questions.Add(new CDPQuestion(QuestionType.Boolean) { QuestionId = "CC05", QuestionGroupId = "CC0", QuestionText = "CC0.5: 	Please select if you wish to complete a shorter information request [SME questionnaire only]" });

            //questions.AddRange(group1.Questions);
            //questionGroups.Add(group1);

            //group1 = new CDPQuestionGroup();
            //group1.QuestionGroupId = "CC1";
            //group1.QuestionGroupName = "CC1. Governance";
            //CDPQuestion question = new CDPQuestion(QuestionType.Option) { QuestionId = "CC11", QuestionGroupId = "CC1", QuestionText = "Where is the highest level of direct responsibility for climate change within your organization?" };
            //question.Options.AddRange(new List<string>()
            //{"Board or individual/sub-set of the Board or other committee appointed by the Board;",
            //@"Senior Manager/Officer;",
            //@"Other Manager/Officer",
            //@"No individual or committee with overall responsibility for climate change"});
            //group1.Questions.Add(question);
            //group1.Questions.Add(AddQuestionWithOptions(QuestionType.OptionList, groupId: group1.QuestionGroupId, questionId: "CC12", questionText: "Incentivized performance indicator", options: "Emissions reduction project;Emissions reduction target;Energy reduction project;Energy reduction target;Efficiency project;Efficiency target;Behavior change related indicator;Environmental criteria included in purchases; Supply chain engagement; Other, please specify"));

            //group1.Questions.Add(new CDPQuestion(QuestionType.CDPGrid) { QuestionId = "CC12a", QuestionGroupId = "CC1", QuestionText = "CC1.2a:Please provide further details on the incentives provided for the management of climate change issues" });
            //questions.AddRange(group1.Questions);
            //questionGroups.Add(group1);
        }

        private CDPQuestion AddQuestionWithOptions(QuestionType questionType, string groupId, string questionId, string questionText, string options)
        {
            //CDPQuestion question = new CDPQuestion(questionType) { QuestionId = questionId, QuestionGroupId = groupId, QuestionText = questionText };
            //if (!string.IsNullOrWhiteSpace(options) && options.Contains(';'))
            //{
            //    foreach (string option in options.Split(';'))
            //    {
            //        question.Options.Add(option);
            //    }
            //}
            return new CDPQuestion();
        }
    }

    public class CDPQuestionGroup
    {
        public string QuestionGroupId { get; set; }
        public string QuestionGroupName { get; set; }
        public List<CDPQuestion> Questions { get; set; }
        public CDPQuestionGroup()
        {
            Questions = new List<CDPQuestion>();
        }
    }
    public class CDPQuestion
    {
        public CDPQuestion(QuestionType questionType)
        {
            type = questionType;
            InitializeQuestion();
        }
        private List<string> options;

        public List<string> Options
        {
            get { return options; }
            set { options = value; }
        }

        public string QuestionId { get; set; }
        public string QuestionGroupId { get; set; }
        private QuestionType type;

        public QuestionType Type
        {
            get { return type; }
            private set { type = value; }
        }

        public string QuestionText { get; set; }
        void InitializeQuestion()
        {
            switch (type)
            {
                case QuestionType.Simple:

                case QuestionType.Date:
                case QuestionType.DateRange:
                    break;
                case QuestionType.Boolean:
                case QuestionType.Option:
                case QuestionType.OptionList:
                case QuestionType.DropDown:
                case QuestionType.DropDownList:
                    options = new List<string>();
                    break;
                case QuestionType.CDPGrid:
                    break;
            }
        }
    }

    public class Options
    {
        //public Options(QuestionType questionType, )
    }
}