using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transaction = VietWay.Repository.EntityModel.Transaction;

namespace VietWay.Service.ThirdParty
{
    public interface IVnPayService
    {
        public string GetPaymentUrl(Transaction transaction, string userIpAddress);
        public bool VerifyTransaction(long amount, string bankCode, string bankTranNo, string cardType,
            string orderInfo, string payDate, string responseCode, string tmnCode, string transactionNo,
            string transactionStatus, string txnRef, string secureHash);
    }
}
