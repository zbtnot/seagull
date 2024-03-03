using Seagull.Model;

namespace Seagull.Service.Contract;

public interface IIndexGenerator
{
    public string Generate(IEnumerable<Page> pages, string template, Configuration configuration);
}
