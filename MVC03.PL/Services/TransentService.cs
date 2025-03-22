
namespace MVC03.PL.Services
{
    public class TransentService : ITransentService
    {
        public TransentService()
        {
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; set; }

        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}
