namespace Authentication
{
    public interface IHeaderContextService
    {
        public int GetUserDomainId();
        public string GetUserUsername();
        public int GetEudId();
    }
}
