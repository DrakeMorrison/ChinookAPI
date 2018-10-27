using ChinookAPI.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
namespace ChinookAPI.DataAccess
{
    public class InvoiceStorage
    {
        private readonly string ConnectionString;

        public InvoiceStorage(IConfiguration config)
        {
            ConnectionString = config.GetSection("ConnectionString").Value;
        }

        public List<Invoice> GetInvoiceByAgent(int agentId)
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

                if (reader.Read())
                {
                    var invoiceBox = new List<Invoice>();

                    foreach (var item in reader)
                    {
                        var invoice = new Invoice()
                        {
                            AgentName = reader["agent_name"].ToString(),
                            CustomerId = (int)reader["CustomerId"],
                            InvoiceDate = (DateTime)reader["InvoiceDate"],
                            BillingAddress = reader["BillingAddress"].ToString(),
                            BillingCity = reader["BillingCity"].ToString(),
                            BillingState = reader["BillingAddress"].ToString(),
                            BillingCountry = reader["BillingState"].ToString(),
                            BillingPostalCode = reader["BillingPostalCode"].ToString(),
                            Total = (Decimal)reader["Total"]
                        };

                        invoiceBox.Add(invoice);
                    }
                    return invoiceBox;
                }
                return null;
            }
        }

        public List<InvoiceInfo> GetInvoices()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                var command = db.CreateCommand();

                command.CommandText = @"
                    select
	                    agent_name = Employee.FirstName + ' ' + Employee.LastName,
	                    customer_name = Customer.FirstName + ' ' + Customer.LastName,
	                    country = customer.Country,
	                    Total
                    from Invoice
                    join Customer
	                    on Customer.CustomerId = Invoice.CustomerId
                    join Employee
	                    on EmployeeId = SupportRepId";

                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    var invoiceInfoBox = new List<InvoiceInfo>();

                    foreach (var item in reader)
                    {
                        var invoice = new InvoiceInfo()
                        {
                            AgentName = reader["agent_name"].ToString(),
                            CustomerName = reader["customer_name"].ToString(),
                            Country = reader["country"].ToString(),
                            Total = (Decimal)reader["Total"]
                        };

                        invoiceInfoBox.Add(invoice);
                    }
                    return invoiceInfoBox;
                }
                return null;
            }
        }

        public int LineItemsPerInvoice(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();
                var command = db.CreateCommand();

                command.CommandText = @"
                    select items_per_invoice = count(*)
                    from InvoiceLine
                    where InvoiceId = @id";

                command.Parameters.AddWithValue("@id", id);

                var result = command.ExecuteScalar();

                return (int)result;
            }
        }

        public bool InsertInvoice(Invoice invoice)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                var newInvoice = new Invoice()
                {
                    CustomerId = invoice.CustomerId != 0 ? invoice.CustomerId : 0,
                    BillingAddress = invoice.BillingAddress != null ? invoice.BillingAddress : null,
                    InvoiceDate = DateTime.Now,
                    BillingCity = invoice.BillingCity != null? invoice.BillingCity : null,
                    BillingState = invoice.BillingState != null ? invoice.BillingState : null,
                    BillingCountry = invoice.BillingCountry != null ? invoice.BillingCountry : null,
                    BillingPostalCode = invoice.BillingPostalCode != null ? invoice.BillingPostalCode : null,
                    Total = invoice.Total != decimal.Zero ? invoice.Total : decimal.Zero,
                    AgentName = invoice.AgentName != null ? invoice.AgentName : null
                };

                var result = db.Execute(@"INSERT INTO [dbo].[Invoice]
           ([CustomerId]
           ,[InvoiceDate]
           ,[BillingAddress]
           ,[BillingCity]
           ,[BillingState]
           ,[BillingCountry]
           ,[BillingPostalCode]
           ,[Total])
     VALUES
            (@customerId,
            @invoiceDate,
            @billingAddress,
            @billingCity,
            @billingState,
            @billingCountry,
            @billingPostalCode,
            @total)", newInvoice);

                return result == 1;
            };
        }

        public int updateEmployee(Employee employee)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                var newEmployee = new Employee()
                {
                    EmployeeId = 
                };
            }
        }
    }
}
