using Playground.IoC;

namespace Playground.Generation
{
    public abstract class Block
    {
        protected ServiceCollector services;

        public void Prepare(ServiceCollector serv)
        {
            services = serv;
        }
    }
}
