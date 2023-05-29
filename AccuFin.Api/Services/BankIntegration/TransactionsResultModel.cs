namespace AccuFin.Api.Services.BankIntegration
{
    public class TransactionsResultModel
    {
        public Transactions Transactions { get; set; }
    }
    public class Transactions
    {
        public Transaction[] Booked { get; set; }
        public Transaction[] Pending { get; set; }
    }

    public class Transaction
    {
        public string TransactionId { get; set; }
        public string EntryReference { get; set; }
        public string EndToEndId { get; set; }
        public string CreditorId { get; set; }
        public string DebtorName { get; set; }
        public Debtoraccount DebtorAccount { get; set; }
        public TransactionAmount TransactionAmount { get; set; }
        public string BankTransactionCode { get; set; }
        public string BookingDate { get; set; }
        public string ValueDate { get; set; }
        public string RemittanceInformationUnstructured { get; set; }
        public string ProprietaryBankTransactionCode { get; set; }
        public string InternalTransactionId { get; set; }
    }

    public class Debtoraccount
    {
        public string Iban { get; set; }
    }

    public class TransactionAmount
    {
        public string Currency { get; set; }
        public string Amount { get; set; }
    }

}
