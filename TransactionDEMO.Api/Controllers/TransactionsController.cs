using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TransactionDEMO.Api.Manager;
using TransactionDEMO.Api.Models;
using TransactionDEMO.Api.Utils;
using TransactionDEMO.Domain.Models;

namespace TransactionDEMO.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionsController : ControllerBase
    {
        private TransactionManager _transactionMgr;
        public TransactionsController()
        {
            _transactionMgr = new TransactionManager();
        }

        [HttpPost()]
        public async Task<IActionResult> InsertTransaction(Transaction transaction)
        {
            Transaction response = await _transactionMgr.InsertTransaction(transaction);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("transactions")]
        public async Task<IActionResult> InsertTransactions(IEnumerable<Transaction> transactions)
        {
            IEnumerable<Transaction> response = await _transactionMgr.InsertTransactions(transactions);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("alltransactions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllTransactions(int page = 1, int pageSize = 10)
        {
            PagedListModel<ResponseTransaction> response = await _transactionMgr.GetAllTransactions(page, pageSize);
            if (response.Results != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("currency")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTransactionsByCurrency(string currencyCode)
        {
            List<ResponseTransaction> response = await _transactionMgr.GetTransactionsByCurrency(currencyCode);
            if (response.Count() > 0)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("date")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTransactionsByDate(DateTime startDate, DateTime endDate)
        {
            List<ResponseTransaction> response = await _transactionMgr.GetTransactionsByDate(startDate, endDate);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("status")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTransactionsByStatus(string status)
        {
            List<ResponseTransaction> response = await _transactionMgr.GetTransactionsByStatus(status);
            if (response != null)
            {
                return Ok(response);
            }
            else
            {
                return NotFound();
            }
        }

    }
}
