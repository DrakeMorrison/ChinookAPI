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
    public class DataStorage
    {
        private readonly string ConnectionString;

        public DataStorage(IConfiguration config)
        {
            ConnectionString = config.GetSection("ConnectionString").Value;
        }

        public List<Invoice> GetInvoiceByAgent(int agentId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                var result = connection.Query<Invoice>(@"
                    select
	                    agent_name = Employee.FirstName + ' ' + Employee.LastName,
	                    Invoice.*
                    from Invoice
                    join Customer
	                    on Customer.CustomerId = Invoice.CustomerId
                    join Employee
	                    on EmployeeId = SupportRepId
                    where EmployeeId = @agentId
                ", new { agentId = agentId });

                return result.ToList();
            }
        }


        public List<InvoiceInfo> GetInvoices()
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                var result = db.Query<InvoiceInfo>(@"
                    select
	                    agent_name = Employee.FirstName + ' ' + Employee.LastName,
	                    customer_name = Customer.FirstName + ' ' + Customer.LastName,
	                    country = customer.Country,
	                    Total
                    from Invoice
                    join Customer
	                    on Customer.CustomerId = Invoice.CustomerId
                    join Employee
	                    on EmployeeId = SupportRepId");

                return result.ToList();
            }
        }

        public int LineItemsPerInvoice(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                var result = db.ExecuteScalar(@"
                    select items_per_invoice = count(*)
                    from InvoiceLine
                    where InvoiceId = @id", new { id = id});

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
                    CustomerId = invoice.CustomerId,
                    BillingAddress = invoice.BillingAddress,
                    InvoiceDate = DateTime.Now,
                    BillingCity = invoice.BillingCity,
                    BillingState = invoice.BillingState,
                    BillingCountry = invoice.BillingCountry,
                    BillingPostalCode = invoice.BillingPostalCode,
                    Total = invoice.Total,
                    AgentName = invoice.AgentName
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

        public Employee GetEmployee(int id)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                return db.QueryFirst<Employee>(@"select * from employee where EmployeeId = @id", new { id = id });
            };
        }

        public bool updateEmployee(Employee employee)
        {
            using (var db = new SqlConnection(ConnectionString))
            {
                db.Open();

                var oldEmployee = GetEmployee(employee.EmployeeId);

                var newEmployee = oldEmployee;

                newEmployee.FirstName = employee.FirstName;
                newEmployee.LastName = employee.LastName;

                var result = db.Execute(@"UPDATE [dbo].[Employee]
   SET [LastName] = @LastName
      ,[FirstName] = @FirstName
      ,[Title] = @Title 
      ,[ReportsTo] = @ReportsTo
      ,[BirthDate] = @BirthDate
      ,[HireDate] = @HireDate
      ,[Address] = @Address
      ,[City] = @City
      ,[State] = @State
      ,[Country] = @Country
      ,[PostalCode] = @PostalCode
      ,[Phone] = @Phone
      ,[Fax] = @Fax
      ,[Email] = @Email
    WHERE [EmployeeId] = @EmployeeId", newEmployee);

                return result == 1;
            }
        }
    }
}