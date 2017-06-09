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

        public List<Question> Questions
        {
            get { return questions; }
        }

    }
}