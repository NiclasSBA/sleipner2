namespace Sleipner.Cache.Netcore.Configuration.Parsers
{
    public interface IParameterParser
    {
        bool IsMatch(object value);
    }
}