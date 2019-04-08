using System;
using System.Collections.Generic;
using System.Text;

namespace TaazaTV.Model
{
    //class PollContestModel
    //{
    //}

    // List Model
    public class PollContestList
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public PollContestData data { get; set; }
    }

    public class PollContestData
    {
        public Poll_List[] poll_list { get; set; }
        public int total_news { get; set; }
        public int current_page { get; set; }
        public int total_pages { get; set; }
    }

    public class Poll_List
    {
        public int poll_id { get; set; }
        public string question { get; set; }
        public string question_title { get; set; }
        public string type { get; set; }
        public string type_text { get; set; }

    }

    // Details Model
    public class PollContestModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public PollContestModelData data { get; set; }
    }

    public class PollContestModelData
    {
        public Poll_Details poll_details { get; set; }
        public Poll_Result[] poll_result { get; set; }

    }

    public class Poll_Details
    {
        public int poll_id { get; set; }
        public string question { get; set; }
        public string question_title { get; set; }
        public string type { get; set; }
        public string type_text { get; set; }
        public Poll_Options[] poll_options { get; set; }
        public int is_poll_submitted_by_user { get; set; }
        public bool is_Enabled
        {
            get
            {
                return is_poll_submitted_by_user == 0 ? true : false;
            }
            set { }
        }
    }

    public class Poll_Options
    {
        public int poll_option_id { get; set; }
        public string option_value { get; set; }
        public int is_submitted { get; set; }
        public bool Is_Selected
        {
            get
            {
                return is_submitted == 1 ? true : false;
            }
            set { }
        }
    }

    public class Poll_Result
    {
        public int poll_option_id { get; set; }
        public string option_value { get; set; }
        public int is_submitted { get; set; }
        public double total_submitted { get; set; }
        public double total_submitted_per { get; set; }
    }

    public class SubmitContestOrPoll
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public object[] data { get; set; }
    }


    public class TaazaDekhoModel
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public TaazaDekhoData data { get; set; }
    }

    public class TaazaDekhoData
    {
        public TaazaDekhoDetails details { get; set; }
        public object[] result { get; set; }
    }

    public class TaazaDekhoDetails
    {
        public int poll_id { get; set; }
        public string question { get; set; }
        public string question_title { get; set; }
        public string remaining_time { get; set; }
        public string type { get; set; }
        public string type_text { get; set; }
        public Poll_Options[] poll_options { get; set; }
        public int is_poll_submitted_by_user { get; set; }
        public bool is_Enabled
        {
            get
            {
                return is_poll_submitted_by_user == 0 ? true : false;
            }
            set { }
        }
    }

    public class SubmitContestOrPollTazzaDekho
    {
        public string responseCode { get; set; }
        public string responseText { get; set; }
        public object[] resourceList { get; set; }
        public TaazaDekhoAnswer data { get; set; }
    }

    public class TaazaDekhoAnswer
    {
        public int is_submitted_correct_answer { get; set; }
        public string current_wallet_balance { get; set; }
    }
}
