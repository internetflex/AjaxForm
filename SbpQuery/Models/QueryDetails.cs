using System.ComponentModel.DataAnnotations;

namespace SbpQuery.Models
{
    public class QueryDetails
    {
        [Display(Name = "Complaint Number")]
        public int CaseId { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Results")]
        public string QueryAnswer { get; set; }

        public string Request { get; set; }
    }
}