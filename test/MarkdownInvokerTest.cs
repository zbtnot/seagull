using Markdig;
using Markdig.Syntax;
using Seagull.Service;

namespace test;

[TestClass]
public class MarkdownInvokerTest
{
    [TestMethod]
    public void TestInvokeHtml()
    {
        var invoker = new MarkdownInvokerWithMocks();
        var html = invoker.InvokeHtml(new MarkdownDocument(), new MarkdownPipelineBuilder().Build());
        
        Assert.AreEqual(string.Empty, html);
        Assert.AreEqual(1, invoker.DocumentInvokeHtmlCalls);
    }
}

class MarkdownInvokerWithMocks : MarkdownInvoker
{
    public int DocumentInvokeHtmlCalls { get; set; }
    
    protected override string DocumentInvokeHtml(MarkdownDocument document, MarkdownPipeline pipeline)
    {
        DocumentInvokeHtmlCalls++;
        return string.Empty;
    }
}
