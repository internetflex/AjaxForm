using System;
using System.Data;
using System.Web.Mvc;
using Oracle.DataAccess.Client;
using SbpQuery.Models;

namespace SbpQuery.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var queryDetails = new QueryDetails();
            queryDetails.Username = User.Identity.Name;
            return View();
        }

        [HttpPost]
        public ActionResult GetDetails(QueryDetails queryDetails)
        {
            string responseText = string.Empty;
            if ("Import".Equals(queryDetails.Request, StringComparison.CurrentCultureIgnoreCase))
            {
                responseText = ImportChecks(queryDetails.CaseId);
            }
            return Json(responseText, JsonRequestBehavior.AllowGet);
        }

        private string ImportChecks(int caseId)
        {
            var caseState = CaseClosedState(caseId);
            string responseText = string.Empty;

            if (string.IsNullOrEmpty(caseState))
            {
                responseText = string.Format("Case {0} not found", caseId);
            }
            else
            {
                if (caseState.Equals("Yes", StringComparison.CurrentCultureIgnoreCase))
                {
                    responseText = string.Format("Case {0} is closed", caseId);
                }
            }
            return responseText;
        }

        public string CaseClosedState(int caseId)
        {
            var connection = new OracleConnection("data source=DEVDATA;user id=FOSDEV;password=FOSDEV;");
            connection.Open();
            var command = new OracleCommand("SELECT closed_flag FROM VEC_COMPLAINT WHERE id = :caseId");

            var p1 = new OracleParameter(":caseId", OracleDbType.Int32, 4, ParameterDirection.Input);
            p1.Value = caseId;

            command.Connection = connection;
            command.Parameters.Add(p1);

            string caseState = null;
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                caseState = (string)reader["closed_flag"];
            }

            reader.Close();
            connection.Close();

            return caseState;
        }

        public void scalarQuery()
        {
            //var returnVal = new OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue);
            //var returnVal = new OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue);

            var connection = new OracleConnection("data source=XE;user id=SBPTEST;password=SBPTEST;");
            connection.Open();
            var command = new OracleCommand("SELECT count(*) FROM VEC_COMPLAINT WHERE id = :caseId");

            var p1 = new OracleParameter(":count", OracleDbType.Int32, 4, ParameterDirection.Output);
            var p2 = new OracleParameter(":caseId", OracleDbType.Int32, 4, ParameterDirection.Input);

            command.Connection = connection;
            command.Parameters.Add(p2);
            //command.Parameters.Add(p2);

            object executeScalar = command.ExecuteScalar();
        }
    }
}