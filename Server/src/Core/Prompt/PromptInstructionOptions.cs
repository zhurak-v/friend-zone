using Core.Prompt.Types;

namespace Core.Prompt;

public class PromptInstructionOptions
{
    private bool _singleQueryOnly;
    private PromptLength _length;
    private PromptStyle _style;

    public PromptInstructionOptions
    (
        bool singleQueryOnly,
        PromptLength length,
        PromptStyle style
    )
    {
        this._singleQueryOnly = singleQueryOnly;
        this._length = length;
        this._style = style;
    }

    public bool GetSingleQueryOnly()
    {
        return this._singleQueryOnly;
    }
    public void SetSingleQueryOnly(bool singleQueryOnly)
    {
        this._singleQueryOnly = singleQueryOnly;
    }

    public PromptLength GetLength()
    {
        return this._length;
    }
    public void SetLength(PromptLength length)
    {
        this._length = length;
    }

    public PromptStyle GetStyle()
    {
        return this._style;
    }
    public void SetStyle(PromptStyle style)
    {
        this._style = style;
    }

}