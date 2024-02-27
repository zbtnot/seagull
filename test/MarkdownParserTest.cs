using Markdig;
using Markdig.Syntax;
using Seagull.Service;

namespace test;

[TestClass]
public class MarkdownParserTest
{
    [TestMethod]
    public void TestParse()
    {
        var parser = new MarkdownParserWithMocks();
        var document = parser.Parse(string.Empty, new MarkdownPipelineBuilder().Build());
        
        Assert.AreEqual(1, parser.MarkdigParseCalls);
        Assert.AreEqual(0, document.LineCount);
    }
}

class MarkdownParserWithMocks : MarkdownParser
{
    public int MarkdigParseCalls { get; set; }
    
    protected override MarkdownDocument MarkdigParse(string content, MarkdownPipeline pipeline)
    {
        MarkdigParseCalls++;
        
        return new MarkdownDocument();
    }
}
