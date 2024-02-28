using System.Linq.Expressions;
using Markdig;
using Markdig.Syntax;
using Moq;
using Seagull.Model;
using Seagull.Service;
using Seagull.Service.Contract;

namespace test;

[TestClass]
public class MarkdownRendererServiceTest
{
    private readonly MarkdownPipeline
        _pipeline = new MarkdownPipelineBuilder().Build(); // this is eh, but moq can't mock a sealed class

    private readonly Mock<IMarkdownParser> _parser = new();
    private readonly Mock<IMarkdownInvoker> _invoker = new();
    private readonly Mock<IHtmlTemplateParser> _htmlParser = new();

    [TestMethod]
    public void TestRenderAsPage()
    {
        var markdownFile = new MarkdownFile
        {
            Contents = "Test contents",
            Path = "foo.md",
        };
        var doc = new MarkdownDocument();
        var templates = new Dictionary<string, string>
        {
            ["default"] = "<html>{{content}}</html>",
        };

        Expression<Func<IMarkdownParser, MarkdownDocument>> parse = mdp => mdp.Parse(It.IsAny<string>(), _pipeline);
        _parser.Setup(parse).Returns(doc);

        Expression<Func<IMarkdownInvoker, string>> invokeHtml = i => i.InvokeHtml(doc, _pipeline);
        _invoker.Setup(invokeHtml).Returns($"<p>{markdownFile.Contents}</p>");

        Expression<Func<IHtmlTemplateParser, HtmlTemplate>> templateParse = htp => htp.Parse(It.IsAny<string>());
        _htmlParser.Setup(templateParse).Returns(new HtmlTemplate());

        Expression<Func<IHtmlTemplateParser, string>> templateRender = htp =>
            htp.Render(It.IsAny<HtmlTemplate>(), It.IsAny<IDictionary<object, object>>());
        _htmlParser.Setup(templateRender).Returns($"<html>{markdownFile.Contents}</html>");

        var markdownFileRendererService = new MarkdownRendererService(
            _pipeline,
            _parser.Object,
            _invoker.Object,
            _htmlParser.Object
        );
        var page = markdownFileRendererService.RenderAsPage(markdownFile, templates, new Configuration());

        Assert.IsTrue(page.Content.Contains(markdownFile.Contents));
        Assert.IsTrue(page.Path.EndsWith(".html"));
        _parser.Verify(parse, Times.Once);
        _invoker.Verify(invokeHtml, Times.Once);
    }
}
