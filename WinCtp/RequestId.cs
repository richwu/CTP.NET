namespace WinCtp
{
    public class RequestId
    {
        private static readonly RequestId TradeQry;
        private static readonly RequestId OrderInsert;
        private static readonly RequestId OrderQry;
        private static readonly RequestId CommonId;

        private const int Init = 100000;

        static RequestId()
        {
            OrderInsert = new RequestId(1 * Init);
            OrderQry = new RequestId(2 * Init);
            TradeQry = new RequestId(3 * Init);
            CommonId = new RequestId(9 * Init);
        }

        public static int NewId()
        {
            return CommonId.Next();
        }

        public static int OrderInsertId()
        {
            return OrderInsert.Next();
        }

        public static int TradeQryId()
        {
            return TradeQry.Next();
        }

        private int _id;

        public RequestId(int initId)
        {
            _id = initId;
        }

        public int Next()
        {
            _id++;
            return _id;
        }
    }
}