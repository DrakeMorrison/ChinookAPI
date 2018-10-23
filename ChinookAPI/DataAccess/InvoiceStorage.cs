using ChinookAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChinookAPI.DataAccess
{
    public class InvoiceStorage
    {
        static List<Invoice> _invoicebox = new List<Invoice>();
        private const string ConnectionString = "Server=(local);Database=Chinook;Trusted_Connection=True;";

        public Invoice GetInvoice(int agentId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();

                command.CommandText = @"
                    select
	                    agent_name = Employee.FirstName + ' ' + Employee.LastName,
	                    Invoice.*
                    from Invoice
                    join Customer
	                    on Customer.CustomerId = Invoice.CustomerId
                    join Employee
	                    on EmployeeId = SupportRepId
                    where EmployeeId = @agentId
                ";

                command.Parameters.AddWithValue("@agentId", agentId);

                var reader = command.ExecuteReader();

                var recheck = reader.Read();

                if (recheck)
                {
                    var invoice = new Invoice()
                    {
                        CustomerId = (int) reader["CustomerId"],
                        InvoiceDate = (DateTime) reader["InvoiceDate"],
                        BillingAddress = reader["BillingAddress"].ToString(),
                        BillingCity = reader["BillingCity"].ToString(),
                        BillingState = reader["BillingAddress"].ToString(),
                        BillingCountry = reader["BillingState"].ToString(),
                        BillingPostalCode = reader["BillingPostalCode"].ToString(),
                        Total = (Decimal) reader["Total"]
                    };

                    return invoice;
                }

                return null;
            }
        }
    }
}
