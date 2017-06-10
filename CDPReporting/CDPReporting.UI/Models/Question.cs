using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDPReporting.UI.Models
{
    public class Question
    {   
        public Question(QuestionType questionType)
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
                case QuestionType.Boolean:
                case QuestionType.Date:
                case QuestionType.DateRange:
                case QuestionType.Option:
                case QuestionType.OptionList:
                case QuestionType.CDPGrid:
                    break;
            }
        }


    }

    public enum QuestionType
    {
        Simple,
        Option,
        OptionList,
        DateRange,
        Date,
        Boolean,
        CDPGrid
    }
}