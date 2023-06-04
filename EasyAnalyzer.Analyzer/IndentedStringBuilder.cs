using System.Text;

namespace EasyAnalyzer.Analyzer;

internal class IndentedStringBuilder
{
    private StringBuilder _o = new();
    private int _indent = 0;

    internal IDisposable IndentBraces
    {
        get
        {
            AppendLine("{");
            _indent++;
            return new DisposeAction(() =>
            {
                _indent--;
                AppendLine("}");
            });
        }
    }

    internal void AppendLine(string s)
    {
        _o.Append(new string(' ', _indent * 2));
        _o.AppendLine(s);
    }

    public override string ToString() => _o.ToString();

    private class DisposeAction : IDisposable
    {
        private readonly Action _a;
        public DisposeAction(Action a) => _a = a;
        public void Dispose() => _a();
    }
}