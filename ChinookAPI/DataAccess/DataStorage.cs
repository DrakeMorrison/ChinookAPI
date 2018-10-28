﻿using ChinookAPI.Models;
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

                //var command = connection.CreateCommand();

                //command.CommandText = @"
                //    select
                //     agent_name = Employee.FirstName + ' ' + Employee.LastName,
                //     Invoice.*
                //    from Invoice
                //    join Customer
                //     on Customer.CustomerId = Invoice.CustomerId
                //    join Employee
                //     on EmployeeId = SupportRepId
                //    where EmployeeId = @agentId
                //";

                //command.Parameters.AddWithValue("@agentId", agentId);

                //var reader = command.ExecuteReader();

                //if (reader.Read())
                //{
                //    var invoiceBox = new List<Invoice>();

                //    foreach (var item in reader)
                //    {
                //        var invoice = new Invoice()
                //        {
                //            AgentName = reader["agent_name"].ToString(),
                //            CustomerId = (int)reader["CustomerId"],
                //            InvoiceDate = (DateTime)reader["InvoiceDate"],
                //            BillingAddress = reader["BillingAddress"].ToString(),
                //            BillingCity = reader["BillingCity"].ToString(),
                //            BillingState = reader["BillingAddress"].ToString(),
                //            BillingCountry = reader["BillingState"].ToString(),
                //            BillingPostalCode = reader["BillingPostalCode"].ToString(),
                //            Total = (Decimal)reader["Total"]
                //        };

                //        invoiceBox.Add(invoice);
                //    }
                return result.ToList();

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