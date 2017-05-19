
namespace Datatrics.Module
{
    /// <summary>
    /// BaseModule class for all modules
    /// </summary>
    public class BaseModule
    {
        internal Client client;
        /// <summary>
        /// Endpoint url for the module
        /// </summary>
        public virtual string url => "";

        internal BaseModule(Client client)
        {
            this.client = client;
        }
    }
}
