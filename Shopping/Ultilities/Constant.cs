namespace Shopping.Ultilities
{
    public static class Constant
    {
        public enum ApiType
        {
            Private = 0,
            Public = 1
        }

        
        public enum Status
        {
            Rejected = 0,
            Running = 1,      
        }

        public enum WorkFlowStatus
        {
            Rejected = 0,
            Approved = 1
        }

        public static readonly string AccessToken = "access_token";

        public static readonly string Root = "Root";
    }
}